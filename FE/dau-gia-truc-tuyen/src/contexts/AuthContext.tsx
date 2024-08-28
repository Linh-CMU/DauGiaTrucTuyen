import axios from 'axios';
import React, { ReactNode, createContext, useMemo, useState } from 'react';
import { AuthContextType, AuthResponse, LoginRequest, SignUpRequest } from 'types';

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  // State for token and username, initialize with values from localStorage
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('token'));
  const [username, setUsername] = useState<string | null>(() => localStorage.getItem('username'));

  // Login function to authenticate user
  const login = async (authRequest: LoginRequest): Promise<boolean> => {
    try {
      const response = await axios.post<AuthResponse>('/api/account/login', authRequest);
      const token = response.data.message;
      const username = authRequest.username;

      // Store token and username in localStorage
      localStorage.setItem('token', token);
      localStorage.setItem('username', username);

      // Update state
      setToken(token);
      setUsername(username);

      return true;
    } catch (error) {
      return false;
    }
  };

  // Logout function to clear user data
  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    setToken(null);
    setUsername(null);
  };

  // Signup function
  const signUp = async (authRequest: SignUpRequest) => {
    try {
      const response = await axios.post<AuthResponse>('/api/account/register', authRequest);
      return response.data.isSucceed;
    } catch (error) {
      return false;
    }
  };

  // Check if user is authenticated
  const isAuthenticated = () => !!token;

  // Memoize context value to optimize performance
  const provideValue = useMemo(() => {
    return { token, username, login, logout, signUp, isAuthenticated };
  }, [token, username]);

  return <AuthContext.Provider value={provideValue}>{children}</AuthContext.Provider>;
};
