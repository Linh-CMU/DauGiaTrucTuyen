import { useEffect, useState } from 'react';
import CardList from '../../common/card-list/CardList';

import {Auction} from 'types';
import {convertDate} from '@utils/helper';

interface AllPropertiesProps {
  listAllAuction: Auction[]; // Use shared Auction type
}
const AllProperties = ({ listAllAuction }: AllPropertiesProps) => {
  return (
    <div className="grid grid-cols-4 gap-4">
      {listAllAuction?.map((card) => (
        <CardList
          id={card.id.toString()}
          isProperties
          key={card.id}
          imgSrc={card.img}
          title={card.name}
          priceStart={card.priceStart}
          startDay={card.startDay}
          targetDate={convertDate(card?.startTime, card?.endDay )}
          url={'thong-tin-chi-tiet'}
        />
      ))}
    </div>
  );
};

export default AllProperties;
