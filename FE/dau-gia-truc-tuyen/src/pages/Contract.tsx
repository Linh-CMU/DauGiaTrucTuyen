import { useMessage } from '@contexts/MessageContext';
import { createPaymentDeposit, registerForAuction } from '../queries/AuctionAPI';
import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

interface Bid {
  id: number;
  price: number;
  timestamp: string;
  userId: number;
}

const AuctionContract = () => {
  const location = useLocation();
  const { 
    companyName, companyAddress, taxCode, representativeName, 
    sellerName, sellerID, sellerAddress, productName, 
    websiteURL, effectiveDate, auctionId, auctionInfo 
  } = location.state || {};
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const [accepted, setAccepted] = useState(false);
  const navigate = useNavigate();
  const handleAccept = async(id: number) => {
    console.log('auctionInfo', auctionId);
    try {
      
      const create = await registerForAuction(id);
      if(create.isSucceed){
        const response = await createPaymentDeposit(id);
        if (response) {
            console.log(response);
            window.location.href = response;
        } else {
            setErrorMessage(response.message);
        } 
      } else {
        setErrorMessage(create.message);
    } 
            
  } catch (error) {
     setErrorMessage('Erro add infomation')
  }
    setAccepted(true);
    // Thực hiện hành động khi chấp nhận hợp đồng (có thể lưu vào cơ sở dữ liệu hoặc thông báo)
  };
  const handleCancel = () => {
    navigate(-1);
    // Thực hiện hành động khi chấp nhận hợp đồng (có thể lưu vào cơ sở dữ liệu hoặc thông báo)
  };
  // const [bidHistory, setBidHistory] = useState<Bid[]>([]);
  // useEffect(() => {
  //   // Establish WebSocket connection
  //   const socket = new WebSocket(`ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=1`);

  //   // On receiving a message from WebSocket
  //   socket.onmessage = (event) => {
  //     const data = JSON.parse(event.data);

  //     // Kiểm tra xem data có phải là mảng hay không
  //     if (Array.isArray(data)) {
  //       setBidHistory(data);
  //       console.log('data', data);
        
  //     } else {
  //       console.error('Received data is not an array:', data);
  //     }
  //   };

  //   // Cleanup WebSocket connection on component unmount
  //   return () => {
  //     socket.close();
  //   };
  // }, []);
  return (
    <div className="p-8 max-w-3xl mx-auto bg-white rounded-xl shadow-lg space-y-6 mt-10">
      <h1 className="text-3xl font-bold text-center text-gray-900">
        CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM
      </h1>
      <h2 className="text-xl text-center mt-2">Độc lập - Tự do - Hạnh phúc</h2>
      <p className="text-center mt-1">-------oOo-------</p>

      <h3 className="text-3xl font-bold text-center text-gray-900 mt-6">
        HỢP ĐỒNG ĐĂNG KÝ THAM GIA ĐẤU GIÁ
      </h3>

      <section className="space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 1: Các bên tham gia</h2>
        <p>
          <strong>Bên A</strong> (Bên cung cấp nền tảng đấu giá): Công ty <span className="italic">{companyName}</span>, 
          địa chỉ: <span className="italic">{companyAddress}</span>, mã số thuế: <span className="italic">{taxCode}</span>, 
          đại diện bởi ông/bà <span className="italic">{representativeName}</span>.
        </p>
        <p>
          <strong>Bên B</strong> (Người bán): <span className="italic">{sellerName}</span>, 
          CMND/CCCD số: <span className="italic">{sellerID}</span>, địa chỉ: <span className="italic">{sellerAddress}</span>.
        </p>
      </section>

      <section className="space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 2: Thông tin về đấu giá</h2>
        {auctionInfo && auctionInfo.map((item: any, index: any) => (
          <p key={index}><strong>{item.label}:</strong> {item.value}</p>
        ))}
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 3: Quy định về tiền đặt trước</h2>
        <p>Bên B đồng ý thanh toán tiền đặt trước là: <span className="italic">{auctionInfo?.find((item: any) => item.label === 'Tiền đặt trước')?.value}</span></p>
        <p><strong>Lưu ý:</strong> Tiền đặt trước là một khoản không hoàn lại, trừ khi có quy định khác từ Bên A. Nếu Bên B không tham gia đấu giá đúng thời gian quy định, tiền đặt trước sẽ không được hoàn trả.</p>
      </section>

      <section className="space-y-4 mt-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 4: Quyền và nghĩa vụ của các bên</h2>
        <p>
          <strong>Bên A</strong> có quyền: 
          <ul className="list-disc pl-6">
            <li>Cung cấp nền tảng đấu giá trực tuyến cho Bên B.</li>
            <li>Xử lý các vấn đề liên quan đến quá trình đấu giá, bao gồm việc xác minh các giao dịch và bảo mật dữ liệu.</li>
            <li>Cập nhật thông tin đấu giá kịp thời và thông báo về bất kỳ thay đổi nào.</li>
          </ul>
        </p>
        <p>
          <strong>Bên B</strong> có nghĩa vụ:
          <ul className="list-disc pl-6">
            <li>Thực hiện thanh toán tiền đặt trước đúng hạn.</li>
            <li>Tham gia đấu giá đúng thời gian quy định.</li>
            <li>Cung cấp thông tin chính xác và đầy đủ về sản phẩm đấu giá.</li>
          </ul>
        </p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 5: Phương thức và điều kiện đấu giá</h2>
        <p><strong>Phương thức đấu giá:</strong> Trả giá không xác định vòng. Người tham gia đấu giá có thể đặt giá bất kỳ lúc nào trong suốt thời gian đấu giá.</p>
        <p><strong>Điều kiện tham gia:</strong> Người tham gia phải thanh toán tiền đặt trước trước khi tham gia đấu giá. Bên A có quyền từ chối các cá nhân không hoàn tất thủ tục đăng ký.</p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 6: Chấp nhận và hiệu lực hợp đồng</h2>
        <p>Hợp đồng này có hiệu lực kể từ ngày Bên B chấp nhận và thanh toán tiền đặt trước.</p>
        <p>Hợp đồng có thể được thay đổi hoặc bổ sung nếu có thỏa thuận giữa các bên và phải được ghi nhận bằng văn bản.</p>
      </section>

      <section className="space-y-4 mt-6">
        <h2 className="text-xl font-semibold text-gray-700">Điều 7: Điều khoản chung</h2>
        <p><strong>Giải quyết tranh chấp:</strong> Mọi tranh chấp phát sinh từ hợp đồng này sẽ được giải quyết bằng thương lượng. Nếu không thể giải quyết được, sẽ đưa ra tòa án có thẩm quyền giải quyết.</p>
      </section>

      <section className="flex justify-center mt-6">
        <button
          onClick={() => handleAccept(auctionId)}
          className={`px-8 py-2 font-semibold rounded-md bg-green-500 text-white`}
        >
          Đã chấp nhận hợp đồng
        </button>
        <button
          onClick={handleCancel}
          className='px-8 py-2 font-semibold rounded-md bg-red-500 text-white ml-4'
        >
          Hủy
        </button>
      </section>
    </div>
  );
};


export default AuctionContract;
