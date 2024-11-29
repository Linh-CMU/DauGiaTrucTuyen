import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';
import { useMessage } from '@contexts/MessageContext';
import { Button, TextField, IconButton, InputAdornment } from '@mui/material';
import { verifyOTP } from '../queries/AuthenAPI';
import { FormEvent, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';

const OTPPage = () => {
  const [otp, setOtp] = useState('');
  const navigate = useNavigate();
  const location = useLocation();
  const { email } = location.state || {};
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
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
      <div className="absolute w-96 h-[40%] bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-[40%] bg-white rounded-lg shadow-lg p-8 z-10">
        <h2 className="text-xl font-bold mb-6">OTP AUTHENTICATION</h2>
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
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            AUTHENTICATION
          </button>
        </form>
      </div>
    </div>
  );
};

export default OTPPage;
