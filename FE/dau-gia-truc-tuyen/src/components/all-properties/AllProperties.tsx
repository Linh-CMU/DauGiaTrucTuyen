import { useEffect, useState } from 'react';
import CardList from '../../common/card-list/CardList';
import {getListAuction} from '../../queries/index'

const AllProperties = (props: object) => {
  const [listAllAuction, setListAllAuction] = useState()

  useEffect(() => {
    const fetchListAuction = async () => {
      const response = await getListAuction(); // Call API function
      console.log(response,"data") 
      if(response?.isSucceed){
        setListAllAuction(response?.result)
      }else {
        console.error("fetch list fail")
      }
    };
    fetchListAuction();
  }, []);
  
  const cards = Array.from({ length: 12 }, (_, index) => ({
    id: index + 1, // Assuming the id is just the index + 1
  }));

  return (
    <div className="grid grid-cols-4 gap-4">
      {cards?.map((card) => (
        <CardList
          id={card.id.toString()} 
          isProperties 
          key={card.id} 
        />
      ))}
    </div>
  );
};
export default AllProperties;
