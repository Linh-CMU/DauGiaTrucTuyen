// pages/DetailPage.tsx
import React from 'react';
import { useParams } from 'react-router-dom';
import {CarouselDetail, DetailInformation} from '@components/properties-detail'

const DetailPage = () => {
  const { id } = useParams<{ id: string }>();

  return (
    <div className="container mx-auto p-4">
      <div className="grid grid-cols-3 grid-rows-5 gap-4">
       <CarouselDetail />
        <div className="col-span-2">
            <DetailInformation />
        </div>
      </div>

      <div>Thông tin chi tiết</div>
    </div>
  );
};

export default DetailPage;
