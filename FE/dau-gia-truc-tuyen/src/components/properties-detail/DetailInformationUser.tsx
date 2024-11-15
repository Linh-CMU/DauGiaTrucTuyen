import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import { AuctionDetailRegister } from 'types';
import { convertDate } from '@utils/helper';
import { useNavigate } from 'react-router-dom';
import HowToRegIcon from '@mui/icons-material/HowToReg';
import { useEffect, useState } from 'react';
import AutoGraphIcon from '@mui/icons-material/AutoGraph';
import HourglassBottomIcon from '@mui/icons-material/HourglassBottom';

interface InfoRowProps {
  label: string;
  value: string;
}

interface DetailInformationProps {
  auctionDetailInfor: AuctionDetailRegister; // Use shared Auction type
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

const DetailInformationUser: React.FC<DetailInformationProps> = ({ auctionDetailInfor }) => {
  const [swith, setSwith] = useState(false);
  const [bidHistory, setBidHistory] = useState([]);
  const targetDate = convertDate(auctionDetailInfor?.endTime, auctionDetailInfor?.endDay);
  const navigate = useNavigate();
  useEffect(() => {
    const socket = new WebSocket(`ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=${1}`);

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setBidHistory(data);
    };

    return () => {
      socket.close();
    };
  }, []);
  const calculateNewEndTime = (endTime: string | undefined, timePerLap: string | undefined): string | null => {
    // Kiểm tra nếu endTime hoặc timePerLap không hợp lệ
    if (!endTime || !timePerLap) {
      console.error("endTime hoặc timePerLap không hợp lệ");
      return null; // Hoặc giá trị mặc định phù hợp
    }
  
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
  const newEndTime = calculateNewEndTime(auctionDetailInfor.endTime, auctionDetailInfor.timePerLap);
  const auctionInfo = [
    {
      label: 'Giá khởi điểm',
      value: `${
        auctionDetailInfor?.startingPrice
          .toLocaleString('vi-VN', {
            style: 'currency',
            currency: 'VND',
          })
          .replace('₫', '') || 0
      } VND`,
    },
    {
      label: 'Bước giá',
      value: `${
        auctionDetailInfor?.stepPrice
          ? auctionDetailInfor.stepPrice
              .toLocaleString('vi-VN', {
                style: 'currency',
                currency: 'VND',
              })
              .replace('₫', '')
          : '0'
      } VNĐ`,
    },
    {
      label: 'Tiền đặt trước',
      value: `${
        auctionDetailInfor?.priceDeposit
          .toLocaleString('vi-VN', {
            style: 'currency',
            currency: 'VND',
          })
          .replace('₫', '') || 0
      } VNĐ`,
    },
    {
      label: 'Thời gian đăng kí tham gia',
      value: newEndTime ? `Từ ${auctionDetailInfor.startTime} ${auctionDetailInfor.startDay} đến ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}` : 'Chưa được duyệt',
    },
    { label: 'Thời gian bắt đầu đấu giá', value: newEndTime 
      ? `Từ ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay} đến ${newEndTime} ${auctionDetailInfor.endDay}`
      : 'Chưa được duyệt' },
    { label: 'Trạng thái sản phẩm', value: auctionDetailInfor.statusAuction || 'Không xác định' },
  ];

  const handleNavigateToContract = () => {
    navigate('/contract');
  };

  return (
    <div className="container flex flex-col gap-2 h-full">
      <div className="flex gap-1">
        <div className="font-bold line-clamp-2">{auctionDetailInfor?.nameAuction}</div>
        <CountdownTimer targetDate={targetDate} />
      </div>

      <div className="h-[2px] w-full bg-gray-200"></div>

      <div className="bg-gray-100 w-full h-full p-8 flex flex-col gap-1">
        <div className="ml-auto flex">
          <div className="mt-1 mr-1 font-bold">{auctionDetailInfor.countBidder}</div>
          <div>
            <HowToRegIcon />
          </div>
        </div>
        {swith ? (
          <>
            <div>
              <div className="mb-5 flex gap-3 size-5 w-full font-bold">
                <AutoGraphIcon />
                Diễn biến cuộc đấu giá
              </div>
              <div className="flex flex-col border border-gray-100 p-4 rounded-lg max-h-80 overflow-y-scroll bg-lightGray">
                <div className="flex flex-col gap-2">
                  {bidHistory?.map((bid: any, index: any) => (
                    <>
                      <div
                        className={`flex justify-between w-full items-center mb-2 mt-2 ${
                          index % 2 === 0 ? 'text-green-600' : 'text-blue-600'
                        }`}
                      >
                        <div>
                          <p>{new Intl.NumberFormat('vi-VN').format(bid.Price)} VND</p>
                          <span>{bid.DateAndTime}</span>
                        </div>
                        <div>
                          <HourglassBottomIcon />
                        </div>
                      </div>
                      <div className="h-[2px] w-full bg-gray-200"></div>
                    </>
                  ))}
                </div>
              </div>
              <div className="mt-8 ml-auto mr-auto">
                <button className="bg-green-600" onClick={() => setSwith(false)}>
                  Back
                </button>
              </div>
            </div>
          </>
        ) : (
          <>
            {auctionInfo.map((item, index) => (
              <InfoRow key={index} label={item.label} value={item.value} />
            ))}
            <div className="mt-14 ml-auto mr-auto">
              {auctionDetailInfor.statusAuction === 'Not approved yet' ? (
                <>
                  <button
                    className="bg-blue-500 text-white px-2 py-1 rounded mr-2 h-10"
                    onClick={handleNavigateToContract}
                  >
                    Update
                  </button>
                  <button
                    className="bg-red-500 text-white px-2 py-1 rounded mr-2 h-10"
                    onClick={handleNavigateToContract}
                  >
                    Delete
                  </button>
                </>
              ) : (
                <>
                  <button
                    className="bg-green-400 text-white px-2 py-1 rounded mr-2 h-10"
                    onClick={() => setSwith(true)}
                  >
                    Join room
                  </button>
                </>
              )}
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default DetailInformationUser;
