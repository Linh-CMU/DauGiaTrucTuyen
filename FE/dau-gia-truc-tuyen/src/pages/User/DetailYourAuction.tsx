// pages/DetailPage.tsx
import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { CarouselDetail } from '@components/properties-detail';
import AuctionRoom from '@components/auction-room/AuctionRoom';
import {  getDetailAuctionUser } from '../../queries/index';
import {  Account, AuctionDetailRegister } from 'types';
import DetailInformationUser from '@components/properties-detail/DetailInformationUser';
import DetailContentUser from '@components/properties-detail/DetailContentUser';

const DetailAuctionPage = () => {
  const { id } = useParams<{ id: string }>();
  const [auctionDetailInfor, setAuctionDetailInfor] = useState<AuctionDetailRegister | null>(null);
  
  useEffect(() => {
    const fetchListAuction = async () => {
      const response = await getDetailAuctionUser(id || '0'); // Call API function
      if (response?.isSucceed) {
        setAuctionDetailInfor(response?.result || null); // Ensure result is an object or null
        console.log(response?.result);
        
      } else {
        console.error('fetch list failed');
      }
    };
    fetchListAuction();
  }, [id]);

  return (
    <div className="container mx-auto p-4 flex flex-col gap-6 mt-20">
      <div className="flex gap-6 max-w-1/2">
        <CarouselDetail imgList={auctionDetailInfor?.images} />
        <div className="col-span-2 w-full">
          {auctionDetailInfor && (
            <DetailInformationUser 
              auctionDetailInfor={auctionDetailInfor}
            />
          )}
        </div>
      </div>
      <DetailContentUser auctionDetailInfor={auctionDetailInfor} />
    </div>
  );
};

export default DetailAuctionPage;
