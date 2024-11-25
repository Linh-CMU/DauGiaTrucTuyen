import { useAuth } from '@contexts/AuthContext';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const SignUpPage = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const navigate = useNavigate();
  const { signUp } = useAuth();

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // Check if passwords match
    if (password !== confirmPassword) {
      alert('Passwords do not match');
      return;
    }
    const isSuccess = await signUp({ username, password, email });
    if (isSuccess) {
      navigate('/otp', { state: { email: email } });
    } else {
      console.log('Signing up is failed');
    }
  };

  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
      <div className="absolute w-96 h-[55%] bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-[55%] bg-white rounded-lg shadow-lg p-8 z-10">
        <h2 className="text-xl font-bold mb-6">Đăng ký</h2>
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
              label="Email"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
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
          <div className="text-right mb-4">
            <a href="/login" className="text-sm text-blue-600 hover:text-blue-700">
              Quay về?
            </a>
          </div>
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            Đăng ký
          </button>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;
