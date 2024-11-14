import axiosInstance from '@services/axiosInstance';

//fetch list data 
export const forgetPassword = async (username: string) => {
    try {
        const response = await axiosInstance.get(`api/account/forgot?username=${username}`);
        console.log(response?.data,"response")
        return response.data;

    } catch (error) {
        throw new Error('Failed to fetch list account');
    }
}

export const resetPass = async (data: { usernameOrEmail: string; resetToken: string; newPassword: string }) => {
    try {
      const response = await axiosInstance.put('api/account/resetPass', data);
      console.log(response.data, "Password reset response");
      return response.data;
    } catch (error) {
      console.error('Failed to reset password:', error);
      throw new Error('Failed to reset password');
    }
  };

  export const changPassWork = async (data: { oldpassword: string; newpassword: string }) => {
    try {
      const token = localStorage.getItem("token");
      const response = await axiosInstance.put('api/UserOrAdmin/changepassword', data, {
        headers: {
          Authorization: `Bearer ${token}`, 
        },
      });
      console.log(response.data, "Password change response");
      return response.data;
    } catch (error) {
      console.error('Failed to change password:', error);
      throw new Error('Failed to change password');
    }
  };