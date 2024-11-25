import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import { AuctionDetails } from 'types';
import { convertDate } from '@utils/helper';
import { useNavigate } from 'react-router-dom';

interface InfoRowProps {
  label: string;
  value: string;
}

interface DetailInformationProps {
  auctionDetailInfor: AuctionDetails; // Use shared Auction type
}
const InfoRow: React.FC<InfoRowProps> = ({ label, value }) => (
  <>
    <div className="flex justify-between py-1">
      <div className="font-bold">{label}</div>
      <div className="font-bold">{value}</div>
    </div>
    <div className="h-[2px] w-full bg-gray-200"></div>
  </>
);

const DetailInformation: React.FC<DetailInformationProps> = ({ auctionDetailInfor }) => {
  const navigate = useNavigate();
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

  const calculateNewTargetDate = (
    endTime: string,
    endDay: string,
    timePerLap: string
  ): Date => {
    if (!endTime || !endDay || !timePerLap) {
      console.warn("Missing required parameters for calculateNewTargetDate:", {
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
      console.error("Error in calculateNewTargetDate:", { endTime, endDay, timePerLap, error });
      return new Date(); // Trả về giá trị mặc định trong trường hợp lỗi
    }
  };
  
  // Tính toán ngày mục tiêu mới
  const targetDate = calculateNewTargetDate(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay, auctionDetailInfor?.timePerLap);
  const calculateFinalTime = (
    endTime: string,
    endDay: string
  ): Date => {
    const [endHours, endMinutes] = endTime.split(':').map(Number); // Tách giờ và phút từ endTime
  
    // Chuyển đổi định dạng dd/MM/yyyy thành yyyy-MM-dd
    const [day, month, year] = endDay.split('/');
    const isoDate = `${year}-${month}-${day}`; // Định dạng yyyy-MM-dd
  
    const endDate = new Date(isoDate); // Tạo đối tượng Date từ định dạng ISO
  
    endDate.setHours(endHours, endMinutes, 0, 0); // Gán giờ và phút từ endTime
  
    return endDate; // Trả về đối tượng Date đã được tính toán
  };
  
  const isRegistrationAllowed = (
    endTime: string,
    endDay: string
  ): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);
    console.log('finalTime', finalTime);
    return finalTime > new Date(); // Kiểm tra nếu thời gian kết thúc lớn hơn hiện tại
  };

  const isEndTimePassed = (
    endTime: string = '',
    endDay: string = ''
  ): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);
    
    return finalTime <= new Date(); // Kiểm tra nếu thời gian cuối đã qua
  };
  const auctionInfo = [
    { label: 'Giá khởi điểm', value: `${auctionDetailInfor?.startingPrice} VNĐ` },
    { label: 'Bước giá', value: `${auctionDetailInfor.priceStep} VNĐ` },
    { label: 'Tiền đặt trước', value: `${auctionDetailInfor.moneyDeposit} VNĐ` },
    {
      label: 'Thời gian đăng kí tham gia',
      value: `Từ ${auctionDetailInfor.startTime} ${auctionDetailInfor.startDay} đến ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}`,
    },
    { label: 'Thời gian bắt đầu đấu giá', value: `Từ ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay} đến ${calculateNewEndTime(auctionDetailInfor.endTime, auctionDetailInfor.timePerLap)} ${auctionDetailInfor.endDay}`, },
    { label: 'Hình thức đấu giá trực tuyến', value: 'Trả giá không xác định vòng' },
    { label: 'Phương thức trả giá', value: auctionDetailInfor.paymentMethod },
  ];
  const handleNavigateToContract = () => {
    navigate('/contract', {
      state: {
        // Contract data
        companyName: 'Tên Công ty ABC',
        companyAddress: '123 Đường ABC, Thành phố XYZ',
        taxCode: '0123456789',
        representativeName: 'Nguyễn Văn A',
        sellerName: 'Trần Văn B',
        sellerID: '123456789',
        sellerAddress: '456 Đường DEF, Thành phố XYZ',
        productName: 'Sản phẩm XYZ',
        websiteURL: 'https://example.com',
        effectiveDate: '05/11/2024',
        auctionId: auctionDetailInfor.listAuctionID,

        // Auction data
        auctionInfo: [
          { label: 'Giá khởi điểm', value: `${auctionDetailInfor?.startingPrice} VNĐ` },
          { label: 'Bước giá', value: `${auctionDetailInfor?.priceStep} VNĐ` },
          { label: 'Tiền đặt trước', value: `${auctionDetailInfor?.moneyDeposit} VNĐ` },
          {
            label: 'Thời gian đăng kí tham gia',
            value: `Từ ${auctionDetailInfor?.startTime} ${auctionDetailInfor?.startDay} đến ${auctionDetailInfor?.endTime} ${auctionDetailInfor?.endDay}`,
          },
          { label: 'Thời gian bắt đầu đấu giá', value: '09:00:00 31/10/2024' },
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
        <div className={`${
              !isEndTimePassed(auctionDetailInfor.endTime, auctionDetailInfor.endDay) ? 'bg-green-500' : targetDate > new Date() ? 'bg-orange-500'  : 'bg-yellow-500'
            } bg-opacity-90 p-2 rounded-full w-60`} >
          <CountdownTimer targetDate={targetDate} />
        </div>
      </div>

      <div className="h-[2px] w-full bg-gray-200"></div>

      <div className="bg-gray-100 w-full h-full p-8 flex flex-col gap-1">
        {auctionInfo.map((item, index) => (
          <InfoRow key={index} label={item.label} value={item.value} />
        ))}
        <div className="mt-8 ml-auto mr-auto">
          <button
            className="bg-blue-500 text-white px-2 py-1 rounded mr-2 w-56 h-10"
            onClick={handleNavigateToContract}
            disabled={!isRegistrationAllowed(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay)}
          >
            THAM GIA ĐẤU GIÁ
          </button>
        </div>
      </div>
    </div>
  );
};

export default DetailInformation;
