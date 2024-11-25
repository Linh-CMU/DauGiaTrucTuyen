import { Button } from '@mui/material';
import { getCity, getDistrict, getWard, profileUser } from '../../queries/AdminAPI';
import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { cityResponse, districtResponse, wardResponse } from '../../types/auth.type';
import { useMessage } from '@contexts/MessageContext';

interface profileResponse {
  fullName: string;
  phone: string;
  city: string;
  ward: string;
  district: string;
  address: string;
  avatar: string;
  frontCCCD: string;
  backsideCCCD: string;
  gender: boolean;
  birthdate: string;
  placeOfResidence: string;
  placeOfIssue: string;
  dateOfIssue: string;
}

const Profile = () => {
  const [profile, setProfile] = useState<profileResponse | null>();
  const [citys, setCitys] = useState<cityResponse[]>([]);
  const [districts, setDistricts] = useState<districtResponse[]>([]);
  const [wards, setWards] = useState<wardResponse[]>([]);
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const navigate = useNavigate();
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
      }
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
    if(profile) {
       const filteredDistrict = districts.filter(
          (district) => district.province_code.toString() === profile?.city
        );
        const filteredWard = wards.filter(
          (ward) => ward.district_code.toString() === profile?.district
        );
        setfilteredWards(filteredWard);
        setfilteredDistricts(filteredDistrict);
    }
  }, [profile])
  
  return (
    <>
      <div className="w-full mt-8 flex items-center justify-center pt-10">
        <h5 className="font-bold text-center text-2xl">INFORMATION</h5>
      </div>
      {profile ? (
        <div className="flex justify-center pt-10 pb-20">
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
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    value={profile.fullName}
                    disabled
                  />
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
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    placeholder="Enter full name"
                    value={profile.gender ? 'Nam' : 'Nữ'}
                    disabled
                  />
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
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.birthdate}
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
                    disabled
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
                  >
                    <option value="">Select a city</option>
                    {citys.map((cityItem) => (
                      <option key={cityItem.code} value={cityItem.code.toString()}>
                        {cityItem.name}
                      </option>
                    ))}
                  </select>
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
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={profile?.district}
                    disabled
                    onChange={(e) => {
                      const selectDis = districts.find(
                        (districtResponse) => districtResponse.code.toString() === e.target.value
                      );
                      setProfile((prev: any) => ({
                        ...prev,
                        district: selectDis?.code.toString(),
                      }));
                    }}
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
                    disabled
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    value={profile?.ward}
                    onChange={(e) => {
                      setProfile((prev: any) => ({
                        ...prev,
                        ward: e.target.value,
                      }));
                    }}
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
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.address}
                    disabled
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
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.phone}
                    disabled
                  />
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
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.placeOfResidence}
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
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.frontCCCD}`}
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
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.backsideCCCD}`}
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
                    value={profile.placeOfIssue}
                    disabled
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
                    type="text"
                    placeholder="Enter full name"
                    value={profile.dateOfIssue}
                    disabled
                  />
                </div>
              </div>
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
                <div className="relative mt-4">
                  <img
                    src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.avatar}`}
                    className="rounded-full w-64 h-64 object-cover border-2 border-gray-300 border-dashed"
                    alt="Selected"
                  />
                </div>
              </div>
            </div>
          </div>
          <div>
            <Button
              className="bg-green-900 hover:bg-green-800 text-white"
              disableElevation
              variant="contained"
              onClick={() => navigate('/update-profile')}
            >
              Update
            </Button>
          </div>
        </div>
      ) : (
        <div>Not info</div>
      )}
    </>
  );
};

export default Profile;
