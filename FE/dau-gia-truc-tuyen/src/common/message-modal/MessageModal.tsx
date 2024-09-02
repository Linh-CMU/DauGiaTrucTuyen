import React, { useEffect } from 'react';
import { useMessage } from '@contexts/MessageContext';
import { Alert } from '@mui/material';

const MessageModal: React.FC = () => {
  const { message, setMessage } = useMessage();

  useEffect(() => {
    const timer = setTimeout(() => {
      setMessage(null);
    }, 3000);

    return () => clearTimeout(timer);
  }, [message, setMessage]);

  return message ? (
    <Alert severity="error" className="absolute top-8 right-8">
      {message}
    </Alert>
  ) : (
    <></>
  );
};

export default MessageModal;
