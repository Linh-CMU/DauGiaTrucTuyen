import { useState, useEffect } from 'react';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Box, Tab, Typography, styled } from '@mui/material';
import AllProperties from '../all-properties/AllProperties';
import { getAuctionRegistration, getListAuction } from '../../queries/index';
import { Auction } from 'types';
import { useLocation } from 'react-router-dom';

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

interface PropertySearch {
  searchResults: any;
  isSearch: boolean;
  setIsSearch: (data: boolean) => void;
}

const PropertiesList = ({ searchResults, isSearch, setIsSearch }: PropertySearch) => {
  const location = useLocation();
  const { data } = location.state || {};

  useEffect(() => {
    if (data) {
      setValue(data);
    }
  }, [data]);

  const [value, setValue] = useState('0');
  const [listAllAuction, setListAllAuction] = useState<Auction[]>([]);

  useEffect(() => {
    const fetchListAuction = async () => {
      try {
        if (isSearch) {
          setValue('0');
          setListAllAuction(searchResults.result);
          return;
        }

        // Lấy dữ liệu dựa trên giá trị `value`
        const response =
          value === '4' ? await getAuctionRegistration() : await getListAuction(value || '0');

        if (response?.isSucceed) {
          setListAllAuction(Array.isArray(response.result) ? response.result : []);
        } else {
          console.error('Failed to fetch auction list');
        }
      } catch (error) {
        console.error('Error fetching auction list:', error);
      }
    };

    fetchListAuction();
  }, [value, isSearch, searchResults]);

  const handleChange = (event: any, newValue: string) => {
    setIsSearch(false);
    setValue(newValue);
  };

  const getRole = () => {
    const role = localStorage.getItem('role');
    return role;
  };

  return (
    <>
      <Typography variant="h5" component="h2" fontWeight="bold">
        AUCTION ASSETS
      </Typography>
      <div>
        <TabContext value={value}>
          <Box>
            <StyledTabList onChange={handleChange} aria-label="lab">
              <Tab label="ALL" value="0" />
              <Tab label="In progress" value="1" />
              <Tab label="Coming soon" value="2" />
              <Tab label="Ended" value="3" />
              {getRole() === 'user' && <Tab label="Registered" value="4" />}
            </StyledTabList>
          </Box>
          <TabPanel value={value}>
            <AllProperties
              listAllAuction={listAllAuction}
              value={value === '4' ? 'phien-dau-gia' : 'thong-tin-chi-tiet'}
            />
          </TabPanel>
        </TabContext>
      </div>
    </>
  );
};

export default PropertiesList;
