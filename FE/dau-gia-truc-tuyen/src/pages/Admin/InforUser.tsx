import { profileResponse } from '../../types/auth.type';
import { inforUser } from '../../queries/AdminAPI';
import React, { useEffect, useState } from 'react';
import { Button } from '@mui/material';
import { useParams } from 'react-router-dom';

const InforUser = () => {
  const [profile, setProfile] = useState<profileResponse | null>();
  const {id} = useParams();
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await inforUser(id?.toString());
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
        <div className="flex justify-center pt-10">
          <div className="w-2/1 p-4">
            <div className="relative mt-4">
              <img
                src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.avatar}`}
                className="rounded-full w-64 h-64 object-cover border-2 border-gray-300 border-dashed"
                alt="Selected"
              />
            </div>
            <div className="mt-5 mr-3">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <div className="flex items-center justify-center w-full">
                    <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                      <img
                        src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile.frontCCCD}`}
                        alt="Back CCCD"
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
            </div>
          </div>

          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Tên người dùng
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.fullName}
                    disabled
                  />
                </div>
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Số điện thoại
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
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Email
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.email}
                    disabled
                  />
                </div>
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Số lần cảnh báo
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.warning}
                    disabled
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Loại người dùng
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.role}
                    disabled
                  />
                </div>
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Trạng thái người dùng
                  </label>
                  <input
                    className={`appearance-none block w-full bg-gray-200 text-gray-700 border  border-gray-200 rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500`}
                    id="grid-last-name"
                    type="text"
                    required
                    value={profile.status === false ? 'Hoạt động' : 'Dừng hoạt động'}
                    disabled
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Thành Phố/Tỉnh
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
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Quận/Huyện
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
              </div>

              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Phường/Xã
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
                <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="grid-first-name"
                  >
                    Số nhà
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
                <Button href={`/listAuction/${profile.accountId}/${profile.fullName}`} style={{backgroundColor : 'green', color: 'white', width : '200px'}} className="bg-red-500 text-white px-2 py-1 rounded">Sản phẩm đấu giá</Button>
                <button className="bg-red-500 text-white px-2 py-1 rounded ml-5">Những buổi đăng ký đấu giá</button>
              </div>
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
