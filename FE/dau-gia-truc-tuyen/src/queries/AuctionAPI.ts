
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
