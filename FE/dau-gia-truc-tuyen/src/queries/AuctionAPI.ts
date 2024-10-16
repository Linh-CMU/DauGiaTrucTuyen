
import axiosInstance from '@services/axiosInstance';

// Fetch user profile data
export const getListAuction = async () => {
  try {
    const response = await axiosInstance.get('api/auction/listAuctioneerforuser');
    console.log(response?.data,"response")
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch user profile');
  }
};
export const getListAuctionAdmin = async (status: number) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.get(`api/Admin/ListAuctionAdmin?status=${status}`, {
      headers: {
        Authorization: `Bearer ${token}`, // Truyền token vào đây
      },
    });
    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction list');
  }
};
export const getListUserAdmin = async (id: number | null) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.get(`api/Admin/listBidderInAuction?auctionId=${id}`, {
      headers: {
        Authorization: `Bearer ${token}`, // Truyền token vào đây
      },
    });
    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction list');
  }
};
export const getDetailAuctionAdmin = async (id: number) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.get(`api/Admin/auctiondetail?id=${id}`, {
      headers: {
        Authorization: `Bearer ${token}`, // Truyền token vào đây
      },
    });
    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction list');
  }
};
export const getCategory = async () => {
  try {
    const response = await axiosInstance.get(`api/Category/listCategory`);
    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction list');
  }
};
export const getCategoryId = async (id : number , status : number) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.get(`api/Admin/ListAuctionCategoryAdmin?status=${status}&category=${id}`, {
      headers: {
        Authorization: `Bearer ${token}`, // Truyền token vào đây
      },
    });
    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    throw new Error('Failed to fetch auction list');
  }
};
export const approveAuction = async (auctionID: number, price: number | null, status: boolean) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.put(`api/Admin/ApproveorRejectAuction`, 
      {
        auctionID: auctionID,
        status: status,
        priceStep: price 
      }, 
      {
        headers: {
          Authorization: `Bearer ${token}`, 
          'Content-Type': 'application/json' 
        },
      }
    );
    if (response.data.isSucceed) {
      return { isSucceed: true, message: response.data.message }; 
    } else {
      throw new Error(response.data.message); 
    }
  } catch (error) {
    console.error('Error approving auction:', error);
    throw new Error('Failed to approve auction'); 
  }
};
