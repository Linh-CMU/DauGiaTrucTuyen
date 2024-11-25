import { Button, InputLabel, MenuItem, Select, TextField, styled } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import { useEffect, useState } from 'react';
import { getCategory } from '@queries/AuctionAPI';
const TextFieldSearch = styled(TextField)({
  width: '100%',
  backgroundColor: '#FFF',

  '& .MuiOutlinedInput-root': {
    height: '100%',
    '& fieldset': {
      borderColor: 'transparent', // Initially set the border color to transparent
    },
    '&:hover fieldset': {
      borderColor: 'transparent', // Disable border color on hover
    },
    '&.Mui-focused fieldset': {
      borderColor: '#e2e8f0', // Disable border color on focus
      boxShadow: '0 0 5px 2px rgba(128, 128, 128, 0.5)',
    },
  },

  '& .Mui-focused': {
    color: 'unset',
    border: 'none',
  },
});

const SelectSearch = styled(Select)({
  width: '200px',
  height: '100%',
  backgroundColor: '#FFF',
});

const ButtonSearch = styled(Button)({
  width: '100%',
  minWidth: '150px',
  height: '100%',
  backgroundColor: '#F87211',
});
interface PropertySearch {
  handleSubmit: (statusSearch: number, search: string) => void;
}
const SearchBox = ({ handleSubmit }: PropertySearch) => {
  const [statusSearch, setStatusSearch] = useState(0);
  const [search, setSearch] = useState('');
  const [listCategory, setCategory] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  // Lấy danh sách danh mục
  const fetchListCategory = async () => {
    try {
      const response = await getCategory();
      if (response?.isSucceed) {
        setCategory(response?.result);
      } else {
        console.error('Failed to fetch categories');
      }
    } catch (error) {
      console.error('Error fetching categories:', error);
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    fetchListCategory();
  }, []);
  return (
    <div className="w-full max-w-[1120px] h-20 bg-[#F19B40] p-4 rounder-md absolute m-auto bottom-[-50px] left-0 right-0 sm:max-w-screen-sm md:max-w-screen-md lg:max-w-screen-lg">
      <div className="flex gap-8 h-10">
        <TextFieldSearch
          id="outlined-textarea"
          placeholder="Nhập từ khoá tìm kiếm (tên, trạng thái, mã số)"
          onChange={(e) => setSearch(e.target.value)}
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
            {listCategory.map((category) => (
              <MenuItem key={category.categoryID} value={category.categoryID}>
                {category.nameCategory}
              </MenuItem>
            ))}
          </SelectSearch>
        </div>
        <div className="w-fit">
          <ButtonSearch onClick={() => handleSubmit(statusSearch, search)} variant="contained" startIcon={<SearchIcon />}>
            Tìm Kiếm
          </ButtonSearch>
        </div>
      </div>
    </div>
  );
};
export default SearchBox;
