import { createContext, useState, useEffect, ReactNode } from 'react';
import { useNavigate } from 'react-router-dom';
import {jwtDecode} from 'jwt-decode';
import { AuthContextType, UserType } from '../types';

const defaultContextValue: AuthContextType = {
    user: null,
    login: () => {},
    logout: () => {},
};
const AuthContext = createContext<AuthContextType>(defaultContextValue);

const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<UserType | null>(null);
    const navigate = useNavigate();
  
    useEffect(() => {
      const token = localStorage.getItem('token');
      if (token) {
        const decoded = jwtDecode<UserType>(token);
        setUser(decoded);
      }
    }, []);
  
    const login = (username: string, password: string) => {
      // Here you would typically call an API to get the token
      // For example:
      // api.login(username, password).then(response => {
      //   const { token } = response.data;
      //   const decoded = jwtDecode<UserType>(token);
      //   setUser(decoded);
      //   localStorage.setItem('token', token);
      //   navigate(decoded.role === 'admin' ? '/admin' : '/');
      // });
  
      // For demonstration purposes, assuming you get a token:
      const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwicm9sZSI6ImFkbWluIiwiaWF0IjoxNjYxNzQxNDAwfQ.W8K6E3AWvZcEC9dBj9hXpD5Jfj19M'
      const decoded = jwtDecode<UserType>(token);
      localStorage.setItem('token', token);
      setUser(decoded);
      navigate(decoded.role === 'admin' ? '/admin' : '/');
    };
  
    const logout = () => {
      setUser(null);
      localStorage.removeItem('token');
      navigate('/');
    };
  
    return (
      <AuthContext.Provider value={{ user, login, logout }}>
        {children}
      </AuthContext.Provider>
    );
  };
  

export { AuthProvider, AuthContext };
