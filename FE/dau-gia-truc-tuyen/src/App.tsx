import React, { useState, useContext } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AdminPage from './pages/AdminPage';
import LoginModal from './components/LoginModal';
import { AuthProvider, AuthContext } from './contexts/AuthContext';
import HeaderTop from './common/header/HeaderTop';

const App = () => {
  const [isLoginModalOpen, setLoginModalOpen] = useState(false);
  const { user } = useContext(AuthContext);
  console.log(user,"user")

  const handleOpenLoginModal = () => setLoginModalOpen(true);
  const handleCloseLoginModal = () => setLoginModalOpen(false);

  return (
        <div className="w-screen h-screen">
          <button onClick={handleOpenLoginModal}>Login</button>
          <HeaderTop />
          <LoginModal open={isLoginModalOpen} onClose={handleCloseLoginModal}/>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/admin" element={user?.role == "admin" ? <AdminPage /> : <Navigate to="/" />} />
          </Routes>
        </div>
  );
};

export default App;
