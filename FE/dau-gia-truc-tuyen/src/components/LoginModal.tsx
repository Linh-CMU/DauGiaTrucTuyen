import React, { useState, useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';
import { Box, Button, Modal, Typography } from '@mui/material';

const style = {
  position: 'absolute' as 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  border: '2px solid #000',
  boxShadow: 24,
  p: 4,
};


interface LoginModalProps {
  open: boolean;
  onClose: () => void;
}


const LoginModal: React.FC<LoginModalProps> = ({ open, onClose }) => {
  const authContext = useContext(AuthContext);

  const handleClose = () => {
    //handle close login modal here
    onClose()
  }

  const handleLogin = () => {
    if (authContext) {
      // authContext.login(username, password);
      onClose();
    }
  };
  
  return (
    <div>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box sx={style}>
          <Typography id="modal-modal-title" variant="h6" component="h2" color="black">
            Text in a modal
          </Typography>
        </Box>
      </Modal>
    </div>
  );
};

export default LoginModal;