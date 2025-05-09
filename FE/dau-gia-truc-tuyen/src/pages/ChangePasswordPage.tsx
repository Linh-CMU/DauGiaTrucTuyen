import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { changPassWork } from '../queries/AuthenAPI';
import { FormEvent, useState } from 'react';
import { useMessage } from '@contexts/MessageContext';

const ChangePasswordPage = () => {
  const [oldPassword, setOldPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (newPassword !== confirmPassword) {
      setSuccessMessage('Passwords do not match');
      return;
    }
    const data = {
      oldpassword: oldPassword,
      newpassword: newPassword,
    };

    try {
      const response = await changPassWork(data);
      if (response.isSucceed) {
        setSuccessMessage('Password changed successfully');
      } else {
        setErrorMessage('Password changed failed');
      }
    } catch (error) {
      setErrorMessage('Failed to change password');
    }
  };

  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-100" >
      <div className="absolute w-96 h-96 bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-96 bg-white rounded-lg shadow-lg p-8">
        <h2 className="text-xl font-bold mb-6">Change Password</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Old Password"
              value={oldPassword}
              onChange={(e) => setOldPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="New Password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Confirm New Password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            CHANGE
          </button>
        </form>
      </div>
    </div>
  );
};

export default ChangePasswordPage;
