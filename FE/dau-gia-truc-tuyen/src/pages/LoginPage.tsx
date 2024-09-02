import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';
import { Button, TextField } from '@mui/material';
import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();
  const { setIsLoading } = useLoading();

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);
    const success = await login({ username, password });
    setIsLoading(false);
    if (success) {
      navigate('/');
      console.log('Login successful!');
    } else {
      console.error('Login failed!');
    }
  };
  return (
    <div className="flex items-center justify-center bg-gray-100">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">ĐĂNG NHẬP</h2>
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
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Mật Khẩu"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <Button fullWidth variant="contained" type="submit">
            ĐĂNG NHẬP
          </Button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
