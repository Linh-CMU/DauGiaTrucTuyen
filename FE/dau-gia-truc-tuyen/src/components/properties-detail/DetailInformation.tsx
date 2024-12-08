import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import { AuctionDetails } from 'types';
import { convertDate } from '@utils/helper';
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';

interface InfoRowProps {
  label: string;
  value: string;
}

interface DetailInformationProps {
  setSwitchdetail: (value: boolean) => void;
  auctionDetailInfor: AuctionDetails; // Use shared Auction type
}
const InfoRow: React.FC<InfoRowProps> = ({ label, value }) => (
  <>
    <div className="flex justify-between py-1">
      <div className="font-bold">{label}</div>
      <div>{value}</div>
    </div>
    <div className="h-[2px] w-full bg-gray-200"></div>
  </>
);

const DetailInformation: React.FC<DetailInformationProps> = ({
  auctionDetailInfor,
  setSwitchdetail,
}) => {
  const navigate = useNavigate();
  const currentPath = window?.location?.pathname;
  const calculateNewEndTime = (endTime: string, timePerLap: string): string => {
    // Tách giờ và phút từ endTime
    const [endHours, endMinutes] = endTime.split(':').map(Number);
    // Tách giờ và phút từ timePerLap
    const [lapHours, lapMinutes] = timePerLap.split(':').map(Number);
    // Tạo đối tượng Date với giờ và phút từ endTime
    const endDate = new Date();
    endDate.setHours(endHours);
    endDate.setMinutes(endMinutes);

    // Cộng thêm giờ và phút từ timePerLap
    endDate.setHours(endDate.getHours() + lapHours);
    endDate.setMinutes(endDate.getMinutes() + lapMinutes);

    // Lấy giờ và phút sau khi cộng thêm
    const newHours = endDate.getHours().toString().padStart(2, '0');
    const newMinutes = endDate.getMinutes().toString().padStart(2, '0');

    // Trả về chuỗi thời gian mới
    return `${newHours}:${newMinutes}`;
  };

  const calculateNewTargetDate = (endTime: string, endDay: string, timePerLap: string): Date => {
    if (!endTime || !endDay || !timePerLap) {
      console.warn('Missing required parameters for calculateNewTargetDate:', {
        endTime,
        endDay,
        timePerLap,
      });
      return new Date(); // Trả về thời gian hiện tại nếu thiếu tham số
    }

    try {
      // Sử dụng convertDate để tạo đối tượng ngày ban đầu
      const endDate = convertDate(endTime, endDay);
      const now = new Date();

      // Tách giờ và phút từ TimePerLap
      const [hours, minutes] = timePerLap.split(':').map(Number);

      if (endDate < now) {
        // Nếu endDate đã qua, cộng thêm giờ và phút từ TimePerLap
        endDate.setHours(endDate.getHours() + hours);
        endDate.setMinutes(endDate.getMinutes() + minutes);
      }

      return endDate; // Trả về đối tượng Date
    } catch (error) {
      console.error('Error in calculateNewTargetDate:', { endTime, endDay, timePerLap, error });
      return new Date(); // Trả về giá trị mặc định trong trường hợp lỗi
    }
  };

  // Tính toán ngày mục tiêu mới
  const targetDate = calculateNewTargetDate(
    auctionDetailInfor?.endTime,
    auctionDetailInfor?.endDay,
    auctionDetailInfor?.timePerLap
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

  const isRegistrationAllowed = (endTime: string, endDay: string): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);
    console.log('finalTime', finalTime);
    return finalTime > new Date(); // Kiểm tra nếu thời gian kết thúc lớn hơn hiện tại
  };

  const isEndTimePassed = (endTime: string = '', endDay: string = ''): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);

    return finalTime <= new Date(); // Kiểm tra nếu thời gian cuối đã qua
  };
  const auctionInfo = [
    {
      label: 'Starting price',
      value: `${auctionDetailInfor?.startingPrice
        .toLocaleString('vi-VN', {
          style: 'currency',
          currency: 'VND',
        })
        .replace('₫', '')} VNĐ`,
    },
    {
      label: 'Price Step',
      value: `${auctionDetailInfor.priceStep
        .toLocaleString('vi-VN', {
          style: 'currency',
          currency: 'VND',
        })
        .replace('₫', '')} VNĐ`,
    },
    {
      label: 'Deposit',
      value: `${auctionDetailInfor.moneyDeposit
        .toLocaleString('vi-VN', {
          style: 'currency',
          currency: 'VND',
        })
        .replace('₫', '')} VNĐ`,
    },
    {
      label: 'Registration time',
      value: `${auctionDetailInfor.startTime} ${auctionDetailInfor.startDay}`,
    },
    {
      label: '',
      value: `${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}`,
    },
    {
      label: 'Auction start time',
      value: `${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}`,
    },
    {
      label: '',
      value: `${calculateNewEndTime(auctionDetailInfor.endTime, auctionDetailInfor.timePerLap)} ${auctionDetailInfor.endDay}`,
    },
    { label: 'Online auction format', value: 'Undefined bidding round' },
    { label: 'Payment method price', value: auctionDetailInfor.paymentMethod },
  ];
  const [check, setCheck] = useState(true);
  const handleNavigateToContract = () => {
    navigate('/contract', {
      state: {
        // Contract data
        companyName: 'Tên Công ty ABC',
        companyAddress: '123 Đường ABC, Thành phố XYZ',
        taxCode: '0123456789',
        representativeName: 'Nguyễn Văn A',
        owner: auctionDetailInfor?.owner,
        productName: auctionDetailInfor?.nameAuction,
        websiteURL: 'https://example.com',
        effectiveDate: '05/11/2024',
        auctionId: auctionDetailInfor.listAuctionID,
        deposit: auctionDetailInfor.moneyDeposit,
        check: check,
        // Auction data
        auctionInfo: [
          {
            label: 'Giá khởi điểm',
            value: `${auctionDetailInfor?.startingPrice
              .toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND',
              })
              .replace('₫', '')}
        VNĐ`,
          },
          {
            label: 'Bước giá',
            value: `${auctionDetailInfor?.priceStep
              .toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND',
              })
              .replace('₫', '')}
        VNĐ`,
          },
          {
            label: 'Tiền đặt trước',
            value: `10% dựa theo giá khởi điểm + 1% phí tham gia theo giá khởi điểm `,
          },
          {
            label: 'Thời gian đăng kí tham gia',
            value: `Từ ${auctionDetailInfor?.startTime} ${auctionDetailInfor?.startDay} đến ${auctionDetailInfor?.endTime} ${auctionDetailInfor?.endDay}`,
          },
          {
            label: 'Thời gian đăng ký tham gia đấu giá',
            value: `${auctionDetailInfor.createDate ? auctionDetailInfor.createDate : new Date().toLocaleDateString('en-GB')}`, // Format as dd/mm/yyyy
          },
          {
            label: 'Thời gian bắt đầu đấu giá',
            value: `${calculateNewEndTime(auctionDetailInfor.endTime, auctionDetailInfor.timePerLap)} ${auctionDetailInfor.endDay}`,
          },
          { label: 'Hình thức đấu giá trực tuyến', value: 'Trả giá không xác định vòng' },
          { label: 'Phương thức trả giá', value: auctionDetailInfor?.paymentMethod },
        ],
      },
    });
  };
  return (
    <div className="container flex flex-col gap-2 h-full">
      <div className="flex gap-1">
        <div className="font-bold line-clamp-2">{auctionDetailInfor?.description}</div>
        <div
          className={`${
            !isEndTimePassed(auctionDetailInfor.endTime, auctionDetailInfor.endDay)
              ? 'bg-green-500'
              : targetDate > new Date()
                ? 'bg-orange-500'
                : 'bg-yellow-500'
          } bg-opacity-90 p-2 rounded-full w-60`}
        >
          <CountdownTimer targetDate={targetDate} />
        </div>
      </div>

      <div className="h-[2px] w-full bg-gray-200"></div>

      <div className="bg-gray-100 w-full h-full p-8 flex flex-col gap-1">
        {auctionInfo.map((item, index) => (
          <InfoRow key={index} label={item.label} value={item.value} />
        ))}
        <div className="mt-8 ml-auto mr-auto">
          {currentPath.includes('phien-dau-gia') ? (
            <>
              {isEndTimePassed(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay) ? (
                <>
                  <button className="bg-amber-500 text-white px-2 py-1 rounded mr-2 h-10"
                  onClick={() => {
                    handleNavigateToContract();
                    setCheck(false);
                  }}>
                    View contract
                  </button>
                  <button
                    className="bg-blue-500 text-white px-2 py-1 rounded mr-2 w-56 h-10"
                    onClick={() => setSwitchdetail(true)}
                  >
                    JOIN THE AUCTION
                  </button>
                </>
              ) : (
                <></>
              )}
            </>
          ) : (
            <>
              <button
                className={`${!isRegistrationAllowed(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay) ? `bg-red-500` : `bg-blue-500`} text-white px-2 py-1 rounded mr-2 w-56 h-10`}
                onClick={handleNavigateToContract}
                disabled={
                  !isRegistrationAllowed(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay)
                }
              >
                REGISTER AUCTION
              </button>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default DetailInformation;
