import { useState, useEffect, useMemo } from 'react';
import { AuctionDetails } from 'types';
import { Box, IconButton, TextField, Grid, Button, Typography, Modal } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import AutoGraphIcon from '@mui/icons-material/AutoGraph';
import EmojiEventsIcon from '@mui/icons-material/EmojiEvents';
import HourglassBottomIcon from '@mui/icons-material/HourglassBottom';
import { getAuctionRoomDetail, postBidMoney } from '../../queries/AuctionAPI';
import CountDownTimeForRoom from '@common/coutdown-timer/CountDownTimeForRoom';
import useTimeDifference from '@hooks/useTimeDifference';
import { useParams } from 'react-router-dom';

const modalStyle = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  boxShadow: 24,
  display: 'flex',
  flexDirection: 'column',
  alignItems: 'center',
  gap: '8px',
  padding: '16px',
  minWidth: '600px',
  borderRadius: '8px',
  outline: 'none',
  border: 'none',
};

interface AuctionRoomProps {
  auctionDetailInfor: AuctionDetails | null;
}
interface BidMoneyParams {
  auctionId: string | undefined;
  price: number;
}

interface RoomAuctionDetails {
  startDay: string;
  startTime: string;
  endDay: string;
  endTime: string;
  // Add other properties as needed
}

const convertToRoomTime = ({
  startDay,
  startTime,
}: {
  startDay: string;
  startTime: string;
}): string => {
  return `${startDay} ${startTime}`;
};

