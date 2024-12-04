import { useEffect, useState } from 'react';
import {
  getListAuctionAdmin,
  getCategory,
  approveAuction,
  getCategoryId,
  getListUserAdmin,
  getListAuctionOfUser,
  getListAuctionRegisterOfUser,
} from '../../queries/index';
import { ApproveModal, CancelModal, UserModal } from '../modalAccept/ApproveModal'; // Import modal
import VisibilityIcon from '@mui/icons-material/Visibility';
import { Box, Button, Modal, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';

const TableAuction1 = ({ tabValue, id, name, status }: { tabValue: number; id?: string; name?: string; status?: boolean }) => {
  const [listAllAuction, setListAllAuction] = useState<any[]>([]);
  const [listCategory, setCategory] = useState<any[]>([]);
  const [listUser, setUser] = useState<any[]>([]);
  const [isApproveModalOpen, setApproveModalOpen] = useState(false); // Modal state
  const [isUserModalOpen, setUserModalOpen] = useState(false); // Modal state
  const [selectedAuctionID, setSelectedAuctionID] = useState<number | null>(null); // Auction ID state
  const [isApproveModalCancelOpen, setApproveModalCancelOpen] = useState(false); // Modal cancel state
  const [price, setPrice] = useState<number | null>(null);
  const [time, setTime] = useState('');
  const [hours, setHours] = useState<number | ''>('');
  const [minutes, setMinutes] = useState<number | ''>('');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    
    fetchListAuction();
    fetchListCategory();
    fetchListUser();
  }, [tabValue]);

  const fetchListAuction = async () => {
    if (id) {
      console.log('status', status);
      if(status) {
        const response = await getListAuctionOfUser(id, tabValue);
        console.log(response, 'data');
        if (response?.isSucceed) {
          setListAllAuction(response?.result);
        } else {
          console.error('fetch list fail');
        }
      }else{
        const response = await getListAuctionRegisterOfUser(id, tabValue);
        console.log(response, 'data');
        if (response?.isSucceed) {
          setListAllAuction(response?.result);
        } else {
          console.error('fetch list fail');
        }
      }
    } else {
      const response = await getListAuctionAdmin(tabValue);
      console.log(response, 'data');
      if (response?.isSucceed) {
        setListAllAuction(response?.result);
      } else {
        console.error('fetch list fail');
      }
    }
  };
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

  const fetchListCategory = async () => {
    const response = await getCategory();
    console.log(response, 'data');
    if (response?.isSucceed) {
      setCategory(response?.result);
    } else {
      console.error('fetch list fail');
    }
  };

  const handleApprove = (id: number) => {
    setSelectedAuctionID(id); // Save auction ID
    setApproveModalOpen(true); // Open approval modal
  };
  const handleUser = (id: number) => {
    setSelectedAuctionID(id); // Save auction ID
    setUserModalOpen(true); // Open approval modal
  };

  const handleReject = (id: number) => {
    setSelectedAuctionID(id); // Save auction ID
    setApproveModalCancelOpen(true); // Open cancel modal
  };

  const handleModalApprove = async () => {
    if (selectedAuctionID) {
      const formattedHours = (hours || 0).toString().padStart(2, '0');
      const formattedMinutes = (minutes || 0).toString().padStart(2, '0');
      const totalTime = `${formattedHours}:${formattedMinutes}`;
      
      const response = await approveAuction(selectedAuctionID, true, price, totalTime);
      if (response.isSucceed) {
        fetchListAuction();
        alert('Bạn đã phê duyệt thành công');
      }
    }
    setApproveModalOpen(false);
  };
  const handleModalUser = async () => {
    setUserModalOpen(false);
  };
  const handleModalReject = async () => {
    if (selectedAuctionID) {
      const response = await approveAuction(selectedAuctionID, false, price, time);
      if (response.isSucceed) {
        fetchListAuction();
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
  const navigate = useNavigate();
  const handleCategoryClick = async (category: number) => {
    const response = await getCategoryId(category, tabValue);
    console.log(response, 'data');
    if (response?.isSucceed) {
      setListAllAuction(response?.result);
    } else {
      console.error('fetch list fail');
    }
  };
  const handleTimeChange = () => {
    const formattedHours = (hours || 0).toString().padStart(2, '0');
    const formattedMinutes = (minutes || 0).toString().padStart(2, '0');
    const totalTime = `${formattedHours}:${formattedMinutes}`;
    if (setTime) {
      setTime(totalTime);
    }
  };
  const style = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
  };


  const headings = [
    { key: 'daugia', value: 'Tên buổi đấu giá' },
    { key: 'hinhanh', value: 'Hình ảnh' },
    { key: 'start', value: 'Giá bắt đầu' },
    { key: 'money', value: 'Tiền cọc' },
    { key: 'category', value: 'Loại đấu giá' },
    { key: 'status', value: 'Trạng thái	' },
    { key: 'action', value: 'Hành động	' },
  ];

  const [currentPage, setCurrenPage] = useState(1);
  const pageSize = 4;

  const filterUser = listAllAuction.filter((account) => 
    account.userName.toLowerCase().includes(searchTerm.toLocaleLowerCase())
  );

  // Pagination logic
  const totalPages = Math.ceil(filterUser.length / pageSize);
  const currentList = filterUser.slice(
    (currentPage - 1) * pageSize,
    currentPage * pageSize
  );

  const handlePrev = () => {
    if (currentPage > 1) {
      setCurrenPage((prev) => prev - 1);
    }
  };

  const handleNext = () => {
    if (currentPage < totalPages) {
      setCurrenPage((prev) => prev + 1);
    }
  };


  const renderPaginationBtn = () => {
    const btns = [];
    const startPage = Math.max(1, currentPage - 1);
    const endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
      btns.push(
        <button
          key={i}
          type="button"
          className={`min-h-[38px] min-w-[38px] flex justify-center items-center border py-2 px-3 text-sm rounded-lg focus:outline-none ${
            i === currentPage
              ? 'border-blue-600 text-blue-600 bg-blue-100'
              : 'border-gray-200 text-gray-800 hover:bg-gray-100'
          }`}
          onClick={() => setCurrenPage(i)}
        >
          {i}
        </button>
      );
    }

    return btns;
  };

  return (
    <>
      {name ? (
        <>
          <div className="ml-[30%] mr-[30%] mb-10 p-4 bg-gray-100 rounded-lg shadow-md text-center">
            <h2 className="text-lg font-semibold text-gray-800"> {status ? 'Chủ sản phẩm:' : 'Sản phẩm tham gia:'} {name}</h2>
          </div>
        </>
      ) : (
        ''
      )}

      <div className="flex justify-between items-start">
        <div
          className={`w-full max-w-7xl ${
            listAllAuction.length > 0
              ? 'h-[500px] overflow-y-scroll scrollbar-thin scrollbar-thumb-gray-400 scrollbar-track-gray-200'
              : ''
          }`}
        >
           <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
            <thead className="text-xs text-gray-700 uppercase dark:text-gray-400">
              <tr>
                {headings.map((heading) => (
                  <th
                    key={heading.key}
                    scope="col"
                    className={`px-6 py-3 bg-gray-50 dark:bg-gray-800 ${heading.key === 'action' ? 'text-center' : ''}`}
                  >
                    {heading.value}
                  </th>
                ))}
              </tr>
            </thead>
            <tbody>
              {currentList.length > 0 ? (
                currentList.map((auction, index) => (
                  <tr
                    key={index}
                    className={`border-b border-gray-200 dark:border-gray-700 ${index % 2 === 0 ? 'bg-white' : 'bg-gray-100'}`}
                    onClick={() => navigate(`/auctionDetail/${auction.listAuctionID}`)}
                  >
                    <td className="border-dashed border-t border-gray-200 w-1/4 px-6 py-3">{auction.nameAuction}</td>
                    <td className="border-dashed border-t border-gray-200 w-1/4 px-6 py-3">
                      <img
                        src={
                          `http://capstoneauctioneer.runasp.net/api/read?filePath=` + auction.image
                        }
                        alt={auction.nameAuction}
                        className="w-16 h-16 object-cover mx-auto"
                      />
                    </td>
                    <td className="px-4 py-2 text-center">
                      {auction.startingPrice.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      })}
                    </td>
                    <td className="px-4 py-2 text-center">
                      {auction.startingPrice.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      })}
                    </td>
                    <td className="px-4 py-2 text-center">{auction.category}</td>
                    <td className="px-4 py-2 text-center">{auction.statusAuction}</td>
                    <td className="px-4 py-2 text-center">
                      {auction.statusAuction == 'Approved' ? (
                        <>
                          <Button
                            onClick={(e) => {
                              e.stopPropagation(); // Ngăn chặn sự kiện click lan lên thẻ <tr>
                              handleUser(auction.listAuctionID);
                            }}
                            className="bg-green-500 text-white px-2 py-1 rounded mr-2"
                            color="success"
                          >
                            <VisibilityIcon />
                          </Button>
                        </>
                      ) : (
                        <>
                          <button
                            onClick={(e) => {
                              e.stopPropagation(); // Ngăn chặn sự kiện click lan lên thẻ <tr>
                              handleApprove(auction.listAuctionID);
                            }}
                            className="bg-green-500 text-white px-2 py-1 rounded mr-2"
                          >
                            Duyệt
                          </button>
                        </>
                      )}

                      <button
                        onClick={(e) => {
                          e.stopPropagation(); // Ngăn chặn sự kiện click lan lên thẻ <tr>
                          handleReject(auction.listAuctionID);
                        }}
                        className="bg-red-500 text-white px-2 py-1 rounded"
                      >
                        Từ chối
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={6} className="text-center py-4">
                    Không có buổi đấu giá nào
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
        <div className="flex flex-col items-start justify-center ml-4">
          <div className="font-semibold text-xl mb-2">Danh mục:</div>
          <div className="flex flex-col bg-gray-100 p-4 rounded-lg shadow-md w-64">
            {listCategory.map((category, index) => (
              <span
                key={index}
                className="text-lg text-gray-700 mb-2 cursor-pointer hover:bg-blue-500 hover:text-white px-2 py-1 rounded transition duration-200"
                onClick={() => handleCategoryClick(category.categoryID)}
              >
                {category.nameCategory}
              </span>
            ))}
          </div>
        </div>
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
      </div>
    </>
  );
};

export default TableAuction1;
