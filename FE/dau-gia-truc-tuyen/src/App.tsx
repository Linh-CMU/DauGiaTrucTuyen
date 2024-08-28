import { Route, Routes } from 'react-router-dom';
import HeaderTop from '@common/header/HeaderTop';
import { HomePage, LoginPage, SignUpPage } from '@pages/index';

const App = () => {
  return (
    <div className="w-full h-screen flex flex-col bg-gray-100">
      <HeaderTop />
      <div className="container mx-auto flex-grow overflow-y-auto flex align-middle justify-center">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/sign-up" element={<SignUpPage />} />
        </Routes>
      </div>
    </div>
  );
};

export default App;
