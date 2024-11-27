import { Box, Typography } from '@mui/material';
import { useEffect, useState } from 'react';
import { approveAuction, getDetailAuctionAdmin, getListUserAdmin } from '../../queries/index';
import { Grid } from '@material-ui/core';
import CountdownTimer from '../../common/coutdown-timer/CountdownTimer';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { ApproveModal, CancelModal, UserModal } from '@components/modalAccept/ApproveModal';
import { convertDate } from '@utils/helper';
import AutoGraphIcon from '@mui/icons-material/AutoGraph';
import SearchIcon from '@mui/icons-material/Search';

const AuctionDetail = () => {
  const [detailAuction, setDetailAuction] = useState<any | null>(null); // Khởi tạo với null
  const [loading, setLoading] = useState<boolean>(true); // Biến trạng thái để theo dõi quá trình tải
  const [error, setError] = useState<string | null>(null); // Biến trạng thái để lưu lỗi
  const [isApproveModalOpen, setApproveModalOpen] = useState(false); // Modal state
  const [isUserModalOpen, setUserModalOpen] = useState(false); // Modal state
  const [isApproveModalCancelOpen, setApproveModalCancelOpen] = useState(false); // Modal cancel state
  const [price, setPrice] = useState<number | null>(null);
  const [listUser, setUser] = useState<any[]>([]);
  const [swith, setSwith] = useState(false);
  const [bidHistory, setBidHistory] = useState([]);
  const [currentPrice, setCurrentPrice] = useState(0);
  // const targetDate = convertDate(detailAuction?.endTime, detailAuction?.endDay);
  const [hours, setHours] = useState<number | ''>('');
  const [minutes, setMinutes] = useState<number | ''>('');
  const { id } = useParams();
  useEffect(() => {
    fetchDetailAuction();
    fetchListUser();
  }, []);
  const fetchListUser = async () => {
    const response = await getListUserAdmin(Number(id));
    console.log(response, 'data');
    if (response?.isSucceed) {
      setUser(response?.result);
      console.log('ds', listUser);
    } else {
      console.error('fetch list fail');
    }
  };
  const handleApprove = () => {
    setApproveModalOpen(true); // Open approval modal
  };
  const handleUser = () => {
    setUserModalOpen(true); // Open approval modal
  };

  const handleReject = () => {
    setApproveModalCancelOpen(true); // Open cancel modal
  };
  const navigate = useNavigate();

  const handleToInfor = (iduser: string) => {
    // Pass `iduser` as part of the state to the `/inforUser` route
    navigate('/inforUser', { state: { iduser: iduser, status: true } });
  };
  const handleModalApprove = async () => {
    if (id) {
      const formattedHours = (hours || 0).toString().padStart(2, '0');
      const formattedMinutes = (minutes || 0).toString().padStart(2, '0');
      const totalTime = `${formattedHours}:${formattedMinutes}`;
      const response = await approveAuction(Number(id), true, price, totalTime);
      if (response.isSucceed) {
        fetchDetailAuction();
        alert('Bạn đã phê duyệt thành công');
      }
    }
    setApproveModalOpen(false);
  };
  const handleModalUser = async () => {
    setUserModalOpen(false);
  };
  const handleModalReject = async () => {
    if (id) {
      const response = await approveAuction(Number(id), false, price, '00:00');
      if (response.isSucceed) {
        fetchDetailAuction();
        alert('Bạn đã từ chối với đơn hàng đấu giá này');
      }
    }
    setApproveModalOpen(false);
  };

  const handleModalClose = () => {
    setApproveModalOpen(false); // Close approval modal
  };
  const handleModalUserClose = () => {
    setUserModalOpen(false); // Close approval modal
  };
  const handleModalCancelClose = () => {
    setApproveModalCancelOpen(false); // Close cancel modal
  };
  const handleClosepopup = () => {
    setSwith(false); // Close cancel modal
  };
  useEffect(() => {
    const socket = new WebSocket(`ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=${id}`);

    socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      setBidHistory(data);
      setCurrentPrice(data[0]?.Price);
    };

    return () => {
      socket.close();
    };
  }, [id]);
  const fetchDetailAuction = async () => {
    try {
      console.log('id', id);

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
  interface InfoRowProps {
    label: string;
    value: string;
  }
  const InfoRow: React.FC<InfoRowProps> = ({ label, value }) => (
    <>
      <div className="flex justify-between py-1">
        <div className="font-bold">{label}</div>
        <div className="">{value}</div>
      </div>
      <div className="h-[2px] w-full bg-gray-200"></div>
    </>
  );
  if (loading) {
    return <Typography>Loading...</Typography>; // Hiển thị khi đang tải
  }

  if (error) {
    return <Typography color="error">{error}</Typography>; // Hiển thị lỗi nếu có
  }
  const calculateNewTargetDate = (endTime: string, endDay: string, timePerLap: string): Date => {
    if (!endTime || !endDay || !timePerLap) {
      console.warn('Missing required parameters for calculateNewTargetDate:', {
        endTime,
        endDay,
        timePerLap,
      });
      return new Date(); // Trả về thời gian hiện tại nếu thiếu tham số
    }

    try {
      // Sử dụng convertDate để tạo đối tượng ngày ban đầu
      const endDate = convertDate(endTime, endDay);
      const now = new Date();

      // Tách giờ và phút từ TimePerLap
      const [hours, minutes] = timePerLap.split(':').map(Number);

      if (endDate < now) {
        // Nếu endDate đã qua, cộng thêm giờ và phút từ TimePerLap
        endDate.setHours(endDate.getHours() + hours);
        endDate.setMinutes(endDate.getMinutes() + minutes);
      }

      return endDate; // Trả về đối tượng Date
    } catch (error) {
      console.error('Error in calculateNewTargetDate:', { endTime, endDay, timePerLap, error });
      return new Date(); // Trả về giá trị mặc định trong trường hợp lỗi
    }
  };
  const targetDate = calculateNewTargetDate(
    detailAuction?.endTime,
    detailAuction?.endDay,
    detailAuction?.timePerLap
  );
  const calculateFinalTime = (endTime: string, endDay: string): Date => {
    const [endHours, endMinutes] = endTime.split(':').map(Number); // Tách giờ và phút từ endTime

    // Chuyển đổi định dạng dd/MM/yyyy thành yyyy-MM-dd
    const [day, month, year] = endDay.split('/');
    const isoDate = `${year}-${month}-${day}`; // Định dạng yyyy-MM-dd

    const endDate = new Date(isoDate); // Tạo đối tượng Date từ định dạng ISO

    endDate.setHours(endHours, endMinutes, 0, 0); // Gán giờ và phút từ endTime

    return endDate; // Trả về đối tượng Date đã được tính toán
  };
  const isEndTimePassed = (endTime: string = '', endDay: string = ''): boolean => {
    const finalTime = calculateFinalTime(endTime, endDay);

    return finalTime <= new Date(); // Kiểm tra nếu thời gian cuối đã qua
  };
  const calculateNewEndTime = (
    endTime: string | undefined,
    timePerLap: string | undefined
  ): string | null => {
    // Kiểm tra nếu endTime hoặc timePerLap không hợp lệ
    if (!endTime || !timePerLap) {
      console.error('endTime hoặc timePerLap không hợp lệ');
      return null; // Hoặc giá trị mặc định phù hợp
    }

    // Tách giờ và phút từ endTime
    const [endHours, endMinutes] = endTime.split(':').map(Number);
    // Tách giờ và phút từ timePerLap
    const [lapHours, lapMinutes] = timePerLap.split(':').map(Number);

    // Tạo đối tượng Date với giờ và phút từ endTime
    const endDate = new Date();
    endDate.setHours(endHours);
    endDate.setMinutes(endMinutes);

    // Cộng thêm giờ và phút từ timePerLap
    endDate.setHours(endDate.getHours() + lapHours);
    endDate.setMinutes(endDate.getMinutes() + lapMinutes);

    // Lấy giờ và phút sau khi cộng thêm
    const newHours = endDate.getHours().toString().padStart(2, '0');
    const newMinutes = endDate.getMinutes().toString().padStart(2, '0');

    // Trả về chuỗi thời gian mới
    return `${newHours}:${newMinutes}`;
  };
  const auctionInfo = [
    {
      label: 'Chủ thầu',
      value: (
        <div onClick={() => handleToInfor(detailAuction.user.accountId)}>
          {detailAuction.user.fullName}
        </div>
      ),
    },
    {
      label: 'Người trúng thầu',
      value:
        detailAuction.winBidder == null ? (
          'Chưa có người trúng thầu'
        ) : (
          <div
            className="cursor-pointer"
            onClick={() => handleToInfor(detailAuction.winBidder.accountId)}
          >
            {detailAuction.winBidder.nameUser}
          </div>
        ),
    },
    {
      label: 'Quản lý',
      value: `${detailAuction.manager}`,
    },
    {
      label: 'Giá khởi điểm',
      value: `${detailAuction?.startingPrice
        .toLocaleString('vi-VN', {
          style: 'currency',
          currency: 'VND',
        })
        .replace('₫', 'VNĐ')}`,
    },
    {
      label: 'Bước giá',
      value: `${
        detailAuction.priceStep ??
        (0)
          .toLocaleString('vi-VN', {
            style: 'currency',
            currency: 'VND',
          })
          .replace('₫', 'VNĐ')
      }`,
    },
    {
      label: 'Tiền đặt trước',
      value: `${detailAuction.moneyDeposit
        .toLocaleString('vi-VN', {
          style: 'currency',
          currency: 'VND',
        })
        .replace('₫', 'VNĐ')}`,
    },
    {
      label: 'Thời gian đăng kí tham gia',
      value: `Từ ${detailAuction.startTime} ${detailAuction.startDay} đến ${detailAuction.endTime} ${detailAuction.endDay}`,
    },
    {
      label: 'Thời gian bắt đầu đấu giá',
      value: `Từ ${detailAuction.endTime} ${detailAuction.endDay} đến ${calculateNewEndTime(detailAuction.endTime, detailAuction.timePerLap)} ${detailAuction.endDay}`,
    },
    { label: 'Hình thức đấu giá trực tuyến', value: 'Trả giá không xác định vòng' },
    { label: 'Phương thức trả giá', value: detailAuction.paymentMethod },
  ];

  return (
    <Box className="relative h-[150vh] mt-16">
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
            <div className="container flex flex-col gap-2 h-full">
              <div className="flex gap-1">
                <div className="font-bold line-clamp-2">{detailAuction?.nameAuction}</div>
                <div
                  className={`${
                    !isEndTimePassed(detailAuction.endTime, detailAuction.endDay)
                      ? 'bg-green-500'
                      : targetDate > new Date()
                        ? 'bg-orange-500'
                        : 'bg-yellow-500'
                  } bg-opacity-90 p-2 rounded-full w-60`}
                >
                  <CountdownTimer targetDate={targetDate} />
                </div>
              </div>

              <div className="h-[2px] w-full bg-gray-200"></div>
              {swith ? (
                <>
                  <div>
                    <div className="mb-5 flex gap-3 size-5 w-full font-bold">
                      <AutoGraphIcon />
                      Diễn biến cuộc đấu giá
                    </div>
                    <div className="flex flex-col border border-gray-100 p-4 rounded-lg max-h-80 overflow-y-scroll bg-lightGray">
                      <div className="flex flex-col gap-2">
                        {bidHistory?.map((bid: any, index) => (
                          <>
                            <div
                              className={`flex justify-between w-full items-center mb-2 mt-2 ${
                                index % 2 === 0 ? 'text-green-600' : 'text-blue-600'
                              }`}
                            >
                              <div>
                                <p>{new Intl.NumberFormat('vi-VN').format(bid.Price)} VND</p>
                                <span>{bid.DateAndTime}</span>
                              </div>
                              <div onClick={() => handleToInfor(bid.userId)}>
                                <SearchIcon />
                              </div>
                            </div>
                            <div className="h-[2px] w-full bg-gray-200"></div>
                          </>
                        ))}
                      </div>
                    </div>
                    <button className="bg-green-600" onClick={() => setSwith(false)}>
                      Back
                    </button>
                  </div>
                </>
              ) : (
                <>
                  <div className="bg-gray-100 w-full h-full p-8 flex flex-col gap-1">
                    {auctionInfo.map((item, index) => (
                      <InfoRow key={index} label={item.label} value={item.value} />
                    ))}
                    <Box className="pt-3 flex justify-end h-14 mr-24">
                      <button className="bg-green-500 text-white px-2 py-1 rounded mr-2">
                        ReUp
                      </button>

                      {detailAuction.statusAuction == 'Approved' ? (
                        <>
                          <button
                            onClick={(e) => {
                              handleUser();
                            }}
                            className="bg-green-500 text-white px-2 py-1 rounded mr-2"
                          >
                            Xem người đăng ký
                          </button>
                        </>
                      ) : (
                        <>
                          <button
                            onClick={(e) => {
                              handleApprove();
                            }}
                            className="bg-green-500 text-white px-2 py-1 rounded mr-2"
                          >
                            Duyệt
                          </button>
                        </>
                      )}
                      <button className="bg-green-500 text-white px-2 py-1 rounded mr-2">
                        Tải file thông tin
                      </button>
                      <button className="bg-blue-600" onClick={() => setSwith(true)}>
                        Join Room
                      </button>
                      <button
                        onClick={(e) => {
                          handleReject();
                        }}
                        className="bg-red-500 text-white px-2 py-1 rounded"
                      >
                        Từ chối
                      </button>
                    </Box>
                  </div>
                </>
              )}
            </div>
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
                    className="absolute ml-[9%] h-[28%] pt-3"
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
                    className="absolute ml-[18%] h-[28%] pt-3"
                  />
                </Box>
              </Grid>
            </Grid>
          </Box>
        </Box>
      </Box>
      <ApproveModal
        open={isApproveModalOpen}
        onClose={handleModalClose}
        setPrice={setPrice}
        onConfirm={handleModalApprove} // Ensure this is correct
        setHours={setHours}
        setMinutes={setMinutes}
      />
      <CancelModal
        open={isApproveModalCancelOpen} // Use the correct state for the cancel modal
        onClose={handleModalCancelClose}
        setPrice={setPrice}
        onConfirm={handleModalReject} // Ensure this is correct
      />
      <UserModal
        open={isUserModalOpen}
        onClose={handleModalUserClose}
        users={listUser} // Pass the list of users
        setPrice={setPrice}
        onConfirm={handleModalUser}
      />
    </Box>
  );
};

export default AuctionDetail;
