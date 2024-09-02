// src/HeaderTop.js
import { Button } from "@mui/material";
import { styled } from "@mui/material/styles";
import { useNavigate } from "react-router-dom";

const HeaderContainer = styled("div")`
  height: 4rem;
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 1rem;
  background-color: #1a202c; /* Darker background for better contrast */
`;

const Title = styled("div")`
  font-size: 1.25rem; /* Increase font size */
  font-weight: bold;
  color: #f7fafc; /* Lighter text color for contrast */
`;

const StyledButton = styled(Button)`
  &.MuiButton-text {
    color: #f7fafc;
    margin-right: 0.5rem;
    &:hover {
      background-color: rgba(255, 255, 255, 0.1); /* Light hover effect */
    }
  }

  &.MuiButton-contained {
    background-color: #3182ce;
    color: #f7fafc;
    &:hover {
      background-color: #2b6cb0;
    }
  }
`;

const HeaderTop = () => {
  const navigate = useNavigate();
  const onTitleClick = () => {
    navigate("/");
  };
  const onLoginBtnClick = () => {
    navigate("/login");
  };
  const onSignupBtnClick = () => {
    navigate("/signup");
  };
  return (
    <HeaderContainer>
      <div className="container mx-auto flex justify-between items-center">
        <Title className="cursor-pointer" onClick={onTitleClick}>
          <div className="flex gap-2">
            <img src="logo.png" alt="logo" width="40px" height="40px"/>
            <div className="flex flex-col">
            Cá độ trực tuyến
            <span className="text-[10px]">Trung tâm dịch vụ đấu giá tài sản thành phố Đà Nẵng</span>

            </div>
          </div>
        </Title>
        <div>
          <StyledButton variant="text" onClick={onLoginBtnClick}>
            Đăng nhập
          </StyledButton>
          <StyledButton variant="contained" onClick={onSignupBtnClick}>
            Đăng kí
          </StyledButton>
        </div>
      </div>
    </HeaderContainer>
  );
};

export default HeaderTop;
