import { useState } from 'react';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Box, Tab, Typography, styled } from '@mui/material';
import TableAuction from '../table-auction/TableAuction';

const StyledTabList = styled(TabList)(() => ({
  '& .MuiTabs-indicator': {
    backgroundColor: 'rgb(241, 155, 64)',
  },
  '& .MuiButtonBase-root': {
    outline: 'none',
  },
  '& .MuiButtonBase-root.Mui-selected': {
    color: 'rgb(241, 155, 64)',
    borderBottom: '2px solid rgb(241, 155, 64)',
  },
}));

const ListAuctionAdmin = () => { // Đổi tên từ listAuctionAdmin thành ListAuctionAdmin
  const [value, setValue] = useState(0); // Khởi tạo value là số

  const handleChange = (event: any, newValue: string) => {
    setValue(Number(newValue)); // Chuyển đổi newValue sang số
  };

  return (
    <>
      <Typography variant="h5" component="h2" fontWeight="bold">
        TÀI SẢN ĐẤU GIÁ
      </Typography>
      <div>
        <TabContext value={value.toString()}> {/* Chuyển đổi value sang chuỗi */}
          <Box>
            <StyledTabList onChange={handleChange} aria-label="lab">
              <Tab label="Tất cả" value='0' />
              <Tab label="Chưa được chấp thuận" value='1' />
              <Tab label="Từ chối" value='2' />
              <Tab label="Đã Duyệt" value='3' />
            </StyledTabList>
          </Box>
          <TabPanel value="0">
            <TableAuction tabValue={value} />
          </TabPanel>
          <TabPanel value="1"><TableAuction tabValue={value} /></TabPanel>
          <TabPanel value="2"><TableAuction tabValue={value} /></TabPanel>
          <TabPanel value="3"><TableAuction tabValue={value} /></TabPanel>
        </TabContext>
      </div>
    </>
  );
};

export default ListAuctionAdmin;
