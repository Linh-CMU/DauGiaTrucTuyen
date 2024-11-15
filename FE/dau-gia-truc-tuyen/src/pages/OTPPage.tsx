import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';
import { useMessage } from '@contexts/MessageContext';
import { Button, TextField, IconButton, InputAdornment } from '@mui/material';
import { verifyOTP } from '@queries/AuthenAPI';
import { FormEvent, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const OTPPage = () => {
  const [otp, setOtp] = useState('');
  const navigate = useNavigate();
  const location = useLocation();
  const { 
    email
  } = location.state || {};
  const { setIsLoading } = useLoading();
    const { setSuccessMessage, setErrorMessage } = useMessage();

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);

    const success = await verifyOTP({ email, otp });

    setIsLoading(false);
    if (success) {
      navigate('/login');
      setSuccessMessage('Veryfy successful!');
      // Optionally, reset the form
      setOtp('');
    } else {
      setErrorMessage('Veryfy failed!');
    }
  };

  return (
    <div className="flex items-center justify-center bg-gray-100 min-h-screen">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">XÁC THỰC OTP</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Email"
              value={email}
              required
              sx={{ marginBottom: '1rem' }}
              InputProps={{
                readOnly: true, // Makes the input field read-only
              }}
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type={'text'}
              label="OTP"
              value={otp}
              onChange={(e) => setOtp(e.target.value)}
              required
              sx={{ marginBottom: '1rem' }}
              InputProps={{
                endAdornment: <InputAdornment position="end"></InputAdornment>,
              }}
            />
          </div>
          <Button
            fullWidth
            variant="contained"
            type="submit"
            sx={{
              backgroundColor: '#6200ea',
              '&:hover': {
                backgroundColor: '#3700b3',
              },
            }}
          >
            Xác thực
          </Button>
        </form>
      </div>
    </div>
  );
};

export default OTPPage;
