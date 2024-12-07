import { useAuth } from '@contexts/AuthContext';
import { useMessage } from '@contexts/MessageContext';
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
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // Check if passwords match
    if (password !== confirmPassword) {
      setSuccessMessage('Passwords do not match');
      return;
    }
    const isSuccess = await signUp({ username, password, email });
    if (isSuccess) {
      navigate('/otp', { state: { email: email } });
    } else {
      setErrorMessage('Signing up is failed');
    }
  };

  return (
    <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
      <div className="absolute w-96 h-[55%] bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-[55%] bg-white rounded-lg shadow-lg p-8 z-10">
        <h2 className="text-xl font-bold mb-6">Register</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Login Name"
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
              label="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Confirm password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <div className="text-right mb-4">
            <a href="/login" className="text-sm text-blue-600 hover:text-blue-700">
            Back?
            </a>
          </div>
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            REGISTER
          </button>
        </form>
      </div>
    </div>
  );
};

export default SignUpPage;
