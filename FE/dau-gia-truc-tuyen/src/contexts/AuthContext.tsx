import React, { createContext, useState, useContext, ReactNode } from 'react';
import axios from 'axios';
import { LoginRequest, SignUpRequest } from 'types';

interface AuthContextType {
  token: string | null;
  login: (data: LoginRequest) => Promise<boolean>;
  logout: () => void;
  signUp: (data: SignUpRequest) => Promise<boolean>;
  isAuthenticated: () => boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('token'));

  const login = async (authRequest: LoginRequest): Promise<boolean> => {
    try {
      const response = await axios.post('https://yourapi.com/login', authRequest);
      const token = response.data.token;
      localStorage.setItem('token', token);
      setToken(token);
      return true;
    } catch (error) {
      return false;
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setToken(null);
  };

  const signUp = async (authRequest: LoginRequest) => {
    try {
      const response = await axios.post('https://yourapi.com/sign-up', authRequest);
      const token = response.data.token;
      localStorage.setItem('token', token);
      setToken(token);
      return true;
    } catch (error) {
      return false;
    }
  };

  const isAuthenticated = () => !!token;

  return (
    <AuthContext.Provider value={{ token, login, logout, signUp, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
