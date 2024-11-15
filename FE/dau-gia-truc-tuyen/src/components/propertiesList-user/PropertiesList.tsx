import { useEffect, useState } from 'react';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Box, Button, Tab, Typography, styled } from '@mui/material';
import { getListAuctionOfContractor } from '../../queries/AuctionAPI';
import { AuctionUser} from 'types';
import AllPropertiesUser from '@components/all-properties/AllPropertiesUser';

const StyledTabList = styled(TabList)({
  '& .MuiTabs-indicator': {
    backgroundColor: 'rgb(241, 155, 64)', // Set custom background color for the indicator
  },

  '& .MuiButtonBase-root': {
    outline: 'none',
  },
  '& .MuiButtonBase-root.Mui-selected': {
    color: 'rgb(241, 155, 64)',
    borderBottom: '2px solid rgb(241, 155, 64)',
  },
});

const PropertiesList = () => {
  const [value, setValue] = useState('0');
  const [listAllAuction, setListAllAuction] = useState<AuctionUser[]>([]); // Initialize as an empty array

  useEffect(() => {
    const fetchListAuction = async () => {
      const response = await getListAuctionOfContractor(value || "0"); // Call API function
      if (response?.isSucceed) {
        setListAllAuction(response?.result || []); // Ensure result is an array
      } else {
        console.error("fetch list failed");
      }
    };
    fetchListAuction();
  }, [value]);
  const handleChange = (event: any, newValue: string) => {
    setValue(newValue);
  };
  return (
    <>
      <Box className="flex">
        <Typography variant="h5" component="h2" fontWeight="bold">
          TÀI SẢN ĐẤU GIÁ CỦA BẠN
        </Typography>
        <Box className="ml-auto">
          <Button
            href="/add-auction"
            className="bg-green-800 text-white"
            variant="contained"
            style={{ backgroundColor: '#1f8f1f', color: 'white' }}
          >
            Thêm sản phẩm
          </Button>
        </Box>
      </Box>
      <div>
      <TabContext value={value}>
          <Box>
            <StyledTabList onChange={handleChange} aria-label="lab">
              <Tab label="Tất cả" value="0" />
              <Tab label="Chưa được chấp thuận" value="1" />
              <Tab label="Từ chối" value="2" />
              <Tab label="Chấp nhận" value="3" />
            </StyledTabList>
          </Box>
          <TabPanel value={value}>
            <AllPropertiesUser 
              listAllAuction ={listAllAuction}
            />
          </TabPanel>
        </TabContext>
      </div>
    </>
  );
};
export default PropertiesList;
