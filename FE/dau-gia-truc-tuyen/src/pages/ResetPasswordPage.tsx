import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { resetPass } from '../queries/AuthenAPI';
import { FormEvent, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useMessage } from '@contexts/MessageContext';

const ResetPasswordPage = () => {
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const { token } = useParams(); 
  const { gmail } = useParams(); 
  const navigate = useNavigate();
  const {setSuccessMessage, setErrorMessage} = useMessage();
  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    
    if (password !== confirmPassword) {
      setErrorMessage('Passwords do not match');
      return;
    }
  
    if (!gmail || !token) {
      setErrorMessage('Invalid or missing reset token or email');
      return;
    }
  
    try {
      const data = {
        usernameOrEmail: gmail,
        resetToken: token,
        newPassword: password,
      };
  
      const response = await resetPass(data);
      console.log(response, 'Password reset response');
      setSuccessMessage('Password has been reset successfully!');
      navigate('/login');
    } catch (error) {
      console.error(error);
      setErrorMessage('Failed to reset password. Please try again.');
    }
  };
  
  return (
    <div className="flex items-center justify-center bg-gray-100 h-[90vh]">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">THAY ĐỔI MẬT KHẨU MỚI</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Mật Khẩu Mới"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Xác Nhận Mật Khẩu"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <Button fullWidth variant="contained" type="submit">
            Thay đổi
          </Button>
        </form>
      </div>
    </div>
  );
};

export default ResetPasswordPage;
