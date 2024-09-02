import { CircularProgress } from '@mui/material';
import React from 'react';

const LoadingIndicator: React.FC = () => {
  return (
    <div className="fixed inset-0 flex items-center justify-center bg-gray-800 bg-opacity-50 z-50">
      <CircularProgress size="8rem" />
    </div>
  );
};

export default LoadingIndicator;
