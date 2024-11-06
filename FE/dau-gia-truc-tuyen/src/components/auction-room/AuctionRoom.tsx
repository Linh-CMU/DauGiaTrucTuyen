import { useState } from 'react';
import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import { convertDate } from '@utils/helper';
import { AuctionDetails } from 'types';
import { Box, IconButton, TextField, Grid, Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import RemoveIcon from '@mui/icons-material/Remove';

interface AuctionRoomProps {
  auctionDetailInfor: AuctionDetails | null; // Use shared Auction type
}

const AuctionRoom: React.FC<AuctionRoomProps> = ({ auctionDetailInfor }) => {
  const [inputValue, setInputValue] = useState(1); // Start with a numeric value
  const stepValue = 5000000;
  const currentPrice = 14000000;

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value;
    if (/^[0-9]*$/.test(value)) { // Accept only numeric input
      setInputValue(value === '' ? 0 : Number(value)); // Set input value or default to 0
    }
  };

  const incrementValue = () => {
    setInputValue((prev) => prev + 1);
  };

 const decrementValue = () => {
  setInputValue((prev) => (prev > 1 ? prev - 1 : 1)); // Prevent going below 1
};

 const calculateResult = () => {
    const result = stepValue * inputValue; // Calculate the result
    return new Intl.NumberFormat('vi-VN').format(result); // Format result to Vietnamese style
  };

  const calculateTotal = () => {
    const total = currentPrice + stepValue * inputValue; // Calculate total price
    return new Intl.NumberFormat('vi-VN').format(total); // Format total to Vietnamese style
  };

  return (
    <div className="container flex flex-col gap-2 h-full">
      <div>
        <div>Diễn biến cuộc đấu giá</div>
        <div className="flex flex-col border border-gray-100 p-4 rounded-lg">
          <div className="flex justify-between w-full items-center mb-2 mt-2 ">
            <div>
              <p>14.000.000</p>
              <span>15/09/2023 12:46:18</span>
            </div>
            <div>ID user</div>
          </div>
          <div className="h-[2px] w-full bg-gray-200"></div>
          <div className="flex justify-between w-full items-center mb-2 mt-2 ">
            <div>
              <p>14.000.000</p>
              <span>15/09/2023 12:46:18</span>
            </div>
            <div>ID user</div>
          </div>
        </div>
      </div>
      <div className="flex flex-col border border-gray-100 p-4 rounded-lg">
        <div className="flex justify-between w-full">
          <p>Giá hiện tại</p>
          <span>14.000.000</span>
        </div>
        <div className="h-[2px] w-full bg-gray-200"></div>

        <div className="flex mt-6">
          <TextField
            required
            id="outlined-required"
            label="Bước giá"
            defaultValue="5000000"
            size="small"
           
            InputProps={{
              readOnly: true, // Set to readOnly if you don't want this input to be editable
            }}
          />
          X
          <div className="flex items-center border border-gray-100 p-1 rounded-lg">
            <Grid item>
              <Box className="flex items-center">
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
                  <AddIcon sx={{ color: 'white' }} />
                </IconButton>
                <TextField
                  value={inputValue}
                  onChange={handleInputChange}
                  placeholder="1 2 3 4 5"
                  variant="outlined"
                  size="small"
                  sx={{
                    mx: 1,
                    width: '80px',
                    '& .MuiInputBase-root': {
                      height: '15px',
                    },
                    '& input': {
                      padding: '0px',
                      height: '15px',
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
                  <RemoveIcon sx={{ color: 'white' }} />
                </IconButton>
              </Box>
            </Grid>
          </div>
          <div>
            = {calculateResult()} {/* Display the calculated result */}
          </div>
        </div>

         <Box mt={2}>
          <Button
            variant="contained"
            color="primary"
            onClick={() => alert(`Giá phải trả: ${calculateResult()}`)} // Display the total value
          >
            Giá phải trả: {calculateTotal()}
          </Button>
        </Box>
      </div>
    </div>
  );
};

export default AuctionRoom;
