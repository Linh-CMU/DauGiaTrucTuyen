import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import { AuctionDetailRegister } from 'types';
import { convertDate } from '@utils/helper';
import { useNavigate } from 'react-router-dom';
import HowToRegIcon from '@mui/icons-material/HowToReg';
import { useEffect, useState } from 'react';
import AutoGraphIcon from '@mui/icons-material/AutoGraph';
import HourglassBottomIcon from '@mui/icons-material/HourglassBottom';
import { CancelModal } from '@components/modalAccept/ApproveModal';
import { deleteAuction } from '@queries/AuctionAPI';
import { useMessage } from '@contexts/MessageContext';
import ContractModal from '@components/modal-contract/ContractModal';

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
      <div>{value}</div>
    </div>
    <div className="h-[2px] w-full bg-gray-200"></div>
  </>
);

const DetailInformationUser: React.FC<DetailInformationProps> = ({ auctionDetailInfor }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [swith, setSwith] = useState(false);
  const [bidHistory, setBidHistory] = useState([]);
  const navigate = useNavigate();
  const [isApproveModalCancelOpen, setApproveModalCancelOpen] = useState(false);
  const [getId, setGetId] = useState(0);
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const [currentPrice, setCurrentPrice] = useState(0);
  const handleModalCancelClose = () => {
    setApproveModalCancelOpen(false); // Close cancel modal
  };
  const handleModalReject = async () => {
    if (getId !== 0) {
      const response = await deleteAuction(getId);
      if (response.isSucceed) {
        navigate('/listYourAuction');
        setSuccessMessage('You have successfully deleted the product.');
      }
    }
    setApproveModalCancelOpen(false);
  };
  const handleReject = (id: number) => {
    setGetId(id);
    setApproveModalCancelOpen(true); // Open cancel modal
  };
  const formatMoney = (int: number) => {
    return new Intl.NumberFormat('vi-VN').format(int ?? 0);
  };
  useEffect(() => {
    const socket = new WebSocket(
      `ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=${auctionDetailInfor.listAuctionID}`
    );

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setBidHistory(data);
      setCurrentPrice(data[0]?.Price ?? auctionDetailInfor?.startingPrice);
    };

    return () => {
      socket.close();
    };
  }, []);
  const calculateNewEndTime = (
    endTime: string | undefined,
    timePerLap: string | undefined
  ): string | null => {
    // Kiểm tra nếu endTime hoặc timePerLap không hợp lệ
    if (!endTime || !timePerLap) {
      console.error('endTime hoặc timePerLap không hợp lệ');
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
      setErrorMessage(`Error in calculateNewTargetDate: ${{ endTime, endDay, timePerLap, error }}`);
      return new Date(); // Trả về giá trị mặc định trong trường hợp lỗi
    }
  };

  // Tính toán ngày mục tiêu mới
  const targetDate = calculateNewTargetDate(
    auctionDetailInfor?.endTime,
    auctionDetailInfor?.endDay,
    auctionDetailInfor?.timePerLap
  );
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
      value: newEndTime
        ? `${auctionDetailInfor.startTime} ${auctionDetailInfor.startDay}`
        : 'Chưa được duyệt',
    },
    {
      label: '',
      value: newEndTime
        ? `${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}`
        : 'Chưa được duyệt',
    },
    {
      label: 'Thời gian bắt đầu đấu giá',
      value: newEndTime
        ? `${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}`
        : 'Chưa được duyệt',
    },
    {
      label: '',
      value: newEndTime ? `${newEndTime} ${auctionDetailInfor.endDay}` : 'Chưa được duyệt',
    },
    { label: 'Trạng thái sản phẩm', value: auctionDetailInfor.statusAuction || 'Không xác định' },
  ];
  const calculateFinalTime = (endTime: string, endDay: string): Date => {
    // Xử lý `endTime` nếu null hoặc không hợp lệ
    if (!endTime || !endTime.match(/^\d{2}:\d{2}$/)) {
      endTime = '00:00'; // Giá trị mặc định
    }

    const [endHours, endMinutes] = endTime.split(':').map(Number); // Tách giờ và phút từ endTime

    // Xử lý `endDay` nếu null hoặc không hợp lệ
    if (!endDay || !endDay.match(/^\d{2}\/\d{2}\/\d{4}$/)) {
      endDay = '01/01/0001'; // Giá trị mặc định
    }

    // Chuyển đổi định dạng dd/MM/yyyy thành yyyy-MM-dd nếu hợp lệ
    const [day, month, year] = endDay.split('/');
    const isoDate = `${year}-${month}-${day}`; // Định dạng yyyy-MM-dd

    let endDate = new Date(isoDate); // Tạo đối tượng Date từ định dạng ISO

    // Kiểm tra `endDate` có hợp lệ không (Invalid Date)
    if (isNaN(endDate.getTime())) {
      endDate = new Date(0); // Giá trị mặc định nếu không hợp lệ
    }

    // Gán giờ và phút từ `endTime`
    endDate.setHours(endHours, endMinutes, 0, 0);

    return endDate; // Trả về đối tượng Date đã được tính toán
  };

  const isEndTimePassed = (endTime: string = '', endDay: string = ''): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);

    return finalTime <= new Date(); // Kiểm tra nếu thời gian cuối đã qua
  };
  const handleNavigateToContract = () => {
    navigate('/edit-auction', { state: { id: auctionDetailInfor.listAuctionID } });
  };

  return (
    <div className="container flex flex-col gap-2 h-full">
      <div className="flex gap-1">
        <div className="font-bold line-clamp-2">{auctionDetailInfor?.nameAuction}</div>
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
                          index === 0 ? 'text-green-600' : 'text-red-600'
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
                <div className="flex justify-between w-full">
                  <p>Giá hiện tại</p>
                  <span>{formatMoney(currentPrice)} VNĐ</span>
                </div>
                <div className="h-[2px] w-full bg-gray-200"></div>
                <div className="mt-2">
                  <button className="bg-green-600" onClick={() => setSwith(false)}>
                    Back
                  </button>
                </div>
              </div>
            </div>
          </>
        ) : (
          <>
            {auctionInfo.map((item, index) => (
              <InfoRow key={index} label={item.label} value={item.value} />
            ))}
            <div className="mt-14 ml-auto mr-auto">
              <button
                className="bg-amber-500 text-white px-2 py-1 rounded mr-2 h-10"
                onClick={() => setIsModalOpen(true)}
              >
                View contract
              </button>
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
                    onClick={() => handleReject(auctionDetailInfor.listAuctionID)}
                  >
                    Delete
                  </button>
                </>
              ) : (
                <>
                  {isEndTimePassed(auctionDetailInfor.endTime, auctionDetailInfor.endDay) ? (
                    <>
                      <button
                        className="bg-green-400 text-white px-2 py-1 rounded mr-2 h-10"
                        onClick={() => setSwith(true)}
                      >
                        Join room
                      </button>
                    </>
                  ) : (
                    <></>
                  )}
                </>
              )}
            </div>
          </>
        )}
      </div>
      <CancelModal
        open={isApproveModalCancelOpen}
        onClose={handleModalCancelClose}
        onConfirm={handleModalReject}
      />
      <ContractModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        detailAuction={auctionDetailInfor}
      />
    </div>
  );
};

export default DetailInformationUser;
