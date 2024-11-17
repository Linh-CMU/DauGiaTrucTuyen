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
    <div className="flex items-center justify-center bg-gray-100 h-[90vh]">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">Đăng Ký</h2>
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
          <Button fullWidth variant="contained" type="submit">
            Đăng Ký
          </Button>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;
