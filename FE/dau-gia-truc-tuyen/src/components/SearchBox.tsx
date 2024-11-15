import {
  Button,
  InputLabel,
  MenuItem,
  Select,
  TextField,
  styled,
} from "@mui/material";
import SearchIcon from "@mui/icons-material/Search";
import { useState } from "react";
const TextFieldSearch = styled(TextField)({
  width: "100%",
  backgroundColor: "#FFF",

  "& .MuiOutlinedInput-root": {
     height: "100%",
    "& fieldset": {
      borderColor: "transparent", // Initially set the border color to transparent
    },
    "&:hover fieldset": {
      borderColor: "transparent", // Disable border color on hover
    },
    "&.Mui-focused fieldset": {
      borderColor: "#e2e8f0", // Disable border color on focus
      boxShadow: "0 0 5px 2px rgba(128, 128, 128, 0.5)",
    },
  },

  "& .Mui-focused": {
    color: "unset",
    border: "none",
  },
});

const SelectSearch = styled(Select)({
  width: "200px",
  height: "100%",
  backgroundColor: "#FFF",
});

const ButtonSearch = styled(Button)({
  width: "100%",
  minWidth: "150px",
  height: "100%",
  backgroundColor: "#F87211"
})

const SearchBox = () => {
  const [statusSearch, setStatusSearch] = useState(0);
  const handleSubmit = (value: any) => {
    console.log(value);
  };
  return (
    <div className="w-full max-w-[1120px] h-20 bg-[#F19B40] p-4 rounder-md absolute m-auto bottom-[-50px] left-0 right-0 sm:max-w-screen-sm md:max-w-screen-md lg:max-w-screen-lg">
      <form onSubmit={handleSubmit} className="flex gap-8 h-10">
        <TextFieldSearch
          id="outlined-textarea"
          placeholder="Nhập từ khoá tìm kiếm (tên, trạng thái, mã số)"
          multiline
        />
        <div>
          <SelectSearch
            labelId="demo-simple-select-label"
            id="demo-simple-select"
            value={statusSearch}
            label="Trạng thái"
            onChange={(e: any) => setStatusSearch(e.target.value)}
          >
            <MenuItem value={0}>Tất cả</MenuItem>
            <MenuItem value={1}>Đang diễn ra</MenuItem>
            <MenuItem value={2}>Sắp diễn ra</MenuItem>
            <MenuItem value={3}>Đã kết thúc</MenuItem>
          </SelectSearch>
        </div>
        <div className="w-fit">
          <ButtonSearch variant="contained" startIcon={<SearchIcon />}>
            Tìm Kiếm
          </ButtonSearch>
        </div>
      </form>
    </div>
  );
};
export default SearchBox;
