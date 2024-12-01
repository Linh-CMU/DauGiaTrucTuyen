import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';
import { useMessage } from '@contexts/MessageContext';
import { CredentialResponse, GoogleLogin, GoogleOAuthProvider } from '@react-oauth/google';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();
  const { login, handleSuccess, handleFailure } = useAuth();
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

  const LoginWithGoogle = async (response: CredentialResponse) => {
    if (response.credential) {
      try {
        const success = await handleSuccess(response);

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
      } catch (error) {
        setErrorMessage('Login failed!');
      }
    } else {
      setErrorMessage('Login failed!');
    }
  };

  const handleError = () => {
    setErrorMessage('Login failed!');
  };
  return (
    <>
      <div className="w-full min-h-screen flex items-center justify-center bg-gray-100">
        <div className="absolute w-96 h-[40%] bg-blue-400 rounded-lg -rotate-6"></div>
        <div className="relative w-96 h-[40%] bg-white rounded-lg shadow-lg p-8 z-10">
          <h2 className="text-xl font-bold mb-6">LOGIN</h2>
          <form onSubmit={handleSubmit}>
            <div className="mb-4">
              <label className="block mb-2 text-sm font-medium">Username or Email</label>
              <input
                className="w-full p-2 border-b border-gray-300 focus:outline-none focus:border-blue-500 bg-slate-300"
                placeholder="Username or Email"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                required
              />
            </div>
            <div className="mb-6">
              <label className="block mb-2 text-sm font-medium">Password</label>
              <input
                type="password"
                className="w-full p-2 border-b border-gray-300 focus:outline-none focus:border-blue-500  bg-slate-300"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            <div className="flex">
              <div className="mr-auto mb-4">
                <a href="/sign-up" className="text-sm text-blue-600 hover:text-blue-700">
                  Don't have an account?
                </a>
              </div>
              <div className="text-left mb-4">
                <a href="/forgot" className="text-sm text-blue-600 hover:text-blue-700">
                  Forgot password?
                </a>
              </div>
            </div>
            <button className="w-full bg-blue-500 text-white p-2 rounded" type="submit">
              Login
            </button>
            <div className='mt-2'>
              <GoogleOAuthProvider clientId={GOOGLE_CLIENT_ID}>
                <div>
                  <GoogleLogin onSuccess={LoginWithGoogle} onError={() => handleError()} />
                </div>
              </GoogleOAuthProvider>
            </div>
          </form>
        </div>
      </div>
    </>
  );
};

export default LoginPage;
