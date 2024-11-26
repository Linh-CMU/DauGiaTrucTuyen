import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { changPassWork } from '../queries/AuthenAPI';
import { FormEvent, useState } from 'react';

const ChangePasswordPage = () => {
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (newPassword !== confirmPassword) {
      alert('Passwords do not match');
      return;
    }
    const data = {
      oldpassword: oldPassword,
      newpassword: newPassword,
    };

    try {
      const response = await changPassWork(data);
      if (response.isSucceed) {
        alert('Password changed successfully');
      } else {
        alert('Password changed successfully');
      }
    } catch (error) {
      console.error('Error changing password:', error);
      alert('Failed to change password');
    }
  };

  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-100" >
      <div className="absolute w-96 h-96 bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-96 bg-white rounded-lg shadow-lg p-8">
        <h2 className="text-xl font-bold mb-6">Thay đổi mật khẩu</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Mật Khẩu Cũ"
              value={oldPassword}
              onChange={(e) => setOldPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Mật Khẩu Mới"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Xác Nhận Mật Khẩu Mới"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            Thay đổi
          </button>
        </form>
      </div>
    </div>
  );
};

export default ChangePasswordPage;
