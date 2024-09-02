import HeaderTop from '@common/header/HeaderTop';
import LoadingIndicator from '@common/loading-indicator/LoadingIndicator';
import MessageModal from '@common/message-modal/MessageModal';
import ProtectedRoute from '@common/protected-route/ProtectedRoute';
import { LoadingProvider, useLoading } from '@contexts/LoadingContext';
import { MessageProvider } from '@contexts/MessageContext';
import AddActionPage from '@pages/AddAuctionPage';
import { HomePage, LoginPage, SignUpPage } from '@pages/index';
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
        </Route>
        <Route path="/" element={<ProtectedRoute />}>
          <Route path="/add-auction" element={<AddActionPage />} />
        </Route>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/sign-up" element={<SignUpPage />} />
      </Routes>
    </>
  );
};

const App = () => {
  return (
    <LoadingProvider>
      <MessageProvider>
        <div className="w-full h-screen flex flex-col bg-gray-100">
          <HeaderTop />
          <div className="container mx-auto flex-grow overflow-y-auto flex align-middle justify-center">
            <AppRoutes />
          </div>
        </div>
      </MessageProvider>
    </LoadingProvider>
  );
};

export default App;
