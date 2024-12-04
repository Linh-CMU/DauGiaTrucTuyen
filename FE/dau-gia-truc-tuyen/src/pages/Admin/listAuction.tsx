import { useLocation, useParams } from 'react-router-dom';
import ListAuctionAdmin from '../../components/listauction-admin/listauctionadmin';
import Footer from '@common/footer/Footer';

const ListAuction = () => {
  const location = useLocation();
  const { id, name, status } = location.state || {};
  return (
    <div className="flex flex-col">
      <div className="w-full h-[450px] relative">
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
