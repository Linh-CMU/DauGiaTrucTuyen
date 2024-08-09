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


interface RegisterModalProps {
  open: boolean;
  onClose: () => void;
}

const RegisterModal: React.FC<RegisterModalProps> = ({ open, onClose }) => {
  const authContext = useContext(AuthContext);

  const handleClose = () => {
    //handle close login modal here
    onClose()
  }
  
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
            Text in register modal
          </Typography>
        </Box>
      </Modal>
    </div>
  );
};

export default RegisterModal;
