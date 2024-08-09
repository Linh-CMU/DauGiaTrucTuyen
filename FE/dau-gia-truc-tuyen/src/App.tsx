import React, { useState, useContext } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import HomePage from './pages/HomePage';
import AdminPage from './pages/AdminPage';
import { AuthContext } from './contexts/AuthContext';
import HeaderTop from './common/header/HeaderTop';

const App = () => {
  const { user } = useContext(AuthContext);
  console.log(user,"user")
  return (
        <div className="w-screen h-screen">
          <HeaderTop />
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/admin" element={user?.role == "admin" ? <AdminPage /> : <Navigate to="/" />} />
          </Routes>
        </div>
  );
};

export default App;
