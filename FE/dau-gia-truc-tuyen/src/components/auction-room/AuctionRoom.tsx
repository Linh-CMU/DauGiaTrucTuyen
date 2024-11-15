import { useState, useEffect, useMemo } from 'react';
import { AuctionDetails } from 'types';
import { Box, IconButton, TextField, Grid, Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';
import AutoGraphIcon from '@mui/icons-material/AutoGraph';
import EmojiEventsIcon from '@mui/icons-material/EmojiEvents';
import HourglassBottomIcon from '@mui/icons-material/HourglassBottom';
import { getAuctionRoomDetail, postBidMoney } from '../../queries/AuctionAPI';
import CountDownTimeForRoom from '@common/coutdown-timer/CountDownTimeForRoom';
import useTimeDifference from '@hooks/useTimeDifference';

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
  const [bidHistory, setBidHistory] = useState([]);
  const [currentPrice, setCurrentPrice] = useState(0);
  const [roomAuctionDetails, setRoomAuctionDetails] = useState<RoomAuctionDetails | null>(null);
  const [isTimeOut, setIsTimeOut] = useState(false);
  const [timeRound, setTimeRound] = useState('');
  const [bidStep, setBidStep] = useState(0);
  const id = 1;

  // const roomTime = convertToRoomTime({
  //   startDay: roomAuctionDetails?.startDay || '',
  //   startTime: roomAuctionDetails?.startTime || '',
  // });

  const roomTime = '14/11/2024 23:59';
  const { greaterTime, isIntime } = useTimeDifference(roomTime, timeRound);

  console.log(isIntime, 'isIntime');

  const stepValue: number | undefined = useMemo(() => {
    return auctionDetailInfor?.priceStep;
  }, [auctionDetailInfor]);

  useEffect(() => {
    const fetchAuctionRoomDetails = async () => {
      const response = await getAuctionRoomDetail(id); // Call API function
      if (response?.isSucceed) {
        setRoomAuctionDetails(response?.result || null); // Ensure result is an object or null
        setTimeRound(response?.result?.timeRound);
      } else {
        console.error('fetch list failed');
      }
    };
    fetchAuctionRoomDetails();
  }, [id]);

  useEffect(() => {
    const socket = new WebSocket(`ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=${id}`);

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setBidHistory(data);
      setCurrentPrice(data[0]?.Price);
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
    return new Intl.NumberFormat('vi-VN').format(int);
  };

  const calculateResult = () => {
    const result = (stepValue || 0) * inputValue; // Calculate the result
    setBidStep(result); // Store the result in state
  };

  useEffect(() => {
    calculateResult(); // Recalculate result when either stepValue or inputValue changes
  }, [inputValue, stepValue]);

  const calculateTotal = () => {
    const total = currentPrice + (stepValue || 0) * inputValue; // Calculate total price
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
                    <p>{new Intl.NumberFormat('vi-VN').format(bid.Price)} VND</p>
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
          X
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
                  onClick={incrementValue}
                >
                  <AddIcon sx={{ color: 'white', width: '12px', height: '12px' }} />
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
                  onClick={decrementValue}
                >
                  <RemoveIcon sx={{ color: 'white', width: '12px', height: '12px' }} />
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
  );
};

export default AuctionRoom;
