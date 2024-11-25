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
  const { setSuccessMessage, setErrorMessage } = useMessage();
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
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
      <div className="absolute w-96 h-72 bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-72 bg-white rounded-lg shadow-lg p-8 z-10">
        <h2 className="text-xl font-bold mb-6">Quên mật khẩu</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Tên Đăng Nhập hoặc Email"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div className="text-right mb-4">
            <a href="/login" className="text-sm text-blue-600 hover:text-blue-700">
              Quay về?
            </a>
          </div>
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            Gửi
          </button>
        </form>
      </div>
    </div>
  );
};

export default ForgotPage;
