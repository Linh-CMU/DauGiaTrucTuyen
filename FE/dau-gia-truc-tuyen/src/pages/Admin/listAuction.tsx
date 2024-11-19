import { useLocation, useParams } from 'react-router-dom';
import ListAuctionAdmin from '../../components/listauction-admin/listauctionadmin';
import SearchBox from '../../components/SearchBox';
import avt from '../../../public/banner.jpg'; // Importing the image correctly

const ListAuction = () => {
  const location = useLocation();
  const { id, name, status } = location.state || {};
  return (
    <div className="flex flex-col">
      <div className="w-full h-[450px] relative">
        <img src={avt} alt="banner-img" className="object-cover h-full w-full" />
        <SearchBox />
        <div className="flex flex-col">
          <div className="bg-white">
            <div className="max-w-1440px mt-28 ml-auto mr-auto">
              <ListAuctionAdmin id={id?.toString()} name={name?.toString()} status={status} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ListAuction;
