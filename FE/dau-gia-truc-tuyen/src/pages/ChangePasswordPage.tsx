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
      if(response.isSucceed){
        alert('Password changed successfully');
      }else{
        alert('Password changed successfully');
      }
    } catch (error) {
      console.error('Error changing password:', error);
      alert('Failed to change password');
    }
  };

  return (
    <div className="flex items-center justify-center bg-gray-100 h-[90vh]">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">THAY ĐỔI MẬT KHẨU</h2>
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
          <Button fullWidth variant="contained" type="submit">
            Thay đổi
          </Button>
        </form>
      </div>
    </div>
  );
};

export default ChangePasswordPage;
