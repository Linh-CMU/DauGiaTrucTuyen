import { useEffect, useState } from 'react';
import {
  getListAuctionAdmin,
  getCategory,
  approveAuction,
  getCategoryId,
  getListUserAdmin,
  getListAuctionOfUser,
  getListAuctionRegisterOfUser,
  profileUser,
} from '../../queries/index';
import { ApproveModal, CancelModal, UserModal } from '../../components/modalAccept/ApproveModal'; // Import modal
import VisibilityIcon from '@mui/icons-material/Visibility';
import { Box, Button, Modal, Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useMessage } from '@contexts/MessageContext';
import { profileResponse } from '../../types/auth.type';

const TableAuction = ({
  tabValue,
  id,
  name,
  status,
}: {
  tabValue: number;
  id?: string;
  name?: string;
  status?: boolean;
}) => {
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
  const [files, setFiles] = useState<File | null>(null);
  const [minutes, setMinutes] = useState<number | ''>('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [profile, setProfile] = useState<profileResponse | null>();

  useEffect(() => {
    fetchListAuction();
    fetchListCategory();
  }, [tabValue]);
  const { setSuccessMessage, setErrorMessage } = useMessage();
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await profileUser();
        console.log(response.result);
        setProfile(response.result);
      } catch (error) {}
    };
    fetchData();
  }, []);
  const fetchListAuction = async () => {
    if (id) {
      if (status) {
        const response = await getListAuctionOfUser(id, tabValue);
        console.log(response, 'data');
        if (response?.isSucceed) {
          setListAllAuction(response?.result);
          console.log(response?.result);
        } else {
          setErrorMessage('fetch list fail');
        }
      } else {
        const response = await getListAuctionRegisterOfUser(id, tabValue);
        if (response?.isSucceed) {
          setListAllAuction(response?.result);
        } else {
          setErrorMessage('fetch list fail');
        }
      }
    } else {
      const response = await getListAuctionAdmin(tabValue);
      if (response?.isSucceed) {
        setListAllAuction(response?.result);
      } else {
        setErrorMessage('fetch list fail');
      }
    }
  };

  const fetchListCategory = async () => {
    const response = await getCategory();
    if (response?.isSucceed) {
      setCategory(response?.result);
    } else {
      setErrorMessage('fetch list fail');
    }
  };

  const handleApprove = (id: number) => {
    setSelectedAuctionID(id); // Save auction ID
    setApproveModalOpen(true); // Open approval modal
  };
  const handleUser = async(id: number) => {
    const response = await getListUserAdmin(id);
    console.log(response, 'data');
    if (response?.isSucceed) {
      setUser(response?.result);
    } else {
      setErrorMessage('fetch list fail');
    }
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
      if (files === null) {
        setErrorMessage('please upload the file');
        return;
      }
      const response = await approveAuction(selectedAuctionID, true, totalTime, files);
      if (response.isSucceed) {
        fetchListAuction();
        setSuccessMessage('You have approved successfully.');
      }
    }
    setApproveModalOpen(false);
  };
  const handleModalUser = async () => {
    setUserModalOpen(false);
  };
  const handleModalReject = async () => {
    if (selectedAuctionID) {
      const response = await approveAuction(selectedAuctionID, false, time, files);
      if (response.isSucceed) {
        fetchListAuction();
        setSuccessMessage('You have declined this auction order.');
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

  const headings = [
    { key: 'daugia', value: 'Auction name' },
    { key: 'hinhanh', value: 'Image' },
    { key: 'start', value: 'Starting price' },
    { key: 'money', value: 'Deposit' },
    { key: 'category', value: 'Auction type' },
    { key: 'status', value: 'Status ' },
    { key: 'action', value: 'Action ' },
  ];

  const [searchTerm, setSearchTerm] = useState('');
  const [currentPage, setCurrenPage] = useState(1);
  const pageSize = 4;

  const filteredAuctions = listAllAuction.filter((account) => {
    const matchesSearch = account.nameAuction.toLowerCase().includes(searchTerm.toLowerCase());
    const matchesCategory = !selectedCategory || account.category === selectedCategory;
    return matchesSearch && matchesCategory;
  });

  // Pagination logic
  const totalPages = Math.ceil(filteredAuctions.length / pageSize);
  const currentList = filteredAuctions.slice((currentPage - 1) * pageSize, currentPage * pageSize);

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
              : 'bg-slate-300 text-gray-800 hover:bg-slate-300'
          }`}
          onClick={() => setCurrenPage(i)}
        >
          {i}
        </button>
      );
    }

    return btns;
  };

  const handleCategoryChange = (selectedCategoryName: any) => {
    console.log('Danh mục được chọn:', selectedCategoryName);
    setSelectedCategory(selectedCategoryName);
  };
  return (
    <>
      {name ? (
        <>
          <div className="ml-[30%] mr-[30%] mb-10 p-4 bg-gray-100 rounded-lg shadow-md text-center">
            <h2 className="text-lg font-semibold text-gray-800">
              {' '}
              {status ? 'Product Owner:' : 'Participating products:'} {name}
            </h2>
          </div>
        </>
      ) : (
        ''
      )}

      <div className="mb-4 flex justify-between items-center pb-5 ">
        <div className="flex-1 pr-4">
          <div className="relative md:w-1/6">
            <input
              type="search"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="bg-slate-200 w-full pl-10 pr-4 py-2 rounded-lg shadow focus:outline-none focus:shadow-outline text-gray-600 font-medium"
              placeholder="Search..."
            />
            <div className="absolute top-0 left-0 inline-flex items-center p-2">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                className="w-6 h-6 text-gray-400"
                viewBox="0 0 24 24"
                strokeWidth="2"
                stroke="currentColor"
                fill="none"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <circle cx="10" cy="10" r="7" />
                <line x1="21" y1="21" x2="15" y2="15" />
              </svg>
            </div>
          </div>
        </div>
        {profile && !profile?.categoryId && (
          <select
            id="countries"
            onChange={(event) => handleCategoryChange(event.target.value)}
            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-48 p-2.5"
          >
            <option value="">Category</option>
            {listCategory.map((category, index) => (
              <option
                value={category.nameCategory}
                key={index}
                className="text-lg text-gray-700 mb-2 cursor-pointer hover:bg-blue-500 hover:text-white px-2 py-1 rounded transition duration-200"
              >
                {category.nameCategory}
              </option>
            ))}
          </select>
        )}
      </div>

      <div className="w-full">
        <div>
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
                    <td className="px-4 py-2 text-left w-96">{auction.nameAuction}</td>
                    <td className="px-4 py-2 text-center">
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
                      {auction.priceDeposit.toLocaleString('vi-VN', {
                        style: 'currency',
                        currency: 'VND',
                      })}
                    </td>
                    <td className="px-4 py-2 text-center">{auction.category}</td>
                    <td className="px-4 py-2 text-center">{auction.statusAuction}</td>
                    <td className="px-4 py-2 text-center flex items-center space-x-2 w-auto">
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
                            className="bg-green-500 text-white px-2 py-1 rounded mr-2 "
                          >
                            Accept
                          </button>
                        </>
                      )}

                      <button
                        onClick={(e) => {
                          e.stopPropagation(); // Ngăn chặn sự kiện click lan lên thẻ <tr>
                          handleReject(auction.listAuctionID);
                        }}
                        className="bg-red-500 text-white px-2 py-1 rounded w-20 h-8 flex items-center justify-center"
                      >
                        Refuse
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={6} className="text-center py-4">
                    There are no auctions
                  </td>
                </tr>
              )}
            </tbody>
          </table>
          <div className="flex justify-center mt-4 space-x-2 pt-8">
            <button
              onClick={handlePrev}
              className="px-4 py-2 bg-gray-200 rounded-md text-sm"
              disabled={currentPage === 1}
            >
              <svg
                className="shrink-0 size-3.5"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              >
                <path d="m15 18-6-6 6-6"></path>
              </svg>
            </button>
            {renderPaginationBtn()}
            <button
              onClick={handleNext}
              className="px-4 py-2 bg-gray-200 rounded-md text-sm"
              disabled={currentPage === totalPages}
            >
              <svg
                className="shrink-0 size-3.5"
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                stroke-width="2"
                stroke-linecap="round"
                stroke-linejoin="round"
              >
                <path d="m9 18 6-6-6-6"></path>
              </svg>
            </button>
          </div>
        </div>

        <ApproveModal
          open={isApproveModalOpen}
          onClose={handleModalClose}
          setPrice={setPrice}
          onConfirm={handleModalApprove} // Ensure this is correct
          setHours={setHours}
          setMinutes={setMinutes}
          setFile={setFiles}
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

export default TableAuction;
