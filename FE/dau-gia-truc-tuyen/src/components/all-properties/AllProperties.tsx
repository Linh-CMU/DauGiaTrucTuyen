import { useEffect, useState } from 'react';
import CardList from '../../common/card-list/CardList';

import { Auction } from 'types';
import { convertDate } from '@utils/helper';

interface AllPropertiesProps {
  listAllAuction: Auction[]; // Use shared Auction type
  value: string;
}
const AllProperties = ({ listAllAuction, value }: AllPropertiesProps) => {
  return (
    <div className="grid grid-cols-4 gap-4">
      {listAllAuction?.map((card) => {
        const calculateNewTargetDate = (
          endTime: string,
          endDay: string,
          timePerLap: string
        ): Date => {
          if (!endTime || !endDay || !timePerLap) {
            console.warn("Missing required parameters for calculateNewTargetDate:", {
              endTime,
              endDay,
              timePerLap,
            });
            return new Date(); // Trả về thời gian hiện tại nếu thiếu tham số
          }
        
          try {
            // Sử dụng convertDate để tạo đối tượng ngày ban đầu
            const endDate = convertDate(endTime, endDay);
            const now = new Date();

            // Tách giờ và phút từ TimePerLap
            const [hours, minutes] = timePerLap.split(':').map(Number);
        
            if (endDate < now) {
              // Nếu endDate đã qua, cộng thêm giờ và phút từ TimePerLap
              endDate.setHours(endDate.getHours() + hours);
              endDate.setMinutes(endDate.getMinutes() + minutes);
            }
        
            return endDate; // Trả về đối tượng Date
          } catch (error) {
            console.error("Error in calculateNewTargetDate:", { endTime, endDay, timePerLap, error });
            return new Date(); // Trả về giá trị mặc định trong trường hợp lỗi
          }
        };
        
        // Tính toán ngày mục tiêu mới
        const targetDate = calculateNewTargetDate(card?.endTime, card?.endDay, card?.timePerLap);
        

        return (
          <CardList
            id={card.id.toString()}
            isProperties
            key={card.id}
            imgSrc={card.img}
            title={card.name}
            endDay={card.endDay}
            endTime={card.endTime}
            priceStart={card.priceStart}
            startDay={card.startDay}
            targetDate={targetDate}
            url={value}
          />
        );
      })}
    </div>
  );
};

export default AllProperties;
