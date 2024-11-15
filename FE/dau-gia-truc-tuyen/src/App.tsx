import HeaderTop from '@common/header/HeaderTop';
import HeaderTop1 from '@common/header/HeaderTop1';
import LoadingIndicator from '@common/loading-indicator/LoadingIndicator';
import MessageModal from '@common/message-modal/MessageModal';
import ProtectedRoute from '@common/protected-route/ProtectedRoute';
import { LoadingProvider, useLoading } from '@contexts/LoadingContext';
import { MessageProvider } from '@contexts/MessageContext';
import AddInfo from '@pages/Admin/AddInfo';
import ListAccountPage from '@pages/Admin/ListAccountPage';
import Profile from '@pages/Admin/Profile';
import { HomePage, LoginPage, SignUpPage, DetailPage, ListAuction, AuctionDetail, ForgotPage, ResetPasswordPage, ChangePasswordPage } from '@pages/index';
import Login1 from '@pages/Login1';
import { Route, Routes, useLocation  } from 'react-router-dom';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';
import Footer from '@common/footer/Footer';
import Test from '@pages/Admin/Test';
import Room from '@pages/Admin/Room';
import Home from '@pages/Dashboard/Home';




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
        <Route path="/login1" element={<Login1 />} />
        <Route path="/listAuction" element={<ListAuction />} />
        <Route path="/auctionDetail" element={<AuctionDetail />} />
        <Route path="/sign-up" element={<SignUpPage />} />
        <Route  path='/listuser' element={<ListAccountPage />}/>
        <Route  path='/add-info' element={<AddInfo />}/>
        <Route  path='/profile' element={<Profile />}/>
        <Route  path='/test' element={<Test />}/>
        <Route path="/forgot" element={<ForgotPage />} />
        <Route path="/resetPasswordPage" element={<ResetPasswordPage />} />
        <Route path="/changePasswordPage" element={<ChangePasswordPage />} />
        <Route  path='/room/:id' element={<Room />}/>
        <Route  path='/dashboard' element={<Home />}/>
      </Routes>
    </>
  );
};

const App = () => {
  const location = useLocation();
  const hidenHeader = ['/login1'];
  return (
    <LoadingProvider>
      <MessageProvider>
        <div className="w-full flex flex-col bg-white">
          { !hidenHeader.includes(location.pathname) && <HeaderTop1 />}
          <div className="bg-white">
            <AppRoutes />
          </div>
          <Footer />
        </div>
      </MessageProvider>
    </LoadingProvider>
  );
};

export default App;
