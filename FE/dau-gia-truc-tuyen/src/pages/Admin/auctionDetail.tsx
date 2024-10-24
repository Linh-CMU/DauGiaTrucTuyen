import { Box, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { getDetailAuctionAdmin } from '../../queries/index';
import { Grid } from '@material-ui/core';
import CountdownTimer from '../../common/coutdown-timer/CountdownTimer';
import { useParams } from 'react-router-dom';

const AuctionDetail = () => {
  const [detailAuction, setDetailAuction] = useState<any | null>(null); // Khởi tạo với null
  const [loading, setLoading] = useState<boolean>(true); // Biến trạng thái để theo dõi quá trình tải
  const [error, setError] = useState<string | null>(null); // Biến trạng thái để lưu lỗi
  const targetDate = new Date('2024-12-31T23:59:59');
  const {id} = useParams();
  useEffect(() => {
    fetchDetailAuction();
  }, []);

  const fetchDetailAuction = async () => {
    try {
      const response = await getDetailAuctionAdmin(Number(id)); // Sử dụng id từ props
      console.log(response, 'data');
      if (response?.isSucceed) {
        setDetailAuction(response.result);
        console.log('123', response.result);
      } else {
        throw new Error('Fetch list failed');
      }
    } catch (error) {
      console.error('Error fetching auction details:', error);
      setError('Failed to load auction details. Please try again later.');
    } finally {
      setLoading(false); // Đặt trạng thái loading là false khi hoàn thành
    }
  };

  if (loading) {
    return <Typography>Loading...</Typography>; // Hiển thị khi đang tải
  }

  if (error) {
    return <Typography color="error">{error}</Typography>; // Hiển thị lỗi nếu có
  }

  return (
    <Box className="relative h-[150vh]">
      <Box className="flex items-center justify-center">
        <Typography className="pt-4 pl-4 text-yellow-700">Trang chủ</Typography>
        <span className="pl-2 pr-2 pt-3">|</span>
        <Typography className="pt-4">Chi tiết sản phẩm</Typography>
      </Box>
      <Box className="flex flex-col items-center mb-1.25">
        <Grid container spacing={1}>
          <Grid item xs={12} md={5}>
            <Box className="relative h-[70vh]">
              <img
                src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${detailAuction.image}`}
                alt={detailAuction.nameAuction}
                className="object-cover w-full h-full"
              />
            </Box>
          </Grid>
          <Grid item xs={12} md={7}>
            <Box className="flex flex-col">
              <Box className="flex flex-col md:flex-row items-start">
                <Typography
                  variant="h5"
                  component="h2"
                  fontWeight="bold"
                  className="pt-1 text-2xl md:text-6xl overflow-hidden text-ellipsis whitespace-normal w-full md:w-[65%]"
                >
                  {detailAuction.nameAuction}
                </Typography>
                <Box className="pt-12 md:ml-4">
                  <CountdownTimer targetDate={targetDate} />
                </Box>
              </Box>
              <Box className="h-[48vh] w-full md:w-[70%] mx-auto mt-5 flex bg-slate-300 rounded-md pl-5 pr-5">
                <Box className="flex flex-col w-1/2">
                  <Typography fontWeight="bold" className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Chủ thầu
                  </Typography>
                  <Typography fontWeight="bold" className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Người trúng thầu
                  </Typography>
                  <Typography fontWeight="bold" className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Giá khởi điểm
                  </Typography>
                  <Typography className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Bước giá
                  </Typography>
                  <Typography className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Tiền đặt trước
                  </Typography>
                  <Typography className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Thời gian bắt đầu
                  </Typography>
                  <Typography className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Thời gian Kết thúc
                  </Typography>
                  <Typography className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Hình thức thanh toán
                  </Typography>
                  <Typography className="text-2xl md:text-6xl text-ellipsis pt-4">
                    Số vòng đấu giá
                  </Typography>
                </Box>
                <Box className="w-1/2 ml-auto text-right mt-2">
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.user.fullName}
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.winBidder == null
                      ? 'Chưa có người trúng thầu'
                      : detailAuction.winBidder}
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.startingPrice
                      .toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      })
                      .replace('₫', 'VNĐ')}
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.priceStep
                      .toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      })
                      .replace('₫', 'VNĐ')}
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.moneyDeposit
                      .toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      })
                      .replace('₫', 'VNĐ')}
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    Ngày {detailAuction.startDay} : {detailAuction.startTime} giờ
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    Ngày {detailAuction.endDay} : {detailAuction.endTime} giờ
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.paymentMethod}
                  </Typography>
                  <Typography fontWeight="bold" className="pt-4">
                    {detailAuction.numberofAuctionRounds}
                  </Typography>
                </Box>
              </Box>
              <Box className="pt-3 flex justify-end h-14 mr-24">
                <button className="bg-green-500 text-white px-2 py-1 rounded mr-2">Duyệt</button>
                <button className="bg-green-500 text-white px-2 py-1 rounded mr-2">ReUp</button>
                <button className="bg-green-500 text-white px-2 py-1 rounded mr-2">
                  Xem người đăng ký
                </button>
                <button className="bg-green-500 text-white px-2 py-1 rounded mr-2">
                  Tải file thông tin
                </button>
                <button className="bg-red-500 text-white px-2 py-1 rounded">Từ chối</button>
              </Box>
            </Box>
          </Grid>
        </Grid>
        <Box className="h-[40vh] w-full justify-center mt-5">
          <Typography className="text-center px-4" variant="h5" component="h2" fontWeight="bold">
            {detailAuction.nameAuction}
          </Typography>
          <Typography className="px-4" variant="h6" component="h2" fontWeight="bold">
            Mô tả:
          </Typography>
          <Typography className="px-4" fontWeight="bold">
            - {detailAuction.description}
          </Typography>
          <Box>
            <Grid container spacing={1}>
              <Grid item xs={12} md={5}>
                <Box className="h-[50vh] mt-5">
                  <Typography className="text-center" variant="h6" component="h2" fontWeight="bold">
                    Hình ảnh chữ ký
                  </Typography>
                  <img
                    src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${detailAuction.signatureImg}`}
                    alt={detailAuction.signatureImg}
                    className="absolute ml-[9%] h-96 pt-3"
                  />
                </Box>
              </Grid>
              <Grid item xs={12} md={7}>
                <Box className="h-[50vh] mt-5">
                  <Typography className="text-center" variant="h6" component="h2" fontWeight="bold">
                    Hình ảnh bằng chứng sở hữu
                  </Typography>
                  <img
                    src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${detailAuction.tImange.imange}`}
                    alt={detailAuction.tImange.imange}
                    className="absolute ml-[18%] h-96 pt-3"
                  />
                </Box>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </Box>
    </Box>
  );
};

export default AuctionDetail;