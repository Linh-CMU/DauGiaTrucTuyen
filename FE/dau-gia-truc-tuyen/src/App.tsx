import { useContext } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import HeaderTop from "./common/header/HeaderTop";
import { AuthContext } from "./contexts/AuthContext";
import AdminPage from "./pages/AdminPage";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import SignUpPage from "./pages/SignupPage";

const App = () => {
  const { user } = useContext(AuthContext);
  console.log(user, "user");

  return (
    <div className="w-full h-screen flex flex-col bg-gray-100">
      <HeaderTop />
      <div className="container mx-auto flex-grow overflow-y-auto">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route
            path="/admin"
            element={
              user?.role == "admin" ? <AdminPage /> : <Navigate to="/" />
            }
          />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/signup" element={<SignUpPage />} />
        </Routes>
      </div>
    </div>
  );
};

export default App;
