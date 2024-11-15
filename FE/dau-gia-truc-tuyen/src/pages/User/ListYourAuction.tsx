import SearchBox from '../../components/SearchBox';
import PropertiesList from '../../components/propertiesList-user/PropertiesList';

const ListYourAuction = () => {
  return (
    <>
      <div className="flex flex-col mt-16">
        <div className="w-full h-[450px] relative">
          <img src="banner.jpg" alt="banner-img" className="object-cover h-full w-full" />
          <SearchBox />
          <div className="flex flex-col">
            <div className="bg-white">
              <div className=" max-w-1440px mt-28 ml-auto mr-auto">
                <PropertiesList />
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
export default ListYourAuction;
