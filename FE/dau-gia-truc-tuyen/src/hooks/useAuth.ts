import { AuthContext } from '@contexts/AuthContext';
import { useContext } from 'react';
import { AuthContextType } from 'types';

// Custom hook to use auth context
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
