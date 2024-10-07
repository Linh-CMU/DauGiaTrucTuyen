import * as React from 'react';
import CountdownTimer from '../coutdown-timer/CountdownTimer';
import news from '../../../public/news.webp';
import { useNavigate } from 'react-router-dom';


interface CardListProps {
  id: string,
  isProperties: boolean;
  // imgSrc: string,
  // title: string,
  // priceStart: string,

}

const CardList = ({ isProperties = true, id }: CardListProps) => {
  const targetDate = new Date('2024-12-31T23:59:59');
     const navigate = useNavigate();

  const handleDetailClick = () => {
    navigate(`/thong-tin-chi-tiet/${id}`);
  };

  return (
    <div className="max-w-sm rounded overflow-hidden shadow-lg">
      <div className="relative">
        <div 
        className="group hover:cursor-pointer relative"
        onClick={handleDetailClick}
        >
          {isProperties ? (
            <img className="w-full mb-4" src="http://capstoneauctioneer.runasp.net/api/read?filePath=ListAuctioneer/1dd89b64-6060-44a3-b67e-4ed1d719849d_20241006164359_26082024102501739DewQXfcxrgPw3ca3.png" alt="img-properties" />
          ): (
              <img className="w-full mb-4" src={news} alt="img-properties" />
          )}
          <div className="absolute inset-0 bg-black opacity-0 group-hover:opacity-80 transition-opacity duration-300 flex justify-center items-center">
            <span className="text-white font-semibold text-lg">Xem chi tiết</span>
          </div>
        </div>
        {isProperties ? (
          <div style={{ position: 'absolute', bottom: '-2rem' }}>
            <CountdownTimer targetDate={targetDate} />
          </div>
        ) : (
          <div
            className="text-white text-xs w-full bottom-0 p-2"
            style={{ position: 'absolute', backgroundColor: '#00000061' }}
          >
            16/09/2024
          </div>
        )}
      </div>

      <div className="px-3 py-4">
        <div className="font-bold text-l mb-2 line-clamp-2 hover:text-orange-400 hover:cursor-pointer">
          Cho thuê Tầng 1 (sảnh) của Cơ quan Thông tấn xã Việt Nam khu vực Miền Trung - Tây Nguyên:
          Diện tích 100 m2 x đơn giá 363.000 đồng/1m2/1 tháng = 36.300.000 đồng/tháng.
        </div>
        {isProperties ? (
          <div className="flex space justify-between text-l">
            <div className="font-thin"> Giá khởi điểm</div>
            <div className="font-bold"> 36.000.000 VNĐ</div>
          </div>
        ) : (
          <div className="font-thin line-clamp-4">
            Giá khởi điểm: 2.850.997.500 đồng (Bằng chữ: Hai tỷ, tám trăm năm mươi triệu, chín trăm
            chín mươi bảy nghìn, năm trăm đồng). Giá bán tài sản chưa bao gồm các loại thuế, phí
            liên quan đến việc chuyển quyền sở hữu.
          </div>
        )}
      </div>
    </div>
  );
};
export default CardList;
