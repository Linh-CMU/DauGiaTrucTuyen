import { useState, useEffect } from 'react';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Box, Tab, Typography, styled } from '@mui/material';
import AllProperties from '../all-properties/AllProperties';
import { getAuctionRegistration, getListAuction } from '../../queries/index';
import {Auction} from 'types';

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
  const [listAllAuction, setListAllAuction] = useState<Auction[]>([]); // Initialize as an empty array

  useEffect(() => {
    const fetchListAuction = async () => {
      try {
        const response =
          value === '1'
            ? await getAuctionRegistration()
            : await getListAuction(value || '0');
        
        if (response?.isSucceed) {
          setListAllAuction(Array.isArray(response.result) ? response.result : []);
        } else {
          console.error('Failed to fetch auction list');
        }
      } catch (error) {
        console.error('Error fetching auction list:', error);
      }
    };
    if(listAllAuction) {
      setListAllAuction([])
    }
    fetchListAuction();
  }, [value]);
  const handleChange = (event: any, newValue: string) => {
    setValue(newValue);
  };

  return (
    <>
      <Typography variant="h5" component="h2" fontWeight="bold">
        TÀI SẢN ĐẤU GIÁ
      </Typography>
      <div>
        <TabContext value={value}>
          <Box>
            <StyledTabList onChange={handleChange} aria-label="lab">
              <Tab label="Tất cả" value="0" />
              <Tab label="Đã đăng kí" value="1" />
              <Tab label="Sắp diễn ra" value="2" />
              <Tab label="Đã kết thúc" value="3" />
            </StyledTabList>
          </Box>
          <TabPanel value={value}>
            <AllProperties 
              listAllAuction={listAllAuction} 
              value={value === '1' ? 'phien-dau-gia' : 'thong-tin-chi-tiet'}
            />

          </TabPanel>
        </TabContext>
      </div>
    </>
  );
};
export default PropertiesList;
