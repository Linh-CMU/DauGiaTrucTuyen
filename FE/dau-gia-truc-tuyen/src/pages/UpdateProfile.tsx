import React, { useEffect, useState } from 'react';
import { cityResponse, districtResponse, wardResponse } from '../types/auth.type';
import {
  getCity,
  getDistrict,
  getWard,
  profileUser,
  UpdateUserInformation,
} from '../queries/AdminAPI';
import { useMessage } from '@contexts/MessageContext';
import { useNavigate } from 'react-router-dom';

interface profileResponse {
  fullName: string;
  phone: string;
  city: string;
  ward: string;
  district: string;
  address: string;
  avatar: any;
  frontCCCD: string;
  signature: string;
  backsideCCCD: string;
  gender: boolean;
  birthdate: string;
  placeOfResidence: string;
  placeOfIssue: string;
  dateOfIssue: string;
}

const Updateprofile = () => {
  const [citys, setCitys] = useState<cityResponse[]>([]);
  const [districts, setDistricts] = useState<districtResponse[]>([]);
  const [wards, setWards] = useState<wardResponse[]>([]);
  const [isTouched, setIsTouched] = useState(false);
  const navigate = useNavigate();

  const { setErrorMessage, setSuccessMessage } = useMessage();
  const [profile, setProfile] = useState<profileResponse | null>();
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await profileUser();
        console.log(response.result);
        setProfile(response.result);
      } catch (error) {
        setErrorMessage('Error fetching user');
      }
    };
    fetchData();
  }, []);
  useEffect(() => {
    const fetchCity = async () => {
      try {
        const cityData = await getCity();
        setCitys(cityData);
      } catch (error) {
        setErrorMessage('Error fetching city');
      }
    };
    fetchCity();
  }, []);

  useEffect(() => {
    const fetchDistricts = async () => {
      try {
        const districtData = await getDistrict();
        setDistricts(districtData);
      } catch (error) {
        setErrorMessage('Error fetching districts');
      }
    };
    fetchDistricts();
  }, []);

  useEffect(() => {
    const fetchWard = async () => {
      try {
        const wardData = await getWard();
        setWards(wardData);
      } catch (error) {
        setErrorMessage('Error fetching wards');
      }
    };

    fetchWard();
  }, []);

  const [filteredDistricts, setfilteredDistricts] = useState<any>('');
  const [filteredWards, setfilteredWards] = useState<any>('');

  useEffect(() => {
    if (profile) {
      const filteredDistrict = districts.filter(
        (district) => district.province_code.toString() === profile?.city
      );
      const filteredWard = wards.filter(
        (ward) => ward.district_code.toString() === profile?.district
      );
      setfilteredWards(filteredWard);
      setfilteredDistricts(filteredDistrict);
    }
  }, [profile]);

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const previewURL = URL.createObjectURL(file);
      setAvatar(previewURL);
      setProfile((prev: any) => ({
        ...prev,
        avatar: file,
      }));
    }
  };

  const [avatar, setAvatar] = useState<any>();
  useEffect(() => {
    console.log(avatar);
    if (profile?.avatar && !avatar) {
      setAvatar(`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.avatar}`);
    }
  }, [profile, avatar]);
  const handleRemoveImage = () => {
    setAvatar('');
    setProfile((prev: any) => ({
      ...prev,
      avatar: '',
    }));
  };
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsTouched(true);

    const formData = new FormData();
    formData.append('fullName', profile?.fullName ?? '');
    formData.append('phone', profile?.phone ?? '');
    formData.append('city', profile?.city ?? '');
    formData.append('district', profile?.district ?? '');
    formData.append('ward', profile?.ward ?? '');
    formData.append('address', profile?.address ?? '');
    if (profile?.avatar instanceof File) formData.append('avatar', profile?.avatar);
    try {
      const response = await UpdateUserInformation(formData);
      if (response.isSucceed) {
        navigate('/profile');
        setSuccessMessage('Successfully updated user information!');
      } else {
        setErrorMessage(response.message);
      }
    } catch (error) {
      setErrorMessage('Erro add infomation');
    }
  };

  return (
    <>
      <div className="w-full mt-3 flex items-center justify-center pt-20">
        <h5 className="font-bold text-center text-2xl pl-9">ADD INFORMATION</h5>
      </div>
      <form onSubmit={handleSubmit}>
        <div className="flex justify-center pt-10">
          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    họ và tên
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!profile?.fullName && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    defaultValue={profile?.fullName}
                    onChange={(e) =>
                      setProfile((prev: any) => ({
                        ...prev,
                        fullName: e.target.value,
                      }))
                    }
                    required
                    onFocus={() => setIsTouched(true)}
                  />
                  {!profile?.fullName && isTouched && (
                    <p className="text-red-500 text-xs italic">Please enter your full name.</p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-gender"
                  >
                    Giới tính
                  </label>
                  {profile?.gender ? (
                    <>
                      <input
                        className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                        id="grid-last-name"
                        type="text"
                        value={'Nam'}
                        disabled
                      />
                    </>
                  ) : (
                    <>
                      <input
                        className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                        id="grid-last-name"
                        type="text"
                        value={'Nữ'}
                        disabled
                      />
                    </>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ngày sinh
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="date"
                    placeholder="Chọn ngày cấp"
                    value={profile?.birthdate}
                    disabled
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    City
                  </label>
                  <select
                    id="grid-city"
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={profile?.city}
                    onChange={(e) => {
                      const selectedCode = e.target.value;
                      const selectedCity = citys.find(
                        (cityItem) => cityItem.code.toString() === selectedCode
                      );
                      setProfile((prev: any) => ({
                        ...prev,
                        city: selectedCity?.code.toString(),
                      }));
                    }}
                    onFocus={() => setIsTouched(true)}
                  >
                    <option value="">Select a city</option>
                    {citys.map((cityItem) => (
                      <option key={cityItem.code} value={cityItem.code.toString()}>
                        {cityItem.name}
                      </option>
                    ))}
                  </select>
                  {!profile?.city && isTouched && (
                    <p className="text-red-500 text-xs italic">Please select a city.</p>
                  )}
                </div>
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Districts
                  </label>
                  <select
                    id="grid-city"
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!profile?.city && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={profile?.district}
                    onChange={(e) => {
                      const selectDis = districts.find(
                        (districtResponse) => districtResponse.code.toString() === e.target.value
                      );
                      setProfile((prev: any) => ({
                        ...prev,
                        district: selectDis?.code.toString(),
                      }));
                    }}
                    onFocus={() => setIsTouched(true)}
                  >
                    <option value="">Select a districts</option>
                    {filteredDistricts.length > 0 ? (
                      filteredDistricts.map((districtItem: any) => (
                        <option key={districtItem.code} value={districtItem.code}>
                          {districtItem.name}
                        </option>
                      ))
                    ) : (
                      <option value="">No districts available</option>
                    )}
                  </select>
                  {!profile?.city && isTouched && (
                    <p className="text-red-500 text-xs italic">Please select a district.</p>
                  )}
                </div>
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ward
                  </label>
                  <select
                    id="grid-city"
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!profile?.city && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={profile?.ward}
                    onChange={(e) => {
                      setProfile((prev: any) => ({
                        ...prev,
                        ward: e.target.value,
                      }));
                    }}
                    onFocus={() => setIsTouched(true)}
                  >
                    <option value="">Select a ward</option>
                    {filteredWards.length > 0 ? (
                      filteredWards.map((wardItem: any) => (
                        <option key={wardItem.code} value={wardItem.name}>
                          {wardItem.name}
                        </option>
                      ))
                    ) : (
                      <option value="">No wards available</option>
                    )}
                  </select>
                  {!profile?.city && isTouched && (
                    <p className="text-red-500 text-xs italic">Please select a ward.</p>
                  )}
                </div>
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    địa chỉ nhà
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!profile?.address && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    defaultValue={profile?.address}
                    onChange={(e) =>
                      setProfile((prev: any) => ({
                        ...prev,
                        address: e.target.value,
                      }))
                    }
                    onFocus={() => setIsTouched(true)}
                  />
                </div>
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    số điện thoại
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border ${!/^[0-9]{10}$/.test(profile?.phone ?? '') && isTouched ? 'border-red-500' : 'border-gray-200'} rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter phone"
                    defaultValue={profile?.phone}
                    onChange={(e) =>
                      setProfile((prev: any) => ({
                        ...prev,
                        phone: e.target.value,
                      }))
                    }
                    onFocus={() => setIsTouched(true)}
                    required
                  />
                  {!/^[0-9]{10}$/.test(profile?.phone ?? '') && isTouched && (
                    <p className="text-red-500 text-xs italic">
                      Please enter a valid phone number.
                    </p>
                  )}
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    hộ khẩu thường trú
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Nhập hộ khẩu thường trú"
                    value={profile?.placeOfResidence}
                    disabled
                  />
                </div>
              </div>
            </div>
          </div>
          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ảnh CMND/CCCD
                  </label>
                </div>
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <div className="flex items-center justify-center w-full">
                    <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                      <img
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile?.frontCCCD}`}
                        alt="Front CCCD"
                        className="w-full h-full object-cover rounded-lg"
                      />
                    </div>
                  </div>
                </div>
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <div className="flex items-center justify-center w-full">
                    <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                      <img
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile?.backsideCCCD}`}
                        alt="Back CCCD"
                        className="w-full h-full object-cover rounded-lg"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Nơi cấp:
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    value={profile?.placeOfIssue}
                    readOnly
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ngày Cấp
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="date"
                    placeholder="Chọn ngày cấp"
                    value={profile?.dateOfIssue}
                    readOnly
                  />
                </div>
              </div>
              <div className="flex">
                <div>
                  <div className="flex flex-wrap -mx-3 mb-6">
                    <div className="w-full  px-3 mb-6 md:mb-0">
                      <label
                        className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                        htmlFor="grid-first-name"
                      >
                        Avatar
                      </label>
                    </div>
                  </div>
                  <div>
                    {!avatar ? (
                      <div className="flex items-center justify-center w-64 h-64 border-2 border-gray-300 border-dashed rounded-full cursor-pointer bg-gray-50 dark:hover:bg-gray-800 dark:bg-gray-700 hover:bg-gray-100 dark:border-gray-600 dark:hover:border-gray-500 dark:hover:bg-gray-600">
                        <input
                          type="file"
                          accept="image/*"
                          onChange={handleImageChange}
                          className="hidden"
                          id="upload-avatar"
                        />
                        <label
                          htmlFor="upload-avatar"
                          className="flex flex-col items-center justify-center w-full h-full cursor-pointer"
                        >
                          <svg
                            className="w-8 h-8 mb-4 text-gray-500 dark:text-gray-400"
                            aria-hidden="true"
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 20 16"
                          >
                            <path
                              stroke="currentColor"
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              strokeWidth="2"
                              d="M13 13h3a3 3 0 0 0 0-6h-.025A5.56 5.56 0 0 0 16 6.5 5.5 5.5 0 0 0 5.207 5.021C5.137 5.017 5.071 5 5 5a4 4 0 0 0 0 8h2.167M10 15V6m0 0L8 8m2-2 2 2"
                            />
                          </svg>
                          <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
                            <span className="font-semibold">Upload</span> avatar
                          </p>
                        </label>
                      </div>
                    ) : (
                      <div className="relative mt-4">
                        <img
                          src={avatar}
                          className="rounded-full w-64 h-64 object-cover border-2 border-gray-300 border-dashed"
                          alt="Selected"
                        />
                        <button
                          type="button"
                          className="absolute top-1 right-1 p-1 bg-red-500 text-white rounded-full w-8 h-8 flex items-center justify-center"
                          onClick={handleRemoveImage}
                        >
                          <svg
                            className="w-4 h-4"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                            xmlns="http://www.w3.org/2000/svg"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              strokeWidth="2"
                              d="M6 18L18 6M6 6l12 12"
                            ></path>
                          </svg>
                        </button>
                      </div>
                    )}
                  </div>
                </div>
                <div className="ml-16">
                  <div>
                    <div className="flex flex-wrap -mx-3 mb-6">
                      <div className="w-full  px-3 mb-6 md:mb-0">
                        <label
                          className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                          htmlFor="grid-first-name"
                        >
                          signature
                        </label>
                      </div>
                    </div>
                    <div className="relative mt-4">
                      <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                        <img
                          src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile?.signature}`}
                          alt="Back CCCD"
                          className="w-full h-full object-cover rounded-lg"
                        />
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="flex justify-center pb-20 pt-10">
          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Thêm thông tin
          </button>
        </div>
      </form>
    </>
  );
};

export default Updateprofile;
