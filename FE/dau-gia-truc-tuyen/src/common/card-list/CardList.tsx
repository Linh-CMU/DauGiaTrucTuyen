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
  isApproved?: string;
  url: string
}

const CardList = ({
  id,
  isProperties = true,
  title,
  imgSrc,
  priceStart,
  startDay,
  targetDate,
  isApproved, 
  url,
}: CardListProps) => {
  const navigate = useNavigate();

  const handleDetailClick = () => {
    navigate(`/${url}/${id}`);
  };

  const renderPrice = () => (
    <div className="flex justify-between text-l">
      <div className="font-thin">Giá khởi điểm</div>
      <div className="font-bold">
        {Number(priceStart).toLocaleString('vi-VN', {
          style: 'currency',
          currency: 'VND',
          minimumFractionDigits: 0,  // Optional: removes decimal places for VND
        }).replace('₫', '')} VNĐ
      </div>
    </div>
  );  

  return (
    <div className="max-w-xs bg-white rounded-lg shadow-lg overflow-hidden group border border-transparent hover:border-blue-500">
  <div className="relative">
    <div className="group hover:cursor-pointer relative" onClick={handleDetailClick}>
      {/* Overlay trạng thái "approved" */}
      {isApproved && (
        <div className="absolute top-2 left-2 bg-green-600 text-white font-semibold px-2 py-1 rounded">
          {isApproved}
        </div>
      )}
      <img
        className="w-full h-64 object-cover transition-transform duration-500 transform group-hover:scale-105"
        src={
          isProperties
            ? `http://capstoneauctioneer.runasp.net/api/read?filePath=${imgSrc}`
            : news
        }
        alt="Art Auction"
      />
    </div>

    {isProperties ? (
      <div className="absolute bottom-5 left-1/2 transform -translate-x-1/2 bg-white bg-opacity-90 p-2 rounded-full w-11/12 flex justify-center items-center gap-4 group-hover:hidden">
        <CountdownTimer targetDate={targetDate} />
      </div>
    ) : (
      <div className="absolute bottom-0 bg-black/60 text-white text-xs w-full p-2">
        {startDay}
      </div>
    )}
  </div>

  <div className="p-4">
    <h2 className="text-gray-900 font-semibold text-lg line-clamp-1">
      {title || "Cho thuê Tầng 1 (sảnh) của Cơ quan Thông tấn xã Việt Nam khu vực Miền Trung - Tây Nguyên"}
    </h2>
    <p className="mt-2 text-gray-600">
      <span className="text-black font-bold">
        {renderPrice() || "36.000.000 VNĐ"}
      </span>
    </p>
    <button
      className="mt-4 w-full bg-black text-white font-bold py-2 px-4 rounded group-hover:bg-sky-700"
      onClick={handleDetailClick}
    >
      Đấu giá
    </button>
  </div>
</div>

  );
};

export default CardList;
