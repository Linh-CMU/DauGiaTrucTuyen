// src/HeaderTop.js

import { useAuth } from '@contexts/AuthContext';
import { Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { HeaderContainer, Title, UserInfo } from './HeaderTop.styles';

const HeaderTop = () => {
  const { username, logout, isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const onTitleClick = () => {
    navigate('/');
  };
  const onLoginBtnClick = () => {
    navigate('/login');
  };
  const onSignUpBtnClick = () => {
    navigate('/sign-up');
  };
  const onLogoutBtnClick = () => {
    logout();
    navigate('./login');
  };
  return (
    <HeaderContainer>
      <div className="container mx-auto flex justify-between items-center">
        <Title className="cursor-pointer" onClick={onTitleClick}>
          <div className="flex gap-2">
            <img src="logo.png" alt="logo" width="40px" height="40px"/>
            <div className="flex flex-col">
            Đấu giá trực tuyến
            <span className="text-[10px]">Trung tâm dịch vụ đấu giá tài sản thành phố Đà Nẵng</span>

            </div>
          </div>
        </Title>
        <div className="flex align-middle gap-1">
          {isAuthenticated() ? (
            <>
              <UserInfo>Hi {username}!</UserInfo>{' '}
              <Button variant="outlined" onClick={onLogoutBtnClick}>
                Đăng xuất
              </Button>
            </>
          ) : (
            <>
              <Button variant="outlined" onClick={onLoginBtnClick}>
                Đăng nhập
              </Button>
              <Button variant="contained" onClick={onSignUpBtnClick}>
                Đăng kí
              </Button>
            </>
          )}
        </div>
      </div>
    </HeaderContainer>
  );
};

export default HeaderTop;
