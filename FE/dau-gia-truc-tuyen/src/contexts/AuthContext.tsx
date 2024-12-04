import { CredentialResponse } from '@react-oauth/google';
import axios from 'axios';
import React, { ReactNode, createContext, useContext, useState } from 'react';
import { AuthResponse, GoogleLoginFailureResponse, LoginRequest, SignUpRequest } from 'types';

interface AuthContextType {
  token: string | null;
  username: string | null;
  login: (data: LoginRequest) => Promise<any>;
  logout: () => void;
  signUp: (data: SignUpRequest) => Promise<boolean>;
  isAuthenticated: () => boolean;
  handleSuccess: (data: CredentialResponse) => Promise<any>;
  handleFailure: (error: any) => Promise<any>;
}

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  // State for token and username, initialize with values from localStorage
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('token'));
  const [username, setUsername] = useState<string | null>(() => localStorage.getItem('username'));

  // Login function to authenticate user
  const login = async (authRequest: LoginRequest): Promise<any> => {
    try {
      const response = await axios.post<AuthResponse>('/api/account/login', authRequest);
      const token = response.data.result.token;
      const username = authRequest.username;
      const role = response.data.result.role;

      // Store token and username in localStorage
      localStorage.setItem('token', token);
      localStorage.setItem('username', username);
      localStorage.setItem('role', role);

      // Update state
      setToken(token);
      setUsername(username);
      return response.data;
    } catch (error) {
      return false;
    }
  };

  // Logout function to clear user data
  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('username');
    localStorage.removeItem('role');
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

  const handleSuccess = async (response: CredentialResponse) => {
    if (response.credential) {
      try {
        const result = await axios.post<AuthResponse>('/api/google', {
          tokenId: response.credential, // Google Identity Services trả về credential thay vì tokenId
        });

        const token = result.data.result.token;
        const role = result.data.result.role;

        // Store token and username in localStorage
        localStorage.setItem('token', token);
        localStorage.setItem('role', role);
        
        // Update state
        setToken(token);
        return result.data;
      } catch (error) {
        console.error('Error logging in with Google:', error);
      }
    } else {
      console.error('No credential received');
    }
  };

  const handleFailure = async (error: any) => {
    console.error('Login failed:', error);
  };

  // Check if user is authenticated
  const isAuthenticated = () => !!token;

  return (
    <AuthContext.Provider
      value={{
        token,
        username,
        login,
        logout,
        signUp,
        isAuthenticated,
        handleSuccess,
        handleFailure,
      }}
    >
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
