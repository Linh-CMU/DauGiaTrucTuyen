import { Button } from "@mui/material";

const HeaderTop = () => {
  return (
    <div className="h-16 w-full flex items-center justify-between px-4 bg-main">
      <div className="text-black">Đấu giá trực tuyến</div>
      <div>
        <Button variant="text">Đăng nhập</Button>
        <Button variant="contained">Đăng kí</Button>
      </div>
    </div>
  );
};

export default HeaderTop;
