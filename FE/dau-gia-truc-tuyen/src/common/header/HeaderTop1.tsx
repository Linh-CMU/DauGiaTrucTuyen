import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '@contexts/AuthContext';
import { profileResponse } from '../../types/auth.type';
import { profileUser } from '../../queries/AdminAPI';
import { Button, Menu, MenuItem } from '@mui/material';
import { HeaderContainer } from './HeaderTop.styles';

const HeaderTop1 = () => {
  const navigate = useNavigate();
  const { logout, isAuthenticated } = useAuth();
  const [isProfileMenu, setIsProfileMenu] = useState(false);
  const [isMobileMenu, setIsMobileMenu] = useState(false);
  const [profile, setProfile] = useState<profileResponse | null>();
  const getRole = () => {
    const role = localStorage.getItem('role');
    return role;
  };
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
    navigate('/');
  };
  const toggleProfileMenu = () => {
    setIsProfileMenu(!isProfileMenu);
  };
  const toggleMobileMenu = () => {
    setIsMobileMenu(!isMobileMenu);
  };

  const onProfileClick = () => {
    navigate('/profile');
    setIsProfileMenu(false);
  };
  const onChangePassword = () => {
    navigate('/changePasswordPage');
    setIsProfileMenu(false);
  };

  // Close both menus when clicking outside
  useEffect(() => {
    const handleClickOutside = (e: any) => {
      if (!e.target.closest('#user-menu-button')) {
        setIsProfileMenu(false);
      }
      if (!e.target.closest('#mobile-menu-button')) {
        setIsMobileMenu(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, []);

  useEffect(() => {
    const fetchCity = async () => {
      try {
        const userData = await profileUser();
        setProfile(userData.result);
      } catch (error) {}
    };
    fetchCity();
  }, []);

  const [anchorEl, setAnchorEl] = useState(null);
  const open = Boolean(anchorEl);
  const handleClick = (event: any) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <HeaderContainer className="fixed top-0 z-10">
      <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
        <div className="relative flex h-16 items-center justify-between">
          <div className="absolute inset-y-0 left-0 flex items-center sm:hidden">
            <button
              id="mobile-menu-button"
              type="button"
              className="relative inline-flex items-center justify-center rounded-md p-2 text-gray-400 hover:bg-gray-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white"
              aria-controls="mobile-menu"
              aria-expanded={isMobileMenu}
              onClick={toggleMobileMenu}
            >
              <span className="sr-only">Open main menu</span>
              <svg
                className="block h-6 w-6"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
                aria-hidden="true"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5"
                />
              </svg>
            </button>
          </div>

          <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
            <div className="flex flex-shrink-0 items-center">
              <img className="h-8 w-auto" src="logo.png" alt="Your Company " />
            </div>
            <div className="hidden sm:ml-6 sm:block">
              {getRole() === 'user' ? (
                <>
                  <div className="flex space-x-4">
                    <a
                      href="/"
                      className="rounded-md bg-gray-900 px-3 py-2 text-sm font-medium text-white"
                      aria-current="page"
                    >
                      Trang chủ
                    </a>
                    <a
                      href="/listYourAuction"
                      className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                    >
                      Sản phẩm của bạn
                    </a>
                    <a
                      href="#"
                      className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                    >
                      Giới thiệu
                    </a>
                  </div>
                </>
              ) : getRole() === 'admin' ? (
                <div className="flex space-x-4">
                  <a
                    href="/dashboard"
                    className="rounded-md bg-gray-900 px-3 py-2 text-sm font-medium text-white"
                    aria-current="page"
                  >
                    Thống kê
                  </a>
                  <a
                    href="/listAuction"
                    className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  >
                    Danh sách sản phẩm
                  </a>
                  <a
                    href="/listuser"
                    className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  >
                    Danh sách người dùng
                  </a>
                </div>
              ) : (
                <div className="flex space-x-4">
                  <a
                    href="/"
                    className="rounded-md bg-gray-900 px-3 py-2 text-sm font-medium text-white"
                    aria-current="page"
                  >
                    Trang chủ
                  </a>
                  <a
                    href="#"
                    className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  >
                    Thông tin
                  </a>
                </div>
              )}
            </div>
          </div>
          {isAuthenticated() ? (
            <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
              <div className="relative ml-3">
                <div>
                  <Button
                    id="basic-button"
                    aria-controls={open ? 'basic-menu' : undefined}
                    aria-haspopup="true"
                    aria-expanded={open ? 'true' : undefined}
                    onClick={handleClick}
                  >
                    {profile ? (
                      <img
                        className="h-8 w-auto"
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.avatar}`}
                        alt="Your Profile"
                      />
                    ) : (
                      <img className="h-8 w-auto" src="logo.png" alt="Your Company" />
                    )}
                  </Button>
                  <Menu
                    id="basic-menu"
                    anchorEl={anchorEl}
                    open={open}
                    onClose={handleClose}
                    MenuListProps={{
                      'aria-labelledby': 'basic-button',
                    }}
                  >
                    <MenuItem onClick={onProfileClick}>Profile</MenuItem>
                    <MenuItem onClick={onChangePassword}>Change password</MenuItem>
                    <MenuItem onClick={onLogoutBtnClick}>Logout</MenuItem>
                  </Menu>
                </div>
              </div>
            </div>
          ) : (
            <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
              <div className="relative ml-3 flex">
                <div>
                  <Button variant="outlined" onClick={onLoginBtnClick}>
                    Đăng nhập
                  </Button>
                </div>
                <div className='ml-6'>
                  <Button className="ml-10" variant="contained" onClick={onSignUpBtnClick}>
                    Đăng kí
                  </Button>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>

      {/* Mobile Menu */}
      {isMobileMenu && (
        <div className="sm:hidden" id="mobile-menu">
          <div className="space-y-1 px-2 pb-3 pt-2">
            <a
              href="#"
              className="block rounded-md bg-gray-900 px-3 py-2 text-base font-medium text-white"
              aria-current="page"
              onClick={() => setIsMobileMenu(false)}
            >
              Trang chủ
            </a>
            <a
              href="#"
              className="block rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
              onClick={() => setIsMobileMenu(false)}
            >
              Team
            </a>
            <a
              href="#"
              className="block rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
              onClick={() => setIsMobileMenu(false)}
            >
              Projects
            </a>
            <a
              href="#"
              className="block rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
              onClick={() => setIsMobileMenu(false)}
            >
              Calendar
            </a>
          </div>
        </div>
      )}
    </HeaderContainer>
  );
};

export default HeaderTop1;
