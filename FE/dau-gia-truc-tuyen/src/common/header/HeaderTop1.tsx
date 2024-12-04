import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '@contexts/AuthContext';
import { profileResponse } from '../../types/auth.type';
import { profileUser } from '../../queries/AdminAPI';
import { Button, Menu, MenuItem } from '@mui/material';
import { HeaderContainer } from './HeaderTop.styles';
import { getListNotification } from '@queries/AuctionAPI';

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
  const [notificationOpen, setNotificationOpen] = useState(false);
  const [notificationAnchorEl, setNotificationAnchorEl] = useState(null);
  const [notifications, setNotifications] = useState<any>([]);

  const handleNotificationClick = (event: any) => {
    setNotificationAnchorEl(event.currentTarget);
    setNotificationOpen(!notificationOpen);
  };

  const handleNotificationClose = () => {
    setNotificationAnchorEl(null);
    setNotificationOpen(false);
  };

  useEffect(() => {
    const fetchNotifi = async () => {
      try {
        const userData = await getListNotification();
        setNotifications(userData.result);
      } catch (error) {}
    };
    fetchNotifi();
  }, []);
  return (
    <HeaderContainer className="fixed top-0 z-10">
      <div className="mx-auto px-2 sm:px-6 lg:px-8 w-full items-center">
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
          <div className="flex flex-1 w-full justify-between items-center">
            <div className="flex flex-shrink-0 items-center gap-2">
              <img className="h-9 w-auto" src="logo.png" alt="Your Company " />
              <div>
                <h6 className="font-bold text-white">ONLINE AUCTION</h6>
                <p className=" text-white">
                  Danang City Property Auction and Service Center
                </p>
              </div>
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
                      HOME
                    </a>
                    <a
                      href="/listYourAuction"
                      className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                    >
                      MY PRODUCT
                    </a>
                    <a
                      href="/about"
                      className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                    >
                      ABOUT
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
                    DASHBOARD
                  </a>
                  <a
                    href="/listAuction"
                    className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  >
                    PRODUCT LIST
                  </a>
                  <a
                    href="/listuser"
                    className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  >
                    USER LIST
                  </a>
                </div>
              ) : (
                <div className="flex space-x-4">
                  <a
                    href="/"
                    className="rounded-md bg-gray-900 px-3 py-2 text-sm font-medium text-white"
                    aria-current="page"
                  >
                    HOME
                  </a>
                  <a
                    href="/about"
                    className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white"
                  >
                    ABOUT
                  </a>
                </div>
              )}
            </div>
            {isAuthenticated() ? (
              <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
                <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0 ">
                  <div className="relative ml-3">
                    <Button
                      id="notification-button"
                      aria-controls={notificationOpen ? 'notification-menu' : undefined}
                      aria-haspopup="true"
                      aria-expanded={notificationOpen ? 'true' : undefined}
                      onClick={handleNotificationClick}
                    >
                      <svg
                        className="w-6 h-6 text-white dark:text-white"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        width="24"
                        height="24"
                        fill="none"
                        viewBox="0 0 24 24"
                      >
                        <path
                          stroke="currentColor"
                          stroke-linecap="round"
                          stroke-linejoin="round"
                          stroke-width="2"
                          d="m10.827 5.465-.435-2.324m.435 2.324a5.338 5.338 0 0 1 6.033 4.333l.331 1.769c.44 2.345 2.383 2.588 2.6 3.761.11.586.22 1.171-.31 1.271l-12.7 2.377c-.529.099-.639-.488-.749-1.074C5.813 16.73 7.538 15.8 7.1 13.455c-.219-1.169.218 1.162-.33-1.769a5.338 5.338 0 0 1 4.058-6.221Zm-7.046 4.41c.143-1.877.822-3.461 2.086-4.856m2.646 13.633a3.472 3.472 0 0 0 6.728-.777l.09-.5-6.818 1.277Z"
                        />
                      </svg>
                    </Button>
                    <Menu
                      id="notification-menu"
                      anchorEl={notificationAnchorEl}
                      open={notificationOpen}
                      onClose={handleNotificationClose}
                      MenuListProps={{
                        'aria-labelledby': 'notification-button',
                      }}
                      className="w-[250px] max-h-[300px]"
                    >
                      {notifications.map((notification: any) => (
                        <div
                          key={notification.noticationID}
                          className="p-4 border-b border-gray-200 hover:bg-gray-100 cursor-pointer"
                          onClick={handleNotificationClose}
                        >
                          <div className="font-bold text-gray-700 truncate w-full text-[10px]">
                            {notification.title}
                          </div>
                          <div className="text-sm text-gray-500 text-[8px]">
                            {notification.description}
                          </div>
                          <div className="text-xs text-gray-400 text-[6px]">
                            {new Date(notification.createDate).toLocaleString()}
                          </div>
                        </div>
                      ))}
                      {notifications.length === 0 && (
                        <div className="p-4 text-center text-gray-500">Không có thông báo nào.</div>
                      )}
                    </Menu>
                  </div>
                </div>
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
                  <div className="ml-6">
                    <Button className="ml-10" variant="contained" onClick={onSignUpBtnClick}>
                      Đăng kí
                    </Button>
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </HeaderContainer>
  );
};

export default HeaderTop1;
