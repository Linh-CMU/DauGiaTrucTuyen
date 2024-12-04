import {AuctionDetailRegister } from 'types'
interface DetailContentProps {
  auctionDetailInfor: AuctionDetailRegister | null; // Use shared Auction type
}


const DetailContentUser: React.FC<DetailContentProps> = ({auctionDetailInfor}) => {
  return (
    <div className="mx-auto bg-white p-6 shadow-lg rounded-md w-full">
          <h1 className="text-2xl font-bold text-center text-blue-800 mb-4">THÔNG BÁO ĐẤU GIÁ TÀI SẢN</h1>
          
          <div className="mb-6">
              <p><span className="font-semibold">{auctionDetailInfor?.nameAuction}</span></p>
              <p className="text-red-600 font-semibold">Giá khởi điểm: {auctionDetailInfor?.startingPrice} đồng</p>
              <p className="text-sm text-gray-700 italic">* Lưu ý: Người trúng đấu giá nhận tài sản theo hiện trạng thực tế hiện có, không qua cân. Giá chưa bao gồm chi phí tháo dỡ, vận chuyển, bốc xếp, phương tiện vận chuyển và các chi phí phát sinh khác (nếu có).</p>
          </div>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">1. Tên, địa chỉ tổ chức đấu giá:</h2>
          <p className="mb-4">Trung tâm dịch vụ đấu giá tài sản thành phố Đà Nẵng - Số 08 Phan Bội Châu, quận Hải Châu, thành phố Đà Nẵng.</p>
  
          {/* <h2 className="text-lg font-semibold text-gray-800 mb-3">2. Tên, địa chỉ đơn vị có tài sản:</h2>
          <p className="mb-4">Công ty cổ phần Rượu và Nước giải khát Hà Nội, địa chỉ: 94 Lò Đúc, phường Phạm Đình Hổ, quận Hai Bà Trưng, thành phố Hà Nội.</p> */}
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">2. Tài sản, giá khởi điểm tài sản đấu giá:</h2>
          <p className="mb-4"> {auctionDetailInfor?.description} <br/> Giá khởi điểm: {auctionDetailInfor?.startingPrice} đồng.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">3. Nơi có tài sản đấu giá:</h2>
          <p className="mb-4">Số 08 Phan Bội Châu, quận Hải Châu, thành phố Đà Nẵng.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">4. Thời gian, địa điểm xem tài sản đấu giá:</h2>
          <p className="mb-2"><span className="font-semibold">Thời gian:</span> từ ngày {auctionDetailInfor?.startDay} đến hết ngày {auctionDetailInfor?.endDay}.</p>
          <p className="mb-4"><span className="font-semibold">Địa điểm:</span> Số 08 Phan Bội Châu, quận Hải Châu, thành phố Đà Nẵng.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">5. Thời gian, điều kiện, cách thức đăng ký tham gia đấu giá:</h2>
          <p className="mb-2"><span className="font-semibold">Thời gian:</span> đến {auctionDetailInfor?.endTime} ngày  {auctionDetailInfor?.endDay}</p>
          <p className="mb-2"><span className="font-semibold">Điều kiện:</span> Cá nhân, tổ chức có hồ sơ hợp lệ và nộp tiền mua hồ sơ, tiền đặt trước.</p>
          {/* <p className="mb-4"><span className="font-semibold">Cách thức:</span> Truy cập website <a href="https://daugia.danang.gov.vn" className="text-blue-600 underline">daugia.danang.gov.vn</a> để đăng ký trực tuyến.</p> */}
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">6. Thời gian, địa điểm bán, nộp tiền mua hồ sơ tham gia đấu giá:</h2>
          <p className="mb-4">Từ ngày thông báo đến 17:00 ngày 28/10/2024, nộp tiền mặt tại Trung tâm đấu giá hoặc chuyển khoản vào tài khoản của Trung tâm.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">7. Tiền bán hồ sơ tham gia đấu giá, tiền đặt trước, bước giá:</h2>
          <p className="mb-2"><span className="font-semibold">Tiền bán hồ sơ:</span> 500.000 đồng/bộ hồ sơ.</p>
          <p className="mb-2"><span className="font-semibold">Tiền đặt trước:</span> {auctionDetailInfor?.startingPrice || 0 * 0.1} đồng.</p>
          <p className="mb-4"><span className="font-semibold">Bước giá:</span> {auctionDetailInfor?.stepPrice} đồng.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">8. Thời gian, địa điểm tổ chức cuộc đấu giá:</h2>
          <p className="mb-4"><span className="font-semibold">Địa điểm:</span> Trực tuyến tại <a href="http://localhost:5175/" className="text-blue-600 underline">Đấu giá trực tuyến</a>.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">9. Hình thức, phương thức đấu giá:</h2>
          <p className="mb-4">Đấu giá trực tuyến; Phương thức trả giá lên với thời gian {auctionDetailInfor?.timePerLap}.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">10. Địa điểm:</h2>
          <p className="mb-4">Trung tâm dịch vụ đấu giá tài sản thành phố Đà Nẵng - Số 08 Phan Bội Châu, quận Hải Châu, Đà Nẵng. Điện thoại: 0236-3889627 – 3887678. Website: <a href="http://localhost:5175/" className="text-blue-600 underline">Đấu giá trực tuyến</a>.</p>
  
          <p className="text-sm text-gray-700 italic">Giờ hành chính: Thứ 2 - Thứ 6, Sáng: 07:30 - 11:30, Chiều: 13:30 - 17:30 (trừ ngày lễ).</p>
      </div>
      )
}
export default DetailContentUser;