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

        const fullName = formData.get('fullName') as string;
        const phone = formData.get('phone') as string;
        const city = formData.get('city') as string;
        const ward = formData.get('ward') as string;
        const district = formData.get('district') as string;
        const address = formData.get('address') as string;
        console.log(formData.get('frontCCCD'));
        console.log(formData.get('avatar'));
        const response = await axiosInstance.put(`api/UserOrAdmin/addInformation?fullName=${fullName}&phone=${phone}&city=${city}&ward=${ward}&district=${district}&address=${address}`, formData, {
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