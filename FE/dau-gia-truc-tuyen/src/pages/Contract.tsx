import { useMessage } from '@contexts/MessageContext';
import { createPaymentDeposit, registerForAuction } from '../queries/AuctionAPI';
import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { getCity, getDistrict, getWard, inforUser, profileUser } from '@queries/AdminAPI';
import { cityResponse, districtResponse, profileResponse, wardResponse } from '../types/auth.type';

interface Bid {
  id: number;
  price: number;
  timestamp: string;
  userId: number;
}

const AuctionContract = () => {
  const location = useLocation();
  const {
    companyName,
    companyAddress,
    taxCode,
    representativeName,
    sellerName,
    sellerID,
    sellerAddress,
    deposit,
    owner,
    effectiveDate,
    auctionId,
    auctionInfo,
  } = location.state || {};

  const { setErrorMessage, setSuccessMessage } = useMessage();
  const navigate = useNavigate();

  const [profile, setProfile] = useState<profileResponse | null>(null);
  const [locationData, setLocationData] = useState({
    cities: [] as cityResponse[],
    districts: [] as districtResponse[],
    wards: [] as wardResponse[],
  });
  const [filteredDistricts, setFilteredDistricts] = useState<districtResponse[]>([]);
  const [filteredDistrict, setFilteredDistrict] = useState<districtResponse[]>([]);
  const [filteredWards, setFilteredWards] = useState<cityResponse[]>([]);
  const [filteredWard, setFilteredWard] = useState<cityResponse[]>([]);
  const [accepted, setAccepted] = useState(false);

  const handleAccept = async (id: number) => {
    try {
      const createResponse = await registerForAuction(id);
      if (createResponse.isSucceed) {
        const paymentResponse = await createPaymentDeposit(id);
        if (paymentResponse) {
          window.location.href = paymentResponse;
        } else {
          setErrorMessage(paymentResponse.message);
        }
      } else {
        setErrorMessage(createResponse.message);
      }
    } catch (error) {
      setErrorMessage('Error adding information');
    }
    setAccepted(true);
  };

  const fetchData = async () => {
    try {
      const [cityData, districtData, wardData, profileData] = await Promise.all([
        getCity(),
        getDistrict(),
        getWard(),
        inforUser(owner),
      ]);
      setLocationData({ cities: cityData, districts: districtData, wards: wardData });
      setProfile(profileData.result);
    } catch (error) {
      setErrorMessage('Error fetching data');
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  useEffect(() => {
    if (profile) {
      const districts = locationData.districts.filter(
        (district) => district.code.toString() === profile.city
      );
      const wards = locationData.cities.filter((ward) => ward.code.toString() === profile.city);
      console.log('wards', wards);

      setFilteredDistricts(districts);
      setFilteredWards(wards);
    }
  }, [profile, locationData]);
  const [profiles, setProfiles] = useState<profileResponse | null>();
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await profileUser();
        setProfiles(response.result);
      } catch (error) {}
    };
    fetchData();
  }, []);
  useEffect(() => {
    if (profiles) {
      const districts = locationData.districts.filter(
        (district) => district.code.toString() === profiles.city
      );
      const wards = locationData.cities.filter((ward) => ward.code.toString() === profiles.city);
      console.log('wards', wards);

      setFilteredDistrict(districts);
      setFilteredWard(wards);
    }
  }, [profiles, locationData]);
  const handleCancel = () => navigate(-1);
  return (
    <div className="p-8 max-w-3xl mx-auto bg-white rounded-xl shadow-lg space-y-6 mt-16">
      <h1 className="text-3xl font-bold text-center text-gray-900">
        CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
      </h1>
      <h2 className="text-xl text-center mt-2">Độc lập - Tự do - Hạnh phúc</h2>
      <p className="text-center mt-1">-------oOo-------</p>

      <h3 className="text-3xl font-bold text-center text-gray-900 mt-6">
        THỎA THUẬN ĐĂNG KÝ THAM GIA ĐẤU GIÁ
      </h3>

      <section className="space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 1: Các bên tham gia</h2>
        <p>
          <strong>Bên A</strong> (Bên cung cấp nền tảng đấu giá): Công ty{' '}
          <span className="italic">{companyName}</span>, địa chỉ:{' '}
          <span className="italic">{companyAddress}</span>, mã số thuế:{' '}
          <span className="italic">{taxCode}</span>, đại diện bởi ông/bà{' '}
          <span className="italic">{representativeName}</span>.
        </p>
        <p>
          <strong>Bên B</strong> Họ tên người bán:{' '}
          <span className="italic">{profile?.fullName}</span>.
          <br />
          Địa chỉ:{' '}
          <span className="italic">
            {filteredWards[0]?.name} - {filteredDistricts[0]?.name} - {profile?.ward} -{' '}
            {profile?.address}
          </span>
          .
          <br />
          SĐT: <span className="italic">{profile?.phone}</span>.
        </p>
        <p>
          <strong>Bên C</strong> Họ tên người tham gia:{' '}
          <span className="italic">{profiles?.fullName}</span>.
          <br />
          Địa chỉ:{' '}
          <span className="italic">
            {filteredWard[0]?.name} - {filteredDistrict[0]?.name} - {profiles?.ward} -{' '}
            {profiles?.address}
          </span>
          .
          <br />
          SĐT: <span className="italic">{profiles?.phone}</span>.
        </p>
      </section>

      <section className="space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 2: Thông tin về đấu giá</h2>
        {auctionInfo &&
          auctionInfo.map((item: any, index: any) => (
            <p key={index}>
              <strong>{item.label}:</strong> {item.value}
            </p>
          ))}
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 3: Quy định về tiền đặt trước</h2>
        <p>
          Bên B đồng ý thanh toán tiền đặt trước là:{' '}
          <span className="italic">
            {deposit
              .toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND',
              })
              .replace('₫', '')}
            VNĐ
          </span>
        </p>
        <p>
          <strong>Lưu ý:</strong> Tiền đặt trước là một khoản không hoàn lại, trừ khi có quy định
          khác từ Bên A. Nếu Bên B không tham gia đấu giá đúng thời gian quy định, tiền đặt trước sẽ
          không được hoàn trả.
        </p>
      </section>

      <section className="space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-gray-700">
          Điều 4: Quyền và nghĩa vụ của các bên
        </h2>
        <p>
          <strong>Bên A</strong> có quyền:
          <ul className="list-disc pl-6">
            <li>Cung cấp nền tảng đấu giá trực tuyến cho Bên B.</li>
            <li>
              Xử lý các vấn đề liên quan đến quá trình đấu giá, bao gồm việc xác minh các giao dịch
              và bảo mật dữ liệu.
            </li>
            <li>Cập nhật thông tin đấu giá kịp thời và thông báo về bất kỳ thay đổi nào.</li>
          </ul>
        </p>
        <p>
          <strong>Bên B</strong> có nghĩa vụ:
          <ul className="list-disc pl-6">
            <li>Cung cấp vật phẩm đấu giá và chứng nhận thẩm định vật phẩm .</li>
            <li>Đưa ra bước giá khởi điểm cho phiên đấu giá .</li>
            <li>Cung cấp thông tin chính xác và đầy đủ về sản phẩm đấu giá .</li>
          </ul>
        </p>
        <p>
          <strong>Bên C</strong> có nghĩa vụ:
          <ul className="list-disc pl-6">
            <li>Thực hiện thanh toán tiền đặt trước đúng hạn.</li>
            <li>Tham gia đấu giá đúng thời gian quy định.</li>
            <li>Cung cấp thông tin cá nhân chính xác và đầy đủ.</li>
          </ul>
        </p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">
          Điều 8: Phương thức và điều kiện đấu giá
        </h2>
        <p>
          <strong>Phương thức đấu giá:</strong> Trả giá không xác định vòng. Người tham gia đấu giá
          có thể đặt giá bất kỳ lúc nào trong suốt thời gian đấu giá.
        </p>
        <p>
          <strong>Điều kiện tham gia:</strong> Người tham gia phải thanh toán tiền đặt trước trước
          khi tham gia đấu giá. Bên A có quyền từ chối các cá nhân không hoàn tất thủ tục đăng ký.
        </p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">
          Điều 6: Chấp nhận và hiệu lực thỏa thuận
        </h2>
        <p>
          Thỏa thuận này có hiệu lực kể từ ngày bên C chấp nhận và thanh toán tiền đặt cọc trước.
        </p>
        <p>
          Thỏa thuận có thể được thay đổi hoặc bổ sung nếu có thêm thỏa thuận giữa các bên và phải
          được ghi nhận bằng văn bản .
        </p>
      </section>

      <section className="space-y-4 mt-6">
        <h2 className="text-xl font-semibold text-gray-700">Điều 7: Điều khoản chung</h2>
        <p>
          <strong>Giải quyết tranh chấp:</strong> Mọi tranh chấp phát sinh từ thỏa thuận này sẽ được
          giải quyết bằng thương lượng. Nếu không thể giải quyết được sẽ đưa ra tòa án có thẩm quyền
          giải quyết.
        </p>
      </section>
      <section className="space-y-4 mt-6">
        <h2 className="text-xl font-semibold text-gray-700 text-center">
          Điều 8: Xác nhận và chữ ký của các bên
        </h2>
        <div className="flex justify-between mt-8">
          <div className="text-center">
            <p className="font-semibold">BÊN A</p>
            <p className="italic">Công ty cung cấp nền tảng</p>
            <div className="mt-16">
              <p>_________________________</p>
              <p>Chữ ký & dấu</p>
            </div>
          </div>
          <div className="text-center">
            <p className="font-semibold">BÊN B</p>
            <p className="italic">Người bán</p>
            <div className="mt-16">
              <img
                src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile?.signature}`}
                alt="Signature"
                style={{
                  width: '100px',
                  height: '50px',
                  marginLeft: 'auto',
                  marginRight: 'auto',
                }}
                className="signature-image"
              />
              <p>_________________________</p>
              <p>{profile?.fullName}</p>
            </div>
          </div>
          <div className="text-center">
            <p className="font-semibold">BÊN C</p>
            <p className="italic">Người tham gia đấu giá</p>
            <div className="mt-16">
              <img
                src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profiles?.signature}`}
                alt="Signature"
                style={{
                  width: '100px',
                  height: '50px',
                  marginLeft: 'auto',
                  marginRight: 'auto',
                }}
                className="signature-image"
              />
              <p>_________________________</p>
              <p>{profiles?.fullName}</p>
            </div>
          </div>
        </div>
      </section>

      <section className="flex justify-center mt-6">
        <button
          onClick={() => handleAccept(auctionId)}
          className={`px-8 py-2 font-semibold rounded-md bg-green-500 text-white`}
        >
          Chấp nhận hợp đồng
        </button>
        <button
          onClick={handleCancel}
          className="px-8 py-2 font-semibold rounded-md bg-red-500 text-white ml-4"
        >
          Hủy
        </button>
      </section>
    </div>
  );
};

export default AuctionContract;
