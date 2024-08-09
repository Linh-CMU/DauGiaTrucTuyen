import { Button, TextField, styled } from "@mui/material";
import { FormEvent, useState } from "react";
import { useNavigate } from "react-router-dom";

const LoginButton = styled(Button)`
  background-color: #3b82f6;
  &:hover {
    background-color: #2563eb;
  }
`;

const LoginPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (username === "admin" && password === "admin") {
      navigate("/");
    } else {
      alert("Invalid Credential");
    }
  };
  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="bg-white p-6 rounded shadow-md w-full max-w-sm">
        <h2 className="text-2xl font-bold mb-4 text-center">Login</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-4">
            <TextField
              fullWidth
              label="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              required
            />
          </div>
          <div className="mb-4">
            <TextField
              fullWidth
              type="password"
              label="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <LoginButton fullWidth variant="contained" type="submit">
            Login
          </LoginButton>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
