import { useLoading } from '@contexts/LoadingContext';
import { Button, TextField } from '@mui/material';
import { forgetPassword } from '../queries/AuthenAPI';
import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMessage } from '@contexts/MessageContext';

const ForgotPage = () => {
  const [username, setUsername] = useState('');
  const { setIsLoading } = useLoading();
  const navigate = useNavigate();
  const {setSuccessMessage, setErrorMessage} = useMessage();
  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    const success = await forgetPassword(username);
    setIsLoading(false);
    if (success) {
      navigate('/login');
      setSuccessMessage('Vui lòng bạn kiểm tra mail');
    } else {
      setErrorMessage('username không tồn tại!!!');
    }
  };
  return (
    <div className="flex items-center justify-center bg-gray-100 h-[90vh]">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">QUÊN MẬT KHẨU</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Tên Đăng Nhập"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <Button fullWidth variant="contained" type="submit">
            GỬI
          </Button>
        </form>
      </div>
    </div>
  );
};

export default ForgotPage;
