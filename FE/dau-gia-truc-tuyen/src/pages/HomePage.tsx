import SearchBox from '../components/SearchBox';
import PropertiesList from '../components/properties-list/PropertiesList';
import NewsList from '../components/news-list/NewsList';
import SearchBox1 from '@components/SearchBox1';
import LiveAuction from '@common/live-auction/LiveAuction';

import CardCarousel from '@common/home/CardCarousel';
import Footer from '@common/footer/Footer';


const HomePage = () => {
  return (
    <>
      <div className="flex flex-col mt-16">
        <div className="w-full h-[450px] relative">
          <img src="banner.jpg" alt="banner-img" className="object-cover h-full w-full" />
          
          <SearchBox />


          <div className="flex flex-col">
            <div className="bg-white">
              <div className=" max-w-1440px mt-28 ml-auto mr-auto">
              <LiveAuction />
              </div>
            </div>

            <div>
              
            </div>
            
            <div className="bg-white">
              <div className=" max-w-1440px mt-28 ml-auto mr-auto">
                <PropertiesList />
              </div>
            </div>
              <div className="bg-lightGray">
              <div className=" max-w-1440px ml-auto mr-auto pl-0 pr-0 pt-8">
                <NewsList/>
              </div>
            </div>
            <div></div>
          </div>
          <Footer />
        </div>
      </div>
    </>
  );
};
export default HomePage;
