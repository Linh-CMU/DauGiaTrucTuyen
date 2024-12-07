import {AuctionDetailRegister } from 'types'
interface DetailContentProps {
  auctionDetailInfor: AuctionDetailRegister | null; // Use shared Auction type
}


const DetailContentUser: React.FC<DetailContentProps> = ({auctionDetailInfor}) => {
  return (
    <div className="mx-auto bg-white p-6 shadow-lg rounded-md w-full">
          <h1 className="text-2xl font-bold text-center text-blue-800 mb-4">AUCTION NOTICE</h1>
          
          <div className="mb-6">
              <p><span className="font-semibold">{auctionDetailInfor?.nameAuction}</span></p>
              <p className="text-red-600 font-semibold">Starting price: {auctionDetailInfor?.startingPrice.toLocaleString('vi-VN', {
              style: 'currency',
              currency: 'VND',
            })
            .replace('₫', '')}
          VNĐ</p>
              <p className="text-sm text-gray-700 italic">* Note: The auction winner will receive the property in its actual condition, without weighing. The price does not include the cost of dismantling, transportation, loading and unloading, means of transport and other costs (if any).</p>
          </div>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">1. Name and address of auction organization:</h2>
          <p className="mb-4">Trung tâm dịch vụ đấu giá tài sản thành phố Đà Nẵng - Số 08 Phan Bội Châu, quận Hải Châu, thành phố Đà Nẵng.</p>
  
          {/* <h2 className="text-lg font-semibold text-gray-800 mb-3">2. Tên, địa chỉ đơn vị có tài sản:</h2>
          <p className="mb-4">Công ty cổ phần Rượu và Nước giải khát Hà Nội, địa chỉ: 94 Lò Đúc, phường Phạm Đình Hổ, quận Hai Bà Trưng, thành phố Hà Nội.</p> */}
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">2. Assets, starting price of auctioned assets:</h2>
          <p className="mb-4"> {auctionDetailInfor?.description} <br/> Starting price: {auctionDetailInfor?.startingPrice.toLocaleString('vi-VN', {
              style: 'currency',
              currency: 'VND',
            })
            .replace('₫', '')}
          VNĐ</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">4. Auction registration time:</h2>
          <p className="mb-2"><span className="font-semibold">Time:</span> start date {auctionDetailInfor?.startDay} end date {auctionDetailInfor?.endDay}.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">5. Time, conditions and method of registration to participate in the auction:</h2>
          <p className="mb-2"><span className="font-semibold">Time:</span> to {auctionDetailInfor?.endTime} date  {auctionDetailInfor?.endDay}</p>
          <p className="mb-2"><span className="font-semibold">Condition:</span> Individuals and organizations have valid documents and pay for documents and deposit.</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">6. Bidding document sale fee, deposit, price step:</h2>
          <p className="mb-2"><span className="font-semibold">Deposit:</span> {auctionDetailInfor?.startingPrice.toLocaleString('vi-VN', {
              style: 'currency',
              currency: 'VND',
            })
            .replace('₫', '')}
          VNĐ</p>
          <p className="mb-4"><span className="font-semibold">Price step:</span> {auctionDetailInfor?.stepPrice ? auctionDetailInfor?.stepPrice.toLocaleString('vi-VN', {
              style: 'currency',
              currency: 'VND',
            })
            .replace('₫', '')
           : 0} VNĐ</p>
  
          <h2 className="text-lg font-semibold text-gray-800 mb-3">7. Auction form and method:</h2>
          <p className="mb-4">Online Auction; Bidding method increases with time {auctionDetailInfor?.timePerLap} clock.</p>
          <p className="text-sm text-gray-700 italic">Office hours: Monday - Friday, Morning: 07:30 - 11:30, Afternoon: 13:30 - 17:30 (except holidays).</p>
      </div>
      )
}
export default DetailContentUser;