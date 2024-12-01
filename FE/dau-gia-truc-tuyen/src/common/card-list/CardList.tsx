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
  endDay: string;
  startDay: string;
  endTime: string;
  targetDate: Date;
  isApproved?: string;
  url: string;
}

const CardList = ({
  id,
  isProperties = true,
  title,
  imgSrc,
  priceStart,
  endDay,
  endTime,
  startDay,
  targetDate,
  isApproved,
  url,
}: CardListProps) => {
  const navigate = useNavigate();

  const handleDetailClick = () => {
    if(url === 'thong-tin-chi-tiet'){
      navigate(`/${url}/${id}`);
    }else if(url === 'phien-dau-gia' && isEndTimePassed(endTime, endDay)){
      navigate(`/${url}/${id}`);
    }
  };

  const renderPrice = () => (
    <div className="flex justify-between text-l">
      <div className="font-thin">Giá khởi điểm</div>
      <div className="font-bold">
        {Number(priceStart)
          .toLocaleString('vi-VN', {
            style: 'currency',
            currency: 'VND',
            minimumFractionDigits: 0, // Optional: removes decimal places for VND
          })
          .replace('₫', '')}{' '}
        VNĐ
      </div>
    </div>
  );

  const calculateFinalTime = (timeEnd: string = '', endDay: string = ''): Date => {
    if (!timeEnd || !endDay) {
      console.error('timeEnd or endDay is missing:', timeEnd, endDay);
      return new Date(); // Return current date if either is missing
    }

    // Chuyển đổi định dạng từ "DD/MM/YYYY" thành "YYYY-MM-DD"
    const [day, month, year] = endDay.split('/').map(Number);
    const formattedEndDay = `${year}-${month.toString().padStart(2, '0')}-${day.toString().padStart(2, '0')}`;
    const endDateTimeString = `${formattedEndDay}T${timeEnd}:00`; // Thêm 'T' giữa ngày và giờ

    const endDate = new Date(endDateTimeString); // Tạo đối tượng Date hợp lệ

    // Kiểm tra nếu đối tượng Date hợp lệ
    if (isNaN(endDate.getTime())) {
      console.error('Invalid endDate:', endDateTimeString);
      return new Date(); // Nếu không hợp lệ, trả về ngày hiện tại
    }
    return endDate; // Trả về đối tượng Date đã được tính toán
  };

  const isEndTimePassed = (endTime: string = '', endDay: string = ''): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);

    return finalTime <= new Date(); // Kiểm tra nếu thời gian cuối đã qua
  };

  return (
    <div className="max-w-xs bg-white rounded-lg shadow-lg overflow-hidden group border border-transparent hover:border-blue-500">
      <div className="relative">
        <div className="group hover:cursor-pointer relative" onClick={handleDetailClick}>
          {/* Overlay trạng thái "approved" */}
          {isApproved && (
            <div className="absolute top-2 left-2 bg-green-600 text-white font-semibold px-2 py-1 rounded" style={{zIndex : '2'}}>
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
          <div
            className={`absolute bottom-5 left-1/2 transform -translate-x-1/2 ${
              !isEndTimePassed(endTime, endDay)
                ? 'bg-green-500'
                : targetDate > new Date()
                  ? 'bg-orange-500'
                  : 'bg-yellow-500'
            } bg-opacity-90 p-2 rounded-full w-11/12 flex justify-center items-center gap-4 group-hover:hidden`}
          >
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
          {title ||
            'Cho thuê Tầng 1 (sảnh) của Cơ quan Thông tấn xã Việt Nam khu vực Miền Trung - Tây Nguyên'}
        </h2>
        <p className="mt-2 text-gray-600">
          <span className="text-black font-bold">{renderPrice() || '36.000.000 VNĐ'}</span>
        </p>
        {url === 'phien-dau-gia' && !isEndTimePassed(endTime, endDay) ? (
          <></>
        ) : (
          <>
            <button
              className="mt-4 w-full bg-green-900 text-white font-bold py-2 px-4 rounded group-hover:bg-sky-700"
              onClick={handleDetailClick}
            >
              VIEW
            </button>
          </>
        )}
      </div>
    </div>
  );
};

export default CardList;
