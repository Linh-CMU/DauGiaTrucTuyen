import { useEffect, useState } from 'react';
import CardList from '../../common/card-list/CardList';
import { AuctionUser } from 'types';
import { convertDate } from '@utils/helper';

interface AllPropertiesProps {
  listAllAuction: AuctionUser[];
}

const AllPropertiesUser = ({ listAllAuction }: AllPropertiesProps) => {
  const now = new Date();

  const getCountdownTarget = (startDay: string, startTime: string, endDay: string, endTime: string) => {
    const [day, month, year] = startDay?.split('/') || [];
    const [startHours, startMinutes] = startTime?.split(':') || [];
    const startDate = new Date(+year, +month - 1, +day, +startHours, +startMinutes);

    if (startDate > now) {
      return { date: startDay, time: startTime };
    } else {
      const [endDayPart, endMonth, endYear] = endDay?.split('/') || [];
      const [endHours, endMinutes] = endTime?.split(':') || [];
      return { date: `${endDayPart}/${endMonth}/${endYear}`, time: `${endHours}:${endMinutes}` };
    }
  };

  return (
    <div className="grid grid-cols-4 gap-4">
      {listAllAuction?.map((card) => {
        const countdownTarget = getCountdownTarget(card.startDay, card.startTime, card.endDay, card.endTime);
        const targetDate = convertDate(countdownTarget.time, countdownTarget.date); // Ensure `convertDate` returns a Date object

        return (
          <CardList
            id={card.listAuctionID.toString()}
            isProperties
            key={card.listAuctionID}
            imgSrc={card.image}
            title={card.nameAuction}
            priceStart={card.startingPrice}
            startDay={card.startDay}
            targetDate={targetDate as Date}
            isApproved={card.statusAuction}
            url={'detail-auction'}
          />
        );
      })}
    </div>
  );
};

export default AllPropertiesUser;
