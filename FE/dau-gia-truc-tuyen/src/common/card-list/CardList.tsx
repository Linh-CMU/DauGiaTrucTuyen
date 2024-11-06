import * as React from 'react';
import CountdownTimer from '../coutdown-timer/CountdownTimer';
import news from '../../../public/news.webp';
import { useNavigate } from 'react-router-dom';

interface CardListProps {
  id: string;
  isProperties: boolean;
  imgSrc: string;
  title: string;
  priceStart: string;
  startDay: string;
  targetDate: Date;
}

const CardList = ({
  id,
  isProperties = true,
  title,
  imgSrc,
  priceStart,
  startDay,
  targetDate,
}: CardListProps) => {
  // const targetDate = new Date('2024-12-31T23:59:59');
  // console.log(targetDate,"targetDate") 
  const navigate = useNavigate();

  const handleDetailClick = () => {
    navigate(`/thong-tin-chi-tiet/${id}`);
  };

  const renderPrice = () => (
    <div className="flex justify-between text-l">
      <div className="font-thin">Giá khởi điểm</div>
      <div className="font-bold">{priceStart} VNĐ</div>
    </div>
  );

  return (
    <div className="max-w-sm rounded overflow-hidden shadow-lg">
      <div className="relative">
        <div
          className="group hover:cursor-pointer relative"
          onClick={handleDetailClick}
        >
          <img
            className="w-full mb-4"
            src={
              isProperties
                ? `http://capstoneauctioneer.runasp.net/api/read?filePath=${imgSrc}`
                : news
            }
            alt="img-properties"
          />
          <div className="absolute inset-0 bg-black opacity-0 group-hover:opacity-80 transition-opacity duration-300 flex justify-center items-center">
            <span className="text-white font-semibold text-lg">Xem chi tiết</span>
          </div>
        </div>

        {isProperties ? (
          <div className="absolute bottom-[-2rem]">
            <CountdownTimer targetDate={targetDate} />
          </div>
        ) : (
          <div className="absolute bottom-0 bg-black/60 text-white text-xs w-full p-2">
            {startDay}
          </div>
        )}
      </div>

      <div className="px-3 py-4">
        <div className="font-bold text-l mb-2 line-clamp-2 hover:text-orange-400 hover:cursor-pointer">
          {title}
        </div>

        {isProperties ? (
          renderPrice()
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
