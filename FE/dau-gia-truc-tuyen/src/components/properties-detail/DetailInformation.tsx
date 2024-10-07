import CountdownTimer from '@common/coutdown-timer/CountdownTimer';

const DetailInformation = () => {
  const targetDate = new Date('2024-12-31T23:59:59');
  return (
    <div className="container flex flex-col gap-2">
      <div className="flex gap-1">
            <div className="text-bold line-clamp-2 h-full">
                Cho thuê Tầng 1 (sảnh) của Cơ quan Thông tấn xã Việt Nam khu vực Miền Trung - Tây Nguyên:
                Diện tích 100 m2 x đơn giá 363.000 đồng/1m2/1 tháng = 36.300.000 đồng/tháng.
            </div>
            <div>
                <CountdownTimer targetDate={targetDate} />
            </div>
      </div>
      <div className="h-[2px] w-full bg-[#eee]"></div>
      <div>
        chi tiet
      </div>
    </div>
  );
};
export default DetailInformation;
