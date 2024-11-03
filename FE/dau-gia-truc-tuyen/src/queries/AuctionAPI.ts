
import { AuctionItemFormData } from '@components/auction-item-form/AuctionItemForm';
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
export const getListAuctionOfUser = async (id: string, status: number) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.get(`api/Admin/listAuctioneerByUser?iduser=${id}&status=${status}`, {
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
export const approveAuction = async (autioneerID: number, status: boolean, priceStep: number | null) => {
  try {
    const token = localStorage.getItem("token");
    const response = await axiosInstance.put(
      `api/Admin/ApproveorRejectAuction`,
      {
        autioneerID: autioneerID, // Corrected the property name
        status: status,
        priceStep: priceStep, // Updated to match the provided JSON
      },
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
      throw new Error(response.data.message);
    }
  } catch (error) {
    console.error('Error approving auction:', error);
    throw new Error('Failed to approve auction');
  }
};

export const submitAuctionForm = async (data: AuctionItemFormData) => {
  try {
    const token = localStorage.getItem("token");
    const formData = new FormData();
    formData.append('nameAuction', data.nameAuction);
    formData.append('description', data.description);
    formData.append('startingPrice', data.startingPrice.toString());
    formData.append('categoryID', data.categoryID);

    if (data.imageAuction) {
      formData.append('imageAuction', data.imageAuction);
    }

    if (data.imageVerification) {
      formData.append('imageVerification', data.imageVerification);
    }

    if (data.signatureImg) {
      formData.append('signatureImg', data.signatureImg);
    }
    console.log('signatureImg', data.signatureImg);
    
    const response = await axiosInstance.post('/api/addAuctionItem', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        Authorization: `Bearer ${token}`,
      },
    });

    console.log('Auction item created successfully:', response.data);
    return response.data;
  } catch (error) {
    console.error('Error creating auction item:', error);
    throw new Error('Failed to create auction item');
  }
};

