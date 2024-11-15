import { profileResponse } from '../../types/auth.type';
import { inforUser } from '../../queries/AdminAPI';
import React, { useEffect, useState } from 'react';
import { Button } from '@mui/material';
import { useLocation, useNavigate, useParams } from 'react-router-dom';

const InforUser = () => {
  const [profile, setProfile] = useState<profileResponse | null>();
  const navigate = useNavigate();
  const location = useLocation();
  const { 
    iduser
  } = location.state || {};
  const handleToInfor = () => {
    // Pass `iduser` as part of the state to the `/inforUser` route
    navigate(-1);
  };
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await inforUser(iduser?.toString());
        console.log(response.result);
        setProfile(response.result);
      } catch (error) {}
    };
    fetchData();
  }, []);
  return (
    <>
      <div className="w-full mt-3 flex items-center justify-center pt-10">
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
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.city}
                    disabled
                  />
                </div>
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Districts
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.district}
                    disabled
                  />
                </div>
                <div className="w-full md:w-1/3  px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Ward
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.ward}
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
            <div className='mt-10'>
              <Button
                className="bg-green-900 hover:bg-green-800 text-white"
                disableElevation
                variant="contained"
                onClick={() => handleToInfor()}
              >
                Back
              </Button>
            </div>
          </div>
        </div>
      ) : (
        <div>Not info</div>
      )}
    </>
  );
};

export default InforUser;
