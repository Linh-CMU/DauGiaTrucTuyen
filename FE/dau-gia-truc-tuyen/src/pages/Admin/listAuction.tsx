import { useLocation, useParams } from 'react-router-dom';
import ListAuctionAdmin from '../../components/listauction-admin/listauctionadmin';
import SearchBox from '../../components/SearchBox';
import avt from '../../../public/banner.jpg'; // Importing the image correctly
import { useState } from 'react';
import { Auction } from '../../types/auction.type';
import { getSearchAuction } from '@queries/AuctionAPI';
import Footer from '@common/footer/Footer';

const ListAuction = () => {
  const location = useLocation();
  const { id, name, status } = location.state || {};
  const [searchResults, setSearchResults] = useState<Auction[]>([]);
  const [isSearch, setIsSearch] = useState(false);
  const handleSubmit = async (categoryId: number, value: string) => {
    try {
      console.log('Category ID:', categoryId);
      console.log('Search Value:', value);

      const response = await getSearchAuction(categoryId, value);
      console.log('Search Results:', response);
      setIsSearch(true);
      setSearchResults(response);
    } catch (error) {
      console.error('Error searching auctions:', error);
    }
  };
  return (
    <div className="flex flex-col">
      <div className="w-full h-[450px] relative">
        <img src={avt} alt="banner-img" className="object-cover h-full w-full" />
        <SearchBox handleSubmit={handleSubmit}/>
        <div className="flex flex-col">
          <div className="bg-white">
            <div className="max-w-1440px mt-28 ml-auto mr-auto">
              <ListAuctionAdmin id={id?.toString()} name={name?.toString()} status={status} />
            </div>
          </div>
        </div>
        <Footer />
      </div>
    </div>
  );
};

export default ListAuction;
