import React, { useState, useContext } from 'react';
import { AuthContext } from '../contexts/AuthContext';

interface LoginModalProps {
  open: boolean;
  onClose: () => void;
}

const LoginModal: React.FC<LoginModalProps> = ({ open, onClose }) => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const authContext = useContext(AuthContext);

  const handleLogin = () => {
    if (authContext) {
      authContext.login(username, password);
      onClose();
    }
  };

  if (!open) return null;

  return (
    <div className="modal">
      <div className="modal-content">
        <h2>Login</h2>
        <input
          type="text"
          placeholder="Username"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <button onClick={handleLogin}>Login</button>
        <button onClick={onClose}>Close</button>
      </div>
    </div>
  );
};

export default LoginModal;
