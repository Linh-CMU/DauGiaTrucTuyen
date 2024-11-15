import { useNavigate } from "react-router-dom";


interface CardListProps {
    id: string,
    isProperties: boolean;
    // imgSrc: string,
    // title: string,
    // priceStart: string,
  
  }
  
const CardList1 = () => {
    const targetDate = new Date('2024-12-31T23:59:59');
    const navigate = useNavigate();

    const handleDetailClick = () => {
        // navigate(`/thong-tin-chi-tiet/${id}`);
      };

    return (
        <div className="max-w-xs bg-white rounded-lg shadow-lg overflow-hidden group border border-transparent hover:border-blue-500">
            <div className="relative">
                <img
                    className="w-full h-64 object-cover transition-transform duration-500 transform group-hover:scale-105"
                    src="http://capstoneauctioneer.runasp.net/api/read?filePath=ListAuctioneer/1dd89b64-6060-44a3-b67e-4ed1d719849d_20241006164359_26082024102501739DewQXfcxrgPw3ca3.png"
                    alt="Art Auction"
                />
                <div className="absolute top-2 left-2 bg-red-600 text-white text-xs font-semibold px-2 py-1 rounded-full flex items-center space-x-1">
                    <span>🔴</span>
                    <span>Live</span>
                </div>
                <div className="absolute bottom-5 left-1/2 transform -translate-x-1/2 bg-white bg-opacity-90 p-2 rounded-full w-11/12 flex justify-center items-center gap-4 group-hover:hidden">
                    <div className="text-center">
                        <span className="font-bold text-sm">00</span>
                        <span className="block text-xs">Days</span>
                    </div>
                    <div className="text-center">
                        <span className="font-bold text-sm">:</span>
                    </div>
                    <div className="text-center">
                        <span className="font-bold text-sm">00</span>
                        <span className="block text-xs">Hours</span>
                    </div>
                    <div className="text-center">
                        <span className="font-bold text-sm">:</span>
                    </div>
                    <div className="text-center">
                        <span className="font-bold text-sm">00</span>
                        <span className="block text-xs">Mint</span>
                    </div>
                    <div className="text-center">
                        <span className="font-bold text-sm">:</span>
                    </div>
                    <div className="text-center">
                        <span className="font-bold text-sm">00</span>
                        <span className="block text-xs">Sec</span>
                    </div>
                </div>
            </div>
            <div className="p-4">
                <h2 className="text-gray-900 font-semibold text-lg line-clamp-1">
                Cho thuê Tầng 1 (sảnh) của Cơ quan Thông tấn xã Việt Nam khu vực Miền Trung - Tây Nguyên:
                </h2>
                <p className="mt-2 text-gray-600">
                    Giá khởi điểm: <span className="text-black font-bold">36.000.000 VNĐ</span>
                </p>
                <button className="mt-4 w-full bg-black text-white font-bold py-2 px-4 rounded group-hover:bg-sky-700">
                    Đấu giá
                </button>
            </div>
        </div>

    )
}

export default CardList1;