import CountdownTimer from '@common/coutdown-timer/CountdownTimer';
import {AuctionDetails} from 'types';
import {convertDate} from '@utils/helper';


interface InfoRowProps {
  label: string;
  value: string;
}

interface DetailInformationProps {
  auctionDetailInfor: AuctionDetails; // Use shared Auction type
}
const InfoRow: React.FC<InfoRowProps> = ({ label, value }) => (
  <>
    <div className="flex justify-between py-1">
      <div className="font-bold">{label}</div>
      <div className="font-bold">{value}</div>
    </div>
    <div className="h-[2px] w-full bg-gray-200"></div>
  </>
);

const DetailInformation: React.FC<DetailInformationProps> = ({ auctionDetailInfor }) => {
  const targetDate = convertDate(auctionDetailInfor?.startTime, auctionDetailInfor?.endDay);

  console.log(targetDate,"targetDate")

  const auctionInfo = [
    { label: 'Giá khởi điểm', value: `${auctionDetailInfor?.startingPrice} VNĐ` },
    { label: 'Bước giá', value: `${auctionDetailInfor.priceStep} VNĐ` },
    { label: 'Tiền đặt trước', value: `${auctionDetailInfor.moneyDeposit} VNĐ` },
    { label: 'Thời gian đăng kí tham gia', value: `Từ ${auctionDetailInfor.startTime} ${auctionDetailInfor.startDay} đến ${auctionDetailInfor.endTime} ${auctionDetailInfor.endDay}` },
    { label: 'Thời gian bắt đầu đấu giá', value: '09:00:00 31/10/2024' },
    { label: 'Hình thức đấu giá trực tuyến', value: 'Trả giá không xác định vòng' },
    { label: 'Phương thức trả giá', value: auctionDetailInfor.paymentMethod },
  ];

  return (
    <div className="container flex flex-col gap-2 h-full">
      <div className="flex gap-1">
        <div className="font-bold line-clamp-2">
        {auctionDetailInfor?.description}
        </div>
        <CountdownTimer targetDate={targetDate} />
      </div>

      <div className="h-[2px] w-full bg-gray-200"></div>

      <div className="bg-gray-100 w-full h-full p-8 flex flex-col gap-1">
        {auctionInfo.map((item, index) => (
          <InfoRow key={index} label={item.label} value={item.value} />
        ))}
      </div>
    </div>
  );
};

export default DetailInformation;
