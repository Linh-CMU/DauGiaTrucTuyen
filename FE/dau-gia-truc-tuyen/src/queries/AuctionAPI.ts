import { AuctionItemFormData } from '@components/modal-contract/ContractModal';
import { AuctionDetail, EditAuctionItemFormData } from '@pages/User/EditAuctionPage';
import axiosInstance from '@services/axiosInstance';
import { Console } from 'console';

// Helper function to get the token with error handling
const getToken = () => {
  const token = localStorage.getItem('token');
  if (!token) throw new Error('Authentication token is missing.');
  return token;
};

// Fetch list of auctions based on status
export const getListAuction = async (status: string = '0') => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/auction/listAuctioneerforuser?status=${status}`,
      {
        headers: {
          'Content-Type': 'application/json', // Correct Content-Type for JSON requests
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction list', error);
    throw error;
  }
};
// Fetch list of auctions of contractor based on status
export const getListAuctionOfContractor = async (status: string = '0') => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/auctionregistrationlist?status=${status}`,{
      headers: {
        'Content-Type': 'application/json', // Correct Content-Type for JSON requests
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction list', error);
    throw error;
  }
};

// Place a bid
export const postBidMoney = async (auctionId: string, price: number) => {
  try {
    const token = getToken();
    // Construct the request body as a JSON object
    const data = {
      auctionId, 
      price,
    };
    const response = await axiosInstance.post('/api/placeBid', data, {
      headers: {
        'Content-Type': 'application/json', // Correct Content-Type for JSON requests
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error placing bid:', error);
    throw error;
  }
};

// Fetch auction details
export const getDetailAuction = async (id: string = '0') => {
  try {
    const response = await axiosInstance.get(`api/auction/auctionDetailforuser?id=${id}`);
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction details', error);
    throw error;
  }
};

// Fetch auction details
export const getDetailAuctionUser = async (id: string = '0') => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/auctionregistrationdetail?id=${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction details', error);
    throw error;
  }
};
//Join auction room
export const getAuctionRoomDetail = async (id: number = 0) => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/joinRoomAuction?id=${id}`, {
       headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction details', error);
    throw error;
  }
};
// Admin: Fetch list of auctions based on status
export const getListAuctionAdmin = async (status: number) => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/Admin/ListAuctionAdmin?status=${status}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch admin auction list');
  }
};

// Fetch auctions for a specific user
export const getListAuctionOfUser = async (id: string, status: number) => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(
      `api/Admin/listAuctioneerByUser?iduser=${id}&status=${status}`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch user auction list');
  }
};

// Admin: Fetch list of users in an auction
export const getListUserAdmin = async (id: number | null) => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/Admin/listBidderInAuction?auctionId=${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch user list in auction');
  }
};

// Admin: Fetch auction details
export const getDetailAuctionAdmin = async (id: number) => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(`api/Admin/auctiondetail?id=${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    });
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction details for admin');
  }
};

// Fetch auction categories
export const getCategory = async () => {
  try {
    const response = await axiosInstance.get(`api/Category/listCategory`);
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction categories');
  }
};

// Admin: Fetch list of auctions by category and status
export const getCategoryId = async (id: number, status: number) => {
  try {
    const token = getToken();
    const response = await axiosInstance.get(
      `api/Admin/ListAuctionCategoryAdmin?status=${status}&category=${id}`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch category-based auction list');
  }
};
export const createPaymentDeposit = async (id: number) => {
  try {
    const token = localStorage.getItem("token");

    // Kiểm tra token có tồn tại không
    if (!token) {
      throw new Error("Token not found in localStorage.");
    }

    const response = await axiosInstance.post(
      `api/Payment/createPaymentDeposit?auctionId=${id}`,
      {}, // Phần data trống nếu không cần gửi dữ liệu trong body
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );
    return response.data;
  } catch (error) {
    throw new Error("Failed to create payment deposit.");
  }
};

export const UpdatePayment = async (id: number, status: string) => {
  try {
    const token = localStorage.getItem("token");

    // Kiểm tra token có tồn tại không
    if (!token) {
      throw new Error("Token not found in localStorage.");
    }

    const response = await axiosInstance.put(
      `api/Payment/update-payment/${id}`,
      { status }, // Truyền status vào body
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json'
        }
      }
    );
    
    return response.data;
  } catch (error) {
    throw new Error("Failed to create payment deposit.");
  }
};

export const registerForAuction = async (id: number) => {
  try {
    const token = localStorage.getItem("token");

    // Kiểm tra xem token có tồn tại không
    if (!token) {
      throw new Error("Token not found in localStorage.");
    }

    const response = await axiosInstance.post(
      `api/registerforauction?aid=${id}`,
      {}, // Phần data trống nếu không cần gửi dữ liệu trong body
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    throw new Error("Failed to register for auction.");
  }
};

export const approveAuction = async (autioneerID: number, status: boolean, priceStep: number | null) => {
  try {
    const token = getToken();
    const response = await axiosInstance.put(
      `api/Admin/ApproveorRejectAuction`,
      { autioneerID, status, priceStep },
      {
        headers: {
          Authorization: `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
      }
    );

    if (response.data.isSucceed) {
      return { isSucceed: true, message: response.data.message };
    } else {
      throw new Error(response.data.message || 'Failed to approve/reject auction');
    }
  } catch (error) {
    console.error('Error approving/rejecting auction:', error);
    throw error;
  }
};

// Submit auction form data
export const submitAuctionForm = async (data: AuctionItemFormData) => {
  try {
    const token = getToken();
    const formData = new FormData();
    formData.append('nameAuction', data.nameAuction);
    formData.append('description', data.description);
    formData.append('startingPrice', data.startingPrice.toString());
    formData.append('categoryID', data.categoryID);

    if (data.imageAuction) formData.append('imageAuction', data.imageAuction);
    if (data.imageVerification) formData.append('imageVerification', data.imageVerification);
    if (data.signatureImg) formData.append('signatureImg', data.signatureImg);

    const response = await axiosInstance.post('/api/addAuctionItem', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  } catch (error) {
    console.error('Error submitting auction form:', error);
    throw error;
  }
};
export const submitEditAuctionForm = async (data: AuctionDetail) => {
  try {
    const token = getToken();
    const formData = new FormData();

    formData.append('auctionID', data.listAuctionID.toString());
    formData.append('nameAuctionItem', data.nameAuction.toString());
    formData.append('description', data.description.toString());
    formData.append('startingPrice', data.startingPrice.toString());
    formData.append('category', data.categoryId.toString());
    if (data.image instanceof File) {
      formData.append('imageAuction', data.image);
    }
    if (data.imageEvidence instanceof File) {
      formData.append('imageEvidence', data.imageEvidence);
    }
    console.log('formData', formData);
    
    const response = await axiosInstance.put('/api/UpdateAuctionItem', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
    
  } catch (error) {
    console.error('Error submitting auction form:', error);
    throw error;
  }
};
