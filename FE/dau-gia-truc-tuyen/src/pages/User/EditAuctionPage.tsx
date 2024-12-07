import React, { useEffect, useState } from 'react';
import { TextField, Select, MenuItem, Button, InputLabel, FormControl } from '@mui/material';
import { getCategory, getDetailAuctionUser, submitEditAuctionForm } from '../../queries/AuctionAPI';
import { AuctionDetailRegister } from 'types';
import { useForm } from 'react-hook-form';
import { useMessage } from '@contexts/MessageContext';
import { useLocation } from 'react-router-dom';
import Footer from '@common/footer/Footer';

export interface AuctionDetail {
  listAuctionID: number;
  category: string;
  categoryId: number;
  name: string;
  image: any;
  imageEvidence: any;
  nameAuction: string;
  description: string;
  startingPrice: number;
  stepPrice: number;
  priceDeposit: number;
  startDay: string;
  startTime: string;
  endDay: string;
  endTime: string;
  statusAuction: string;
  countBidder: number;
}

export interface EditAuctionItemFormData {
  auctionID: number;
  imageAuction: any;
  imageEvidence: any;
  nameAuctionItem: string;
  description: string;
  startingPrice: number;
  category: number;
}
const EditActionPage: React.FC = () => {
  const {
    handleSubmit,
    setValue,
    register,
    formState: { errors },
  } = useForm<EditAuctionItemFormData>();
  const [listCategory, setCategory] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [auctionDetailInfor, setAuctionDetailInfor] = useState<AuctionDetail>();
  const { setSuccessMessage, setErrorMessage } = useMessage();
  const location = useLocation();
  const { id } = location.state || {};
  useEffect(() => {
    const fetchListAuction = async () => {
      const response = await getDetailAuctionUser(id); // Call API function
      if (response?.isSucceed) {
        setAuctionDetailInfor(response?.result); // Ensure result is an object or null
        console.log(response?.result.image);
      } else {
        console.error('fetch list failed');
      }
    };
    fetchListAuction();
  }, []);
  const fetchListCategory = async () => {
    try {
      const response = await getCategory();
      if (response?.isSucceed) {
        setCategory(response?.result);
        console.log('response', response);
      } else {
        console.error('Failed to fetch categories');
      }
    } catch (error) {
      console.error('Error fetching categories:', error);
    } finally {
      setLoading(false);
    }
  };
  useEffect(() => {
    fetchListCategory();
  }, []);
  const submit = async () => {
    try {
      if(auctionDetailInfor?.startingPrice) {
        if(auctionDetailInfor?.startingPrice < 20000){
          setErrorMessage('starting amount must be greater than 20,000 VND');
          return;
        }
      }
      const response = await submitEditAuctionForm(auctionDetailInfor as AuctionDetail);
      if (response.isSucceed) {
        setSuccessMessage('You have successfully edited the product.');
      } else {
        setErrorMessage('System error please contact admin');
      }
    } catch (error) {
      console.error('Error creating auction item:', error);
    }
    // Handle form submission
    console.log('Auction Details:', auctionDetailInfor);
  };

  const [imagePreview, setImagePreview] = useState('');
  const [imageEvidence, setImageEvidence] = useState('');
  useEffect(() => {
    // Khởi tạo hình ảnh ban đầu từ dữ liệu sản phẩm nếu chưa có ảnh nào mới
    if (auctionDetailInfor && !imagePreview && !imageEvidence) {
      setImagePreview(
        `http://capstoneauctioneer.runasp.net/api/read?filePath=${auctionDetailInfor.image}`
      );
      setImageEvidence(
        `http://capstoneauctioneer.runasp.net/api/read?filePath=${auctionDetailInfor.imageEvidence}`
      );
    }
  }, [auctionDetailInfor, imagePreview, imageEvidence]);

  const handleImageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const previewURL = URL.createObjectURL(file);
      setImagePreview(previewURL);
      setAuctionDetailInfor((prev: any) => ({
        ...prev,
        image: file,
      }));
    }
  };
  const handleImageEvidenceChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (file) {
      const previewURL = URL.createObjectURL(file);
      setImageEvidence(previewURL);
      setAuctionDetailInfor((prev: any) => ({
        ...prev,
        imageEvidence: file,
      }));
    }
  };
  const handleRemoveImage = () => {
    setImagePreview(''); // Xóa bản xem trước
    setAuctionDetailInfor((prev: any) => ({
      ...prev,
      image: '', // Xóa ảnh hiện tại khỏi trạng thái
    }));
  };
  const handleRemoveEvidenceImage = () => {
    setImageEvidence(''); // Xóa bản xem trước
    setAuctionDetailInfor((prev: any) => ({
      ...prev,
      imageEvidence: '', // Xóa ảnh hiện tại khỏi trạng thái
    }));
  };

  return (
    <>
      <div className="w-full mt-10 flex items-center justify-center pt-10">
        <h5 className="font-bold text-center text-2xl pl-9">UPDATE AUCTION PRODUCTS</h5>
      </div>
      <form onSubmit={handleSubmit(submit)} className="h-[80vh]">
        <div className="flex justify-center pt-10">
          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="full-name"
                  >
                    Product Name
                  </label>
                  <textarea
                    className="appearance-none block w-full bg-gray-200 text-gray-700 border rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    id="name"
                    defaultValue={auctionDetailInfor?.nameAuction}
                    {...register('nameAuctionItem', {
                      onChange: (e) => {
                        setAuctionDetailInfor((prev: any) => ({
                          ...prev,
                          nameAuction: e.target.value,
                        }));
                      },
                    })}
                    placeholder="Enter full name"
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="gender"
                  >
                    Describe
                  </label>
                  <textarea
                    className="appearance-none block w-full bg-gray-200 text-gray-700 border rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    id="name"
                    defaultValue={auctionDetailInfor?.description}
                    {...register('description', {
                      onChange: (e) => {
                        setAuctionDetailInfor((prev: any) => ({
                          ...prev,
                          description: e.target.value,
                        }));
                      },
                    })}
                    placeholder="Enter full name"
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="birth-date"
                  >
                    Product price
                  </label>
                  <input
                    className="appearance-none block w-full bg-gray-200 text-gray-700 border rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                    id="birth-date"
                    type="text"
                    value={
                      auctionDetailInfor?.startingPrice
                        ? new Intl.NumberFormat('vi-VN', {
                            style: 'currency',
                            currency: 'VND',
                          }).format(auctionDetailInfor.startingPrice)
                        : ''
                    }
                    {...register('startingPrice', {
                      onChange: (e) => {
                        const rawValue = e.target.value.replace(/[^\d]/g, ''); // Loại bỏ ký tự không phải số
                        const numericValue = parseInt(rawValue || '0', 10); // Chuyển về số nguyên
                        setAuctionDetailInfor((prev: any) => ({
                          ...prev,
                          startingPrice: numericValue,
                        }));
                      },
                    })}
                    required
                  />
                </div>
              </div>
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <label
                    className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                    htmlFor="address"
                  >
                    Product Type
                  </label>
                  <select
                    id="category"
                    {...register('category', {
                      onChange: (e) => {
                        setAuctionDetailInfor((prev: any) => ({
                          ...prev,
                          category: e.target.value,
                        }));
                      },
                    })}
                    className="appearance-none block w-full bg-gray-200 text-gray-700 border rounded py-3 px-4 leading-tight focus:outline-none focus:bg-white focus:border-gray-500"
                  >
                    {listCategory ? (
                      <>
                        {listCategory.map((item, index) => (
                          <option
                            key={index}
                            value={item.categoryID}
                            defaultChecked={item.categoryID === auctionDetailInfor?.categoryId}
                          >
                            {item.nameCategory}
                          </option>
                        ))}
                      </>
                    ) : (
                      <></>
                    )}
                  </select>
                </div>
              </div>
              {/* Additional fields such as phone number, household, etc. */}
            </div>
          </div>
          <div className="w-1/3 p-4">
            <div className="w-full max-w-lg">
              {/* Add image upload section here */}
              <div className="flex flex-wrap -mx-3 mb-6">
                <div className="w-full px-3 mb-6 md:mb-0">
                  <div className="flex flex-wrap -mx-3 mb-6 items-center">
                    {/* First column: Label */}
                    <div className="w-full md:w-1/2 px-3">
                      <label
                        className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                        htmlFor="grid-first-name"
                      >
                        Product Image
                      </label>
                    </div>

                    {/* Second column: Image input */}
                    <div className="w-full md:w-1/2 px-3">
                      <label
                        className="block uppercase tracking-wide text-gray-700 text-xs font-bold mb-2"
                        htmlFor="grid-first-name"
                      >
                        Product demonstration images
                      </label>
                    </div>
                  </div>
                  <div className="flex flex-wrap -mx-3 mb-6">
                    <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                      <div className="flex items-center justify-center w-full">
                        {imagePreview ? (
                          <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                            <img
                              src={imagePreview}
                              alt="Preview"
                              className="w-full h-full object-cover rounded-lg"
                            />
                            <button
                              onClick={handleRemoveImage}
                              className="absolute top-0 right-0 p-1 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center"
                            >
                              X
                            </button>
                          </div>
                        ) : (
                          <label
                            htmlFor="dropzone-file"
                            className="flex flex-col items-center justify-center w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100"
                          >
                            <p className="mb-2 text-sm text-gray-500">
                              <span className="font-semibold">Click để tải lên</span> hoặc kéo và
                              thả
                            </p>
                            <input
                              id="dropzone-file"
                              type="file"
                              accept="image/*"
                              onChange={handleImageChange}
                              className="hidden"
                            />
                          </label>
                        )}
                      </div>
                    </div>
                    <div className="w-full md:w-1/2 px-3 mb-6 md:mb-0">
                      <div className="flex items-center justify-center w-full">
                        {imageEvidence ? (
                          <div className="relative w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer">
                            <img
                              src={imageEvidence}
                              alt="Preview"
                              className="w-full h-full object-cover rounded-lg"
                            />
                            <button
                              onClick={handleRemoveEvidenceImage}
                              className="absolute top-0 right-0 p-1 bg-red-500 text-white rounded-full w-6 h-6 flex items-center justify-center"
                            >
                              X
                            </button>
                          </div>
                        ) : (
                          <label
                            htmlFor="dropzone-file"
                            className="flex flex-col items-center justify-center w-full h-32 border-2 border-gray-300 border-dashed rounded-lg cursor-pointer bg-gray-50 hover:bg-gray-100"
                          >
                            <p className="mb-2 text-sm text-gray-500">
                              <span className="font-semibold">Click để tải lên</span> hoặc kéo và
                              thả
                            </p>
                            <input
                              id="dropzone-file"
                              type="file"
                              accept="image/*"
                              onChange={handleImageEvidenceChange}
                              className="hidden"
                            />
                          </label>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        {/* Additional upload sections and fields here */}
        <div className="flex items-center justify-center">
          <button
            type="submit"
            className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
          >
            Submit
          </button>
        </div>
      </form>
    </>
  );
};

export default EditActionPage;
