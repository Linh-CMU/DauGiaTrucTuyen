import React, { useState } from 'react';

const Contract = () => {
  const [accepted, setAccepted] = useState(false);

  const handleAccept = () => {
    setAccepted(true);
  };

  return (
    <div className="p-8 max-w-3xl mx-auto bg-white rounded-xl shadow-lg space-y-6 mt-10">
      <h1 className="text-3xl font-bold text-center text-gray-900">
        HỢP ĐỒNG ĐĂNG KÝ ĐẤU GIÁ SẢN PHẨM
      </h1>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 1: Các bên tham gia</h2>
        <p>
          <strong>Bên A</strong> (Bên cung cấp nền tảng đấu giá): Công ty <span className="italic">[Tên Công ty]</span>, 
          địa chỉ: <span className="italic">[Địa chỉ Công ty]</span>, mã số thuế: <span className="italic">[Mã số thuế]</span>, 
          đại diện bởi ông/bà <span className="italic">[Tên đại diện]</span>.
        </p>
        <p>
          <strong>Bên B</strong> (Người bán): <span className="italic">[Tên người bán]</span>, 
          CMND/CCCD số: <span className="italic">[Số CMND/CCCD]</span>, địa chỉ: <span className="italic">[Địa chỉ người bán]</span>.
        </p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 2: Nội dung hợp đồng</h2>
        <p>
          Bên B đăng ký sản phẩm <span className="italic">[Tên sản phẩm]</span> để đấu giá trên trang web của Bên A tại địa chỉ <span className="italic">[URL của trang web]</span>.
        </p>
        <p>
          Bên A cung cấp nền tảng trực tuyến cho hoạt động đấu giá và đảm bảo tính công bằng, minh bạch cho quá trình đấu giá.
        </p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 3: Trách nhiệm và quyền lợi của các bên</h2>
        <p><strong>Trách nhiệm của Bên A:</strong> Đảm bảo hệ thống hoạt động ổn định, bảo vệ dữ liệu người dùng và hỗ trợ kỹ thuật khi cần.</p>
        <p><strong>Trách nhiệm của Bên B:</strong> Cung cấp thông tin sản phẩm chính xác, tuân thủ quy định và chịu trách nhiệm pháp lý với thông tin sản phẩm của mình.</p>
      </section>

      <section className="space-y-4">
        <h2 className="text-xl font-semibold text-gray-700">Điều 4: Hiệu lực hợp đồng</h2>
        <p>Hợp đồng có hiệu lực từ ngày <span className="italic">[Ngày hiệu lực]</span> và kết thúc khi sản phẩm được đấu giá thành công hoặc hai bên đồng ý chấm dứt hợp đồng.</p>
      </section>

      <div className="flex justify-center mt-6">
        <button
          onClick={handleAccept}
          className={`px-8 py-2 font-semibold rounded-md ${accepted ? 'bg-green-500 text-white' : 'bg-blue-500 hover:bg-blue-700 text-white'}`}
          disabled={accepted}
        >
          {accepted ? 'Đã chấp nhận hợp đồng' : 'Chấp nhận hợp đồng'}
        </button>
      </div>
    </div>
  );
};

export default Contract;
