import { useAuth } from '@contexts/AuthContext';
import { useMessage } from '@contexts/MessageContext';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import { createAccount } from '@queries/AdminAPI';
import { getCategory } from '@queries/AuctionAPI';
import { FormEvent, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const AddAccountAdmin = () => {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [department, setDepartment] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const navigate = useNavigate();
  const { signUp } = useAuth();
  const [listCategory, setCategory] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const { setErrorMessage, setSuccessMessage } = useMessage();
  // Lấy danh sách danh mục
  const fetchListCategory = async () => {
    try {
      const response = await getCategory();
      if (response?.isSucceed) {
        setCategory(response?.result);
      } else {
        setErrorMessage('Failed to fetch categories');
      }
    } catch (error) {
      setErrorMessage('Error fetching categories:'+ error);
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    fetchListCategory();
  }, []);
  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    // Check if passwords match
    if (password !== confirmPassword) {
      setErrorMessage('Passwords do not match');
      return;
    }
    const isSuccess = await createAccount(username, password, email, department);
    if (isSuccess) {
      navigate('/listuser');
      setSuccessMessage('Succcessfully');
    } else {
      setErrorMessage('Signing up is failed');
    }
  };

  return (
    <div className="w-full min-h-screen mt-7 flex items-center justify-center bg-gray-100">
      <div className="absolute w-96 h-[70%] bg-blue-400 rounded-lg -rotate-6"></div>
      <div className="relative w-96 h-[55%] bg-white rounded-lg shadow-lg p-8 z-10">
        <h2 className="text-xl font-bold mb-6">CREATE ACCOUNT</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Username"
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
              label="Confirm Password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <select
              className="appearance-none block w-full bg-white text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
              id="grid-gender"
              required
              value={department}
              onChange={(e) => setDepartment(e.target.value)}
            >
              <option value="">All</option>
              {listCategory.map((category) => (
                <option key={category.categoryID} value={category.categoryID}>
                  {category.nameCategory}
                </option>
              ))}
            </select>
          </div>
          <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
            CREATE
          </button>
        </form>
      </div>
    </div>
  );
};

export default AddAccountAdmin;
