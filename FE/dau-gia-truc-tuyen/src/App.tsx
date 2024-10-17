import HeaderTop from '@common/header/HeaderTop';
import LoadingIndicator from '@common/loading-indicator/LoadingIndicator';
import MessageModal from '@common/message-modal/MessageModal';
import ProtectedRoute from '@common/protected-route/ProtectedRoute';
import { LoadingProvider, useLoading } from '@contexts/LoadingContext';
import { MessageProvider } from '@contexts/MessageContext';
import AddInfo from '@pages/Admin/AddInfo';
import ListAccountPage from '@pages/Admin/ListAccountPage';
import Profile from '@pages/Admin/Profile';
import AddActionPage from '@pages/User/AddActionPage';
import { HomePage, LoginPage, SignUpPage, 
  DetailPage, ListAuction, AuctionDetail, ForgotPage, 
  ResetPasswordPage, ChangePasswordPage, ListYourAuction, AuctionRoom } from '@pages/index';
import { Route, Routes } from 'react-router-dom';

const AppRoutes: React.FC = () => {
  const { isLoading } = useLoading();
  return (
    <>
      <MessageModal />
      {isLoading && <LoadingIndicator />}

      <Routes>
        <Route path="/" element={<ProtectedRoute />}>
          <Route path="/" element={<HomePage />} />
          <Route path="/thong-tin-chi-tiet/:id" element={<DetailPage/>} />
        </Route>
        <Route path="/add-auction" element={<AddActionPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/listAuction" element={<ListAuction />} />
        <Route path="/auctionDetail/:id" element={<AuctionDetail />} />
        <Route path="/sign-up" element={<SignUpPage />} />
        <Route  path='/listuser' element={<ListAccountPage />}/>
        <Route  path='/add-info' element={<AddInfo />}/>
        <Route  path='/profile' element={<Profile />}/>
        <Route path="/forgot" element={<ForgotPage />} />
        <Route path="/auctionRoom" element={<AuctionRoom />} />
        <Route path="/resetPasswordPage" element={<ResetPasswordPage />} />
        <Route path="/changePasswordPage" element={<ChangePasswordPage />} />
        <Route path="/listYourAuction" element={<ListYourAuction />} />
      </Routes>
    </>
  );
};

const App = () => {
  return (
    <LoadingProvider>
      <MessageProvider>
        <div className="w-full flex flex-col bg-white">
          <HeaderTop />
          <div className="bg-white">
            <AppRoutes />
          </div>
        </div>
      </MessageProvider>
    </LoadingProvider>
  );
};

export default App;