const AuctionRoom: React.FC<AuctionRoomProps> = ({ auctionDetailInfor }) => {
  const [inputValue, setInputValue] = useState(1); // Start with a numeric value
 const [bidHistory, setBidHistory] = useState<{ Price: number }[]>([]);
  const [currentPrice, setCurrentPrice] = useState(0);
  const [roomAuctionDetails, setRoomAuctionDetails] = useState<RoomAuctionDetails | null>(null);
  const [isTimeOut, setIsTimeOut] = useState(false);
  const [close, setClose] = useState(true);
  const [timeRound, setTimeRound] = useState('');
  const [bidStep, setBidStep] = useState(0);
  const { id } = useParams<{ id: string }>();

  // const roomTime = convertToRoomTime({
  //   startDay: roomAuctionDetails?.startDay || '',
  //   startTime: roomAuctionDetails?.startTime || '',
  // });

  const handleGoHome = () => {
    setClose(false);
  };

  const calculateFinalTime = (
    endTime: string,
    endDay: string
  ): Date => {
    const [endHours, endMinutes] = endTime.split(':').map(Number); // Tách giờ và phút từ endTime

    const endDate = new Date(endDay); // Tạo đối tượng Date từ endDay
    endDate.setHours(endHours, endMinutes, 0, 0); // Gán giờ và phút từ endTime

    return endDate; // Trả về đối tượng Date đã được tính toán
  };

  const isRegistrationAllowed = (endTime: string, endDay: string): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);
    return finalTime > new Date(); // Kiểm tra nếu thời gian kết thúc lớn hơn hiện tại
  };

  useEffect(() => {
    const fetchAuctionRoomDetails = async () => {
      const response = await getAuctionRoomDetail(Number(id)); // Call API function
      if (response?.isSucceed) {
        setRoomAuctionDetails(response?.result || null); // Ensure result is an object or null
        setTimeRound(response?.result?.timeRound);
      } else {
        console.error('fetch list failed');
      }
    };
    fetchAuctionRoomDetails();
  }, [id]);
  const calculateFinalTimes = (endTime: string, endDay: string, timePerLap: string): string => {
    if (!endTime || !endDay || !timePerLap) {
      // Nếu một trong các tham số không hợp lệ, trả về chuỗi rỗng
      return '';
    }

    // Tách giờ và phút từ endTime
    const [endHours, endMinutes] = endTime.split(':').map(Number);

    // Tách giờ và phút từ timePerLap
    const [lapHours, lapMinutes] = timePerLap.split(':').map(Number);

    // Tạo đối tượng Date từ endDay (giả sử endDay ở định dạng "dd/MM/yyyy")
    const [day, month, year] = endDay.split('/').map(Number);
    const endDate = new Date(year, month - 1, day, endHours, endMinutes);

    // Cộng thêm giờ và phút từ timePerLap
    endDate.setHours(endDate.getHours() + lapHours);
    endDate.setMinutes(endDate.getMinutes() + lapMinutes);

    // Định dạng ngày và giờ theo kiểu dd/MM/yyyy hh:mm
    const formattedDate = `${endDate.getDate().toString().padStart(2, '0')}/${(endDate.getMonth() + 1).toString().padStart(2, '0')}/${endDate.getFullYear()} ${endDate.getHours().toString().padStart(2, '0')}:${endDate.getMinutes().toString().padStart(2, '0')}`;

    return formattedDate;
  };

  const roomTime = calculateFinalTimes(
    roomAuctionDetails?.endTime ?? '', // endTime
    roomAuctionDetails?.endDay ?? '', // endDay
    timeRound ?? '' // timePerLap
  ).toString();
  const { greaterTime, isIntime } = useTimeDifference(roomTime, timeRound);

  const stepValue: number | undefined = useMemo(() => {
    return auctionDetailInfor?.priceStep;
  }, [auctionDetailInfor]);

  useEffect(() => {
    const socket = new WebSocket(`ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=${id}`);

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setBidHistory(data);
      setCurrentPrice(data[0]?.Price ?? auctionDetailInfor?.startingPrice);
    };

    return () => {
      socket.close();
    };
  }, [id]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    if (/^[0-9]*$/.test(value)) {
      // Accept only numeric input
      setInputValue(value === '' ? 0 : Number(value)); // Set input value or default to 0
    }
  };

  const incrementValue = () => {
    setInputValue((prev) => prev + 1);
  };

  const decrementValue = () => {
    setInputValue((prev) => (prev > 1 ? prev - 1 : 1)); // Prevent going below 1
  };

  const formatMoney = (int: number) => {
    return new Intl.NumberFormat('vi-VN').format(int ?? 0);
  };

  const calculateResult = () => {
    const result = (stepValue || 0) * inputValue; // Calculate the result
    setBidStep(result); // Store the result in state
  };

  useEffect(() => {
    calculateResult(); // Recalculate result when either stepValue or inputValue changes
  }, [inputValue, stepValue]);

  const calculateTotal = () => {
    const total = (currentPrice ?? 0) + (stepValue || 0) * inputValue; // Calculate total price
    return new Intl.NumberFormat('vi-VN').format(total); // Format total to Vietnamese style
  };

  const handleBidButton = async (params: BidMoneyParams) => {
    try {
      // Ensure auctionId and price are valid before making the call
      if (!params.auctionId || params.price <= 0) {
        console.error('Auction ID and price must be valid.');
        return;
      }

      // Proceed with the valid number ID
      const response = await postBidMoney(params.auctionId, params.price); // Ensure sending it as string if needed
      console.log('Bid placed successfully:', response);
    } catch (error) {
      console.error('Failed to place bid:', error);
    }
  };

  return (
    <>
      <div className="container flex flex-col gap-2 h-full">
        <div>
          <div className="mb-5 flex gap-3 size-5 w-full font-bold">
            <AutoGraphIcon />
            Diễn biến cuộc đấu giá
          </div>
          <div className="flex flex-col border border-gray-100 p-4 rounded-lg max-h-80 overflow-y-scroll bg-lightGray">
            <div className="flex flex-col gap-2">
              {bidHistory.map((bid: any, index) => (
                <>
                  <div
                    className={`flex justify-between w-full items-center mb-2 mt-2 ${
                      index === 0 ? 'text-green-600' : 'text-red-600'
                    }`}
                  >
                    <div>
                      <p>{new Intl.NumberFormat('vi-VN').format(bid.Price)} VNĐ</p>
                      <span>{bid.DateAndTime}</span>
                    </div>
                    <div>{index === 0 ? <EmojiEventsIcon /> : <HourglassBottomIcon />}</div>
                  </div>
                  <div className="h-[2px] w-full bg-gray-200"></div>
                </>
              ))}
            </div>
          </div>
        </div>
        <div className="flex flex-col border border-gray-100 p-4 rounded-lg bg-lightGray">
          <div className="flex justify-between w-full">
            <p>Giá hiện tại</p>
            <span>{formatMoney(currentPrice)} VNĐ</span>
          </div>
          <div className="h-[2px] w-full bg-gray-200"></div>

          <div className="flex mt-6 items-center justify-between">
            <TextField
              required
              id="outlined-required"
              label="Bước giá"
              defaultValue={stepValue}
              size="small"
              InputProps={{
                readOnly: true, // Set to readOnly if you don't want this input to be editable
              }}
            />
            *
            <div className="flex items-center border border-gray-300 p-1 rounded-lg">
              <Grid item>
                <Box className="flex items-center p-1">
                  <IconButton
                    size="small"
                    sx={{
                      borderRadius: '50%',
                      backgroundColor: 'primary.main',
                      '&:hover': {
                        backgroundColor: 'primary.dark',
                      },
                    }}
                    onClick={decrementValue}
                  >
                    <RemoveIcon sx={{ color: 'white', width: '12px', height: '12px' }} />
                  </IconButton>
                  <TextField
                    value={inputValue}
                    onChange={handleInputChange}
                    placeholder="1 2 3 4 5"
                    variant="outlined"
                    size="small"
                    sx={{
                      mx: 1,
                      width: '50px',
                      '& .MuiInputBase-root': {
                        height: '25px',
                      },
                      '& input': {
                        marginLeft: 'auto',
                        marginRight: 'auto',
                        height: '25px',
                        fontSize: '12px',
                      },
                    }}
                  />
                  <IconButton
                    size="small"
                    sx={{
                      borderRadius: '50%',
                      backgroundColor: 'primary.main',
                      '&:hover': {
                        backgroundColor: 'primary.dark',
                      },
                    }}
                    onClick={incrementValue}
                  >
                    <AddIcon sx={{ color: 'white', width: '12px', height: '12px' }} />
                  </IconButton>
                </Box>
              </Grid>
            </div>
            <div>= {new Intl.NumberFormat('vi-VN').format(bidStep)}</div>
          </div>

          <div className="grid grid-cols-[2fr_3fr] items-center gap-x-6 mt-4">
            <CountDownTimeForRoom timeRound={greaterTime || ''} setIsTimeOut={setIsTimeOut} />
            <Button
              className="w-auto p-2 flex gap-4 h-14"
              variant="contained"
              color="primary"
              disabled={isTimeOut || !isIntime}
              onClick={() =>
                handleBidButton({
                  auctionId: auctionDetailInfor?.listAuctionID.toString(),
                  price: bidStep,
                })
              } // Pass an object as an argument
            >
              Giá phải trả:
              <span className="font-bold text-xl"> {calculateTotal()} VNĐ</span>
            </Button>
          </div>
        </div>
      </div>
      <Modal
        open={isTimeOut}
        onClose={() => setIsTimeOut(!isTimeOut)}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={modalStyle}>
          <Typography id="modal-modal-title" variant="h6" component="h2" className="text-center">
            Phiên đấu giá kết thúc!
          </Typography>

          {isTimeOut && bidHistory[0]?.Price === currentPrice ? (
            <>
              <Typography
                id="modal-modal-title"
                variant="h6"
                component="h1"
                className="text-center text-red-600"
              >
                Chúc mừng bạn đã thắng cuộc
              </Typography>
              <img src="/winner.png" alt="Winner" className="h-40 w-80 mx-auto" />
              <Typography id="modal-modal-description" sx={{ mt: 2 }}>
                Mức giá cuối cùng:{' '}
                <span className="font-bold">
                  {formatMoney((bidHistory[0] as { Price: number })?.Price || 0)} VNĐ
                </span>
              </Typography>
              <Typography style={{ fontSize: '14px' }} className="text-center font-thin italic">
                Vui lòng kiểm tra email của bạn để hoàn thành thủ tục đấu giá sau khi chiến thắng.
              </Typography>
            </>
          ) : (
            <>
              <Typography
                id="modal-modal-title"
                variant="h6"
                component="h1"
                className="text-center text-red-600"
              >
                Bạn không thắng phiên đấu giá này
              </Typography>
              <img src="/lose.png" alt="Loser" className="h-40 w-80 mx-auto" />
               <Typography id="modal-modal-description" sx={{ mt: 2 }}>
                Mức giá cuối cùng:{' '}
                <span className="font-bold">
                  {formatMoney((bidHistory[0] as { Price: number })?.Price || 0)} VNĐ
                </span>
              </Typography>
              <Typography id="modal-modal-description" sx={{ mt: 2 }}>
                Hãy thử lại lần sau!
              </Typography>
            </>
          )}
        </Box>
      </Modal>
    </>
  );
};

export default AuctionRoom;
