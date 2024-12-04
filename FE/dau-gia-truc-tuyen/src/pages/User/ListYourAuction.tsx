import { useState } from 'react';
import SearchBox from '../../components/SearchBox';
import PropertiesList from '../../components/propertiesList-user/PropertiesList';
import { Auction } from '../../types/auction.type';
import { getSearchAuction, searchauctionregistrationlist } from '@queries/AuctionAPI';
import Footer from '@common/footer/Footer';

const ListYourAuction = () => {
  const [searchResults, setSearchResults] = useState<Auction[]>([]);
  const [isSearch, setIsSearch] = useState(false);
  const handleSubmit = async (categoryId: number, value: string) => {
    try {
      const response = await searchauctionregistrationlist(categoryId, value);
      console.log('response', response);
      setIsSearch(true);
      setSearchResults(response);
    } catch (error) {
      console.error('Error searching auctions:', error);
    }
  };
  return (
    <>
      <div className="flex flex-col mt-16">
        <div className="w-full h-[450px] relative">
          <img src="banner.jpg" alt="banner-img" className="object-cover h-full w-full" />
          <SearchBox handleSubmit={handleSubmit}/>
          <div className="flex flex-col">
            <div className="bg-white">
              <div className=" max-w-1440px mt-28 ml-auto mr-auto">
                <PropertiesList searchResults={searchResults} isSearch={isSearch} setIsSearch={setIsSearch}/>
              </div>
            </div>
          </div>
          <Footer />
        </div>
      </div>
    </>
  );
};
export default ListYourAuction;
