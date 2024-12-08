// components/ModalComponents.tsx
import React from 'react';
import { Modal, Box, Typography, Button, TextField } from '@mui/material';

interface ModalProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  setPrice?: (price: number) => void;
  users?: any[];
  setHours?: (time: number) => void;
  setMinutes?: (time: number) => void;
  setFile?: (file: File) => void;
  handleNavigateToContract?: (userId: string) => void;
}

// ApproveModal Component
export const ApproveModal: React.FC<ModalProps> = ({
  open,
  onClose,
  onConfirm,
  setHours,
  setMinutes,
  setFile, // Thêm hàm setFile để lưu file được chọn
}) => {
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
  };

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      if (setFile) {
        setFile(file); // Gọi hàm setFile để lưu file được chọn
      }
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <Typography variant="h6" component="h2">
          Auction approval confirmation
        </Typography>
        <Typography sx={{ mt: 2 }}>Are you sure you want to browse this auction?</Typography>
        {/* Vòng thời gian */}
        <Box sx={{ display: 'flex', gap: 2, marginTop: '20px' }}>
          {/* Nhập số giờ */}
          <TextField
            label="Number of hours"
            type="number"
            onChange={(e) => (setHours ? setHours(Number(e.target.value)) : null)}
            inputProps={{
              min: 0, // Không cho nhập số âm
              step: 1, // Tăng theo từng giờ
              max: 99,
            }}
            sx={{ flex: 1 }}
          />
          {/* Nhập số phút */}
          <TextField
            label="Number of minutes"
            type="number"
            onChange={(e) => (setMinutes ? setMinutes(Number(e.target.value)) : null)}
            inputProps={{
              min: 0,
              max: 59, // Giới hạn phút từ 0 đến 59
              step: 1,
            }}
            sx={{ flex: 1 }}
          />
        </Box>
        {/* Input file */}
        <Box sx={{ mt: 2 }}>
          <Typography>Confirmation file</Typography>
          <input
            type="file"
            accept=".pdf,.doc,.docx"
            onChange={handleFileChange}
            style={{ marginTop: '10px' }}
          />
        </Box>
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button
            variant="contained"
            color="success"
            onClick={() => {
              onConfirm();
            }}
          >
            Accept
          </Button>
          <Button variant="outlined" color="error" onClick={onClose}>
            Reject
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

// CancelModal Component
export const CancelModal: React.FC<ModalProps> = ({ open, onClose, onConfirm }) => {
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <Typography variant="h6" component="h2">
          Auction Cancellation Confirmation
        </Typography>
        <Typography sx={{ mt: 2 }}>Are you sure you want to cancel this auction?</Typography>
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button variant="contained" color="error" onClick={onConfirm}>
            Auction Cancellation
          </Button>
          <Button variant="outlined" onClick={onClose}>
            Close
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
export const UserModal: React.FC<ModalProps> = ({
  open,
  onClose,
  users,
  handleNavigateToContract,
}) => {
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 500,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
    maxHeight: '80vh', // Set max height for the modal
    overflowY: 'auto', // Enable vertical scroll
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box sx={style}>
        <Typography variant="h6" component="h2">
          List of subscribers
        </Typography>
        {users && users.length > 0 ? (
          <Box
            sx={{ maxHeight: 200, overflowY: 'auto' }}
            className="scrollbar-thin scrollbar-thumb-gray-300 scrollbar-track-gray-100"
          >
            {' '}
            {/* Adjust height to show 4 items */}
            <table style={{ width: '100%', borderCollapse: 'collapse' }}>
              <thead className="bg-gray-200 sticky top-0 z-10">
                <tr>
                  <th>User name</th>
                  {handleNavigateToContract && <th>View contract</th>}
                </tr>
              </thead>
              <tbody>
                {users.map((user, index) => (
                  <tr key={index}>
                    <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.userName}</td>
                    {handleNavigateToContract && (
                      <td style={{ border: '1px solid #ddd', padding: '8px' }}>
                        <button
                          className="bg-orange-400 h-8 flex justify-center items-center"
                          onClick={() => handleNavigateToContract(user?.userID)}
                        >
                          View
                        </button>
                      </td>
                    )}
                  </tr>
                ))}
              </tbody>
            </table>
          </Box>
        ) : (
          <Typography>No subscribers.</Typography>
        )}
        <Box mt={2} display="flex" justifyContent="space-between">
          <Button variant="contained" color="primary" onClick={onClose}>
            Close
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};
