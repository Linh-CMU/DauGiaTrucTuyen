import React, { useState, useContext } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AdminPage from './pages/AdminPage';
import LoginModal from './components/LoginModal';
import { AuthProvider, AuthContext } from './contexts/AuthContext';

const App = () => {
  const [isLoginModalOpen, setLoginModalOpen] = useState(false);
  const { user } = useContext(AuthContext);

  const handleOpenLoginModal = () => setLoginModalOpen(true);
  const handleCloseLoginModal = () => setLoginModalOpen(false);

  return (
        <div>
          <button onClick={handleOpenLoginModal}>Login</button>
          <LoginModal open={isLoginModalOpen} onClose={handleCloseLoginModal} />
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/admin" element={user ? <AdminPage /> : <Navigate to="/" />} />
          </Routes>
        </div>
  );
};

export default App;
