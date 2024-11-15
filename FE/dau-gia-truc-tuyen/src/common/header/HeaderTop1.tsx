import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '@contexts/AuthContext';
import { profileResponse } from '../../types/auth.type';
import { profileUser } from '../../queries/AdminAPI';



const HeaderTop1 = () => {
  const navigate = useNavigate();
  const { logout, isAuthenticated } = useAuth();
  const [isProfileMenu, setIsProfileMenu] = useState(false);
  const [isMobileMenu, setIsMobileMenu] = useState(false);
  const [profile, setProfile] = useState<profileResponse | null>();

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
  const toggleProfileMenu = () => {
    setIsProfileMenu(!isProfileMenu);
  };
  const toggleMobileMenu = () => {
    setIsMobileMenu(!isMobileMenu);
  };

  const onProfileClick = () => {
    navigate('/profile');
  };

  // Close both menus when clicking outside
  useEffect(() => {
    const handleClickOutside = (e : any) => {
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
              console.log(userData.result);
          } catch (error) {
              
          }
      };
      fetchCity();
  }, []);



  return (
    <nav className="bg-gray-800">
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
              <svg className="block h-6 w-6" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor" aria-hidden="true">
                <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5" />
              </svg>
            </button>
          </div>

          <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
            <div className="flex flex-shrink-0 items-center">
              <img 
                className="h-8 w-auto" 
                src="logo.png" 
                alt="Your Company "                  
              />
            </div>
            <div className="hidden sm:ml-6 sm:block">
              <div className="flex space-x-4">
                <a href="#" className="rounded-md bg-gray-900 px-3 py-2 text-sm font-medium text-white" aria-current="page">Dashboard</a>
                <a href="#" className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white">Team</a>
                <a href="#" className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white">Projects</a>
                <a href="#" className="rounded-md px-3 py-2 text-sm font-medium text-gray-300 hover:bg-gray-700 hover:text-white">Calendar</a>
              </div>
            </div>
          </div>
        { isAuthenticated() ? (
            <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
                <div className="relative ml-3">
                <div>
                    <button
                    type="button"
                    id="user-menu-button"
                    className="relative flex items-center rounded-full bg-gray-800 text-sm focus:outline-none focus:ring-1 focus:ring-white focus:ring-offset-"
                    aria-expanded={isProfileMenu}
                    aria-haspopup="true"
                    onClick={toggleProfileMenu}
                    >
                    <span className="sr-only">Open user menu</span>
                    {profile ? (
                      <img 
                        className="h-8 w-auto" 
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.avatar}`} 
                        alt="Your Company "                  
                      />
                    ) : (
                      <img 
                        className="h-8 w-auto" 
                        src="logo.png" 
                        alt="Your Company "                  
                      />
                      ) }
                    </button>
                </div>
                {isProfileMenu && (
                    <div
                    className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none"
                    role="menu"
                    aria-orientation="vertical"
                    aria-labelledby="user-menu-button"
                    >
                        <a href="/profile" className="block px-4 py-2 text-sm text-gray-700" role="menuitem">Profile</a>
                        <a href="#" className="block px-4 py-2 text-sm text-gray-700" role="menuitem" onClick={onLogoutBtnClick}>Sign out</a>
                    </div>
                )}
                </div>
            </div>
        ) : (
            <>
              <button variant="outlined" onClick={onLoginBtnClick}>
                Đăng nhập
              </button>
              <button variant="contained" onClick={onSignUpBtnClick}>
                Đăng kí
              </button>
            </>
        )}  
        </div>
      </div>

      {/* Mobile Menu */}
      {isMobileMenu && (
        <div className="sm:hidden" id="mobile-menu">
          <div className="space-y-1 px-2 pb-3 pt-2">
            <a href="#" className="block rounded-md bg-gray-900 px-3 py-2 text-base font-medium text-white" aria-current="page" onClick={() => setIsMobileMenu(false)}>Dashboard</a>
            <a href="#" className="block rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white" onClick={() => setIsMobileMenu(false)}>Team</a>
            <a href="#" className="block rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white" onClick={() => setIsMobileMenu(false)}>Projects</a>
            <a href="#" className="block rounded-md px-3 py-2 text-base font-medium text-gray-300 hover:bg-gray-700 hover:text-white" onClick={() => setIsMobileMenu(false)}>Calendar</a>
          </div>
        </div>
      )}
    </nav>
  );
};

export default HeaderTop1;
