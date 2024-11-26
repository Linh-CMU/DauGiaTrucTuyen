import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';
import { useMessage } from '@contexts/MessageContext';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { login } = useAuth();
  const { setIsLoading } = useLoading();
  const { setSuccessMessage, setErrorMessage } = useMessage();
  const GOOGLE_CLIENT_ID =
    '800544947907-gqq1fut5e84qhsdtapqs1nf1f3rao28r.apps.googleusercontent.com';
  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setIsLoading(true);

    const success = await login({ username, password });

    setIsLoading(false);
    console.log(success);
    if (success.isSucceed) {
      if (!success.result.check) {
        navigate('/add-info');
      } else {
        if (success.result.role === 'user') {
          navigate('/');
        } else {
          navigate('/dashboard');
        }
      }
      setSuccessMessage('Login successful!');
      setUsername('');
      setPassword('');
    } else {
      setErrorMessage('Login failed!');
    }
  };

  return (
    <>
      <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
        <div className="absolute w-96 h-96 bg-blue-400 rounded-lg -rotate-6"></div>
        <div className="relative w-96 h-96 bg-white rounded-lg shadow-lg p-8 z-10">
          <h2 className="text-xl font-bold mb-6">Đăng nhập</h2>
          <form onSubmit={handleSubmit}>
            <div className="mb-4">
              <label className="block mb-2 text-sm font-medium">Tên đăng nhập hoặc email</label>
              <input
                className="w-full p-2 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-slate-300"
                placeholder="Ten Dang nhap hoặc email"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </div>
            <div className="mb-6">
              <label className="block mb-2 text-sm font-medium">Mật khẩu</label>
              <input
                type="password"
                className="w-full p-2 border-b border-gray-300 focus:outline-none focus:border-blue-500  bg-slate-300"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <div className='flex'>
              <div className="mr-auto mb-4">
                <a href="/sign-up" className="text-sm text-blue-600 hover:text-blue-700">
                    Chưa có tài khoản?
                </a>
              </div>
              <div className="text-left mb-4">
                <a href="/forgot" className="text-sm text-blue-600 hover:text-blue-700">
                  Quên mật khẩu?
                </a>
              </div>
            </div>
            <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
              Dang nhap
            </button>
          </form>
        </div>
      </div>
    </>
  );
};

export default LoginPage;
