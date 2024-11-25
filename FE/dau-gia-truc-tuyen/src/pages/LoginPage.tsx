import { useAuth } from '@contexts/AuthContext';
import { useLoading } from '@contexts/LoadingContext';
import { useMessage } from '@contexts/MessageContext';
import { Button, TextField, IconButton, InputAdornment } from '@mui/material';
import { FormEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';
// import { GoogleLogin } from 'react-google-login';

const LoginPage = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
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
      // Optionally, reset the form
      setUsername('');
      setPassword('');
    } else {
      setErrorMessage('Login failed!');
    }
  };
  // const onSuccess = (res: any) => {
  //   console.log('Login google', res.profileObj);
  // }
  // const onFailure = (res: any) => {
  //   console.log('Login fail', res);
  // }
  const handleClickShowPassword = () => setShowPassword((prev) => !prev);
  const handleMouseDownPassword = (event: React.MouseEvent) => event.preventDefault();

  return (
    <div className="flex items-center justify-center bg-gray-100 min-h-screen">
      <div className="bg-white p-6 rounded shadow-md w-[25rem]">
        <h2 className="text-2xl font-bold mb-4 text-center">ĐĂNG NHẬP</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Tên Đăng Nhập"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
              sx={{ marginBottom: '1rem' }}
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type={showPassword ? 'text' : 'password'}
              label="Mật Khẩu"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              sx={{ marginBottom: '1rem' }}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <IconButton
                      onClick={handleClickShowPassword}
                      onMouseDown={handleMouseDownPassword}
                      edge="end"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                ),
              }}
            />
          </div>
          <div className="text-right mb-4">
            <a href="/forgot" className="text-sm text-blue-600 hover:text-blue-700">
              Quên mật khẩu?
            </a>
          </div>
          {/* <div>
            <GoogleLogin
              clientId={GOOGLE_CLIENT_ID}
              buttonText="Login with Google"
              onSuccess={onSuccess}
              onFailure={onFailure}
              cookiePolicy={'single_host_origin'}
            />
          </div> */}
          <Button
            fullWidth
            variant="contained"
            type="submit"
            sx={{
              backgroundColor: '#6200ea',
              '&:hover': {
                backgroundColor: '#3700b3',
              },
            }}
          >
            ĐĂNG NHẬP
          </Button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
