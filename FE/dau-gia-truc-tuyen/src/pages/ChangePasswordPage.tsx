import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { FormEvent, useState } from 'react';

const ChangePasswordPage = () => {
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // Check if passwords match
    if (password !== confirmPassword) {
      alert('Passwords do not match');
      return;
    }
  };

  return (
    <div className="flex items-center justify-center bg-gray-100 h-[90vh]">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">THAY ĐỔI MẬT</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Mật Khẩu Củ"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Mật Khẩu Mới"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
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

export default ChangePasswordPage;
