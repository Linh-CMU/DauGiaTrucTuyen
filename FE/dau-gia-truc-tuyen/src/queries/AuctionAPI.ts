
import axiosInstance from '@services/axiosInstance';

// Fetch list of auctions with status as a parameter (default to 0)
export const getListAuction = async (status: string = "0") => {
  try {
    // Update the URL to include the status query parameter
    const response = await axiosInstance.get(`api/auction/listAuctioneerforuser?status=${status}`);
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction list', error);
    throw new Error('Failed to fetch auction list');
  }
};

export const getDetailAuction = async (id: string = "0") => {
  try {
    // Update the URL to include the status query parameter
    const response = await axiosInstance.get(`api/auction/auctionDetailforuser?id=${id}`);
    console.log(response?.data, "response");
    return response.data;
  } catch (error) {
    console.error('Failed to fetch auction list', error);
    throw new Error('Failed to fetch auction list');
  }
};