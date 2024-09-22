import { useState } from 'react';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Box, Tab, Typography, styled } from '@mui/material';
import AllProperties from '../all-properties/AllProperties';

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
  const [value, setValue] = useState('1');

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
              <Tab label="Tất cả" value="1" />
              <Tab label="Đang diễn ra" value="2" />
              <Tab label="Sắp diễn ra" value="3" />
              <Tab label="Đã kết thúc" value="4" />
            </StyledTabList>
          </Box>
          <TabPanel value="1">
            <AllProperties />
          </TabPanel>
          <TabPanel value="2">Đang diễn ra</TabPanel>
          <TabPanel value="3">Sắp diễn ra</TabPanel>
          <TabPanel value="4">Đã kết thúc</TabPanel>
        </TabContext>
      </div>
    </>
  );
};
export default PropertiesList;
