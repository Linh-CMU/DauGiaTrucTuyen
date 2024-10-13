import HeaderTop from '@common/header/HeaderTop';
import LoadingIndicator from '@common/loading-indicator/LoadingIndicator';
import MessageModal from '@common/message-modal/MessageModal';
import ProtectedRoute from '@common/protected-route/ProtectedRoute';
import { LoadingProvider, useLoading } from '@contexts/LoadingContext';
import { MessageProvider } from '@contexts/MessageContext';
import { HomePage, LoginPage, SignUpPage, DetailPage, ListAuction, AuctionDetail, ForgotPage, ResetPasswordPage, ChangePasswordPage } from '@pages/index';
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
        <Route path="/login" element={<LoginPage />} />
        <Route path="/listAuction" element={<ListAuction />} />
        <Route path="/auctionDetail" element={<AuctionDetail />} />
        <Route path="/sign-up" element={<SignUpPage />} />
        <Route path="/forgot" element={<ForgotPage />} />
        <Route path="/resetPasswordPage" element={<ResetPasswordPage />} />
        <Route path="/changePasswordPage" element={<ChangePasswordPage />} />
        
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
