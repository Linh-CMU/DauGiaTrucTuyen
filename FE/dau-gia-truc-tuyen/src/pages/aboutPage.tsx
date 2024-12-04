import React from 'react';
import img from '../../public/banner.jpg'

const About = () => {
    return (
        <div className="p-8 bg-gray-50 text-gray-800 font-sans mt-16">
            {/* Giới thiệu */}
            <div className="mb-12">
                <h1 className="text-4xl font-bold text-center mb-6 text-blue-600">Trung tâm dịch vụ đấu giá tài sản trực tuyến</h1>
                <p className="text-center text-lg italic text-gray-600">“Nơi hội tụ và phát triển tài sản với giá trị cao nhất”</p>
            </div>

            {/* Thông tin đơn vị */}
            <div className="mb-12 flex flex-col md:flex-row items-center bg-white shadow-lg rounded-lg p-6">
                <img src={img} alt="Location" className="w-40 h-40 object-cover rounded-full mb-4 md:mb-0 md:mr-6" />
                <div>
                    <h2 className="text-2xl font-semibold mb-4 text-blue-500">Thông tin đơn vị</h2>
                    <p className="mb-2"><span className="font-semibold">Tên đơn vị:</span> Trung tâm dịch vụ đấu giá tài sản thành phố Đà Nẵng</p>
                    <p className="mb-2"><span className="font-semibold">Địa chỉ:</span> Số 08 Phan Bội Châu, phường Thạch Thang, quận Hải Châu, thành phố Đà Nẵng</p>
                    <p className="mb-2"><span className="font-semibold">Điện thoại:</span> (0236) 3895 593 - 3889 627 - 3887678</p>
                </div>
            </div>

            {/* Nhân sự */}
            <div className="mb-12">
                <h2 className="text-2xl font-semibold mb-6 text-blue-500">Nhân sự</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Nhật Long:</span> Trưởng nhóm, Đấu giá viên</p>
                    </div>
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Trần Thư: </span> Member</p>
                    </div>
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Nguyễn Trường Hiếu: </span> Member, Đấu giá viên</p>
                    </div>
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Khánh Vân: </span>Member</p>
                    </div>
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Lê Quang: </span> Mentor</p>
                    </div>
                </div>
            </div>

            {/* Chức năng nhiệm vụ */}
            <div>
                <h2 className="text-2xl font-semibold mb-6 text-blue-500">Chức năng nhiệm vụ</h2>
                <p className="mb-4">Trung tâm dịch vụ đấu giá tài sản Đà Nẵng là đơn vị trực thuộc Sở Tư pháp thành phố Đà Nẵng được thành lập theo quyết định số 1540/QĐ-UB ngày 03/6/1997 của UBND thành phố Đà Nẵng và được bổ sung thêm nhiệm vụ mới theo Quyết định số 01/2006/QĐ-UBND ngày 04/01/2006 của UBND thành phố Đà Nẵng.</p>
                <ul className="list-disc list-inside bg-white shadow-lg rounded-lg p-6 space-y-2">
                    <li>Tài sản nhà nước theo quy định của pháp luật về quản lý, sử dụng tài sản nhà nước;</li>
                    <li>Tài sản được xác lập quyền sở hữu toàn dân theo quy định của pháp luật;</li>
                    <li>Tài sản thi hành án theo quy định của pháp luật về thi hành án dân sự;</li>
                    <li>Tài sản là hàng dự trữ quốc gia theo quy định của pháp luật về dự trữ quốc gia;</li>
                    <li>Tài sản là tang vật, phương tiện vi phạm hành chính bị tịch thu sung quỹ nhà nước;</li>
                    <li>Tài sản đảm bảo theo quy định pháp luật về giao dịch đảm bảo Hàng hoá lưu giữ do người vận chuyển đường biển, đường bộ, đường hàng không lưu giữ tại Việt Nam;</li>
                    <li>Tài sản thuộc sở hữu của cá nhân, tổ chức, doanh nghiệp có yêu cầu bán đấu giá tài sản;</li>
                    <li>Tài sản Nhà nước phải bán đấu giá theo quy định pháp luật về quản lý tài sản Nhà nước: tài sản thanh lý của cơ quan hành chính, đơn vị sự nghiệp, tài sản thanh lý của doanh nghiệp Nhà nước, ...</li>
                </ul>
            </div>
        </div>
    );
};

export default About;