import React, { useContext } from "react";
import { AuthContext } from "../../contexts/AuthContext";

const AdminPage = () => {
  const { user } = useContext(AuthContext);
  console.log(user, "user");
  if (user?.role !== "admin") {
    return <h1>Access Denied</h1>;
  }

  return <h1>Admin Page</h1>;
};

export default AdminPage;
