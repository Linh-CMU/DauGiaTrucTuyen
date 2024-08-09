import React, { useState } from "react";
import { Button } from "@mui/material";
import LoginModal from "../../components/LoginModal";
import RegisterModal from "../../components/RegisterModal";

const HeaderTop = () => {
  const [isLoginModalOpen, setIsLoginModalOpen] = useState<boolean>(false);
  const [isRegisterModalOpen, setIsRegisterModalOpen] = useState<boolean>(false);


  const handleOpenLoginModal = () => {
    setIsLoginModalOpen(true);
  };

  const handleCloseLoginModal = () => {
    setIsLoginModalOpen(false);
  };

  return (
    <div className="h-16 w-full flex items-center justify-between px-4 bg-main">
      <div className="text-black">Đấu giá trực tuyến</div>
      <div>
        <Button variant="text" onClick={handleOpenLoginModal}>
          Đăng nhập
        </Button>
        <Button variant="contained" onClick={()=> setIsRegisterModalOpen(true) }>Đăng kí</Button>
      </div>
      <LoginModal open={isLoginModalOpen} onClose={handleCloseLoginModal} />
      <RegisterModal open={isRegisterModalOpen} onClose={()=> setIsRegisterModalOpen(false)} />
    </div>
  );
};

export default HeaderTop;
