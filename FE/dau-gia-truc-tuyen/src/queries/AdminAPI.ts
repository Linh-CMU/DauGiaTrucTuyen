import axiosInstance from '@services/axiosInstance';

//fetch list data 
export const getListAccount = async () => {
    try {
        const token = localStorage.getItem('token');
        const response = await axiosInstance.get('api/Admin/listAccount', {
            headers: {
                Authorization: `Bearer ${token}`,
              },
        });
        console.log(response?.data,"response")
        return response.data;

    } catch (error) {
        throw new Error('Failed to fetch list account');
    }
}
export const inforUser = async (id?: string) => {
    try {
        const token = localStorage.getItem('token');
        const response = await axiosInstance.get(`/api/Admin/userDetail?uid=${id}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        throw new Error('Error fetching user profile'); 
    }
};
// fetch lock user
export const lockUser = async (accountId: string) => {
    if (!accountId) {
        throw new Error('The accountID field is required.');
    }

    try {
        const token = localStorage.getItem('token');

        const response = await axiosInstance.put(`api/Admin/lockaccount?accountID=${accountId}`, {}, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        console.log(response?.data, "response");
        return response.data; 
    } catch (error) {
        throw new Error('Failed to fetch unlock');
    }
}

// fetch unlock user
export const unLockUser = async (accountId: string) => {
    if (!accountId) {
        throw new Error('The accountID field is required.');
    }
    try {
        const token = localStorage.getItem('token');

        const response = await axiosInstance.put(`api/Admin/unlockaccount?accountID=${accountId}`, {}, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        console.log(response?.data, "response");
        return response.data; 
    } catch (error) {
        throw new Error('Failed to fetch unlock');
    }
}

// fetch city
export const getCity =  async () => {
    try {
        const response = await axiosInstance.get('https://provinces.open-api.vn/api/p')
        return response.data;

    } catch (error) {
        throw new Error('Failed to fetch city');
    }
}

// fetch district
export const getDistrict = async () => {
    try {
        const response = await axiosInstance.get('https://provinces.open-api.vn/api/d/');
        return response.data;
    } catch (error) {
        throw new Error('Failed to fetch district');
    }
}

// fetch ward
export const getWard = async () => {
    try {
        const response = await axiosInstance.get('https://provinces.open-api.vn/api/w/');
        return response.data;
    } catch (error) {
        throw new Error('Failed to fetch ward');
    }
}


export const addUserInformation = async (formData: FormData) => {
    try {
        const token = localStorage.getItem('token');    
        const response = await axiosInstance.put('api/UserOrAdmin/addInformation', formData, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        console.log("Response data:", response.data);
        return response.data;
    } catch (error) {
        console.error('Error adding user information:', error);
        throw new Error('Error adding user information');
    }
}

export const UpdateUserInformation = async (formData: FormData) => {
    try {
        const token = localStorage.getItem('token');    
        const response = await axiosInstance.put('api/UserOrAdmin/update-profile', formData, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        console.log("Response data:", response.data);
        return response.data;
    } catch (error) {
        console.error('Error adding user information:', error);
        throw new Error('Error adding user information');
    }
}

export const profileUser = async () => {
    try {
        const token = localStorage.getItem('token');
        const response = await axiosInstance.get('/api/UserOrAdmin/profile', {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        return response.data;
    } catch (error) {
        throw new Error('Error fetching user profile'); 
    }
};

export const createAccount = async (username: string, password: string, email: string, category: string) => {
    try {
    const token = localStorage.getItem('token');
      // Construct the request body as a JSON object
      const data = {
        username, 
        password,
        email,
        category
      };
      const response = await axiosInstance.post('/api/Admin/make-admin', data, {
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