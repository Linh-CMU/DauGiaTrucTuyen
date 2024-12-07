import React from 'react';
import img from '../../public/banner.jpg'

const About = () => {
    return (
        <div className="p-8 bg-gray-50 text-gray-800 font-sans mt-16">
            {/* Giới thiệu */}
            <div className="mb-12">
                <h1 className="text-4xl font-bold text-center mb-6 text-blue-600">Online property auction service center</h1>
                <p className="text-center text-lg italic text-gray-600">“A place to gather and develop assets with the highest value”</p>
            </div>

            {/* Thông tin đơn vị */}
            <div className="mb-12 flex flex-col md:flex-row items-center bg-white shadow-lg rounded-lg p-6">
                <img src={img} alt="Location" className="w-40 h-40 object-cover rounded-full mb-4 md:mb-0 md:mr-6" />
                <div>
                    <h2 className="text-2xl font-semibold mb-4 text-blue-500">Unit information</h2>
                    <p className="mb-2"><span className="font-semibold">Unit name:</span> Danang City Property Auction Service Center</p>
                    <p className="mb-2"><span className="font-semibold">Address:</span> Số 08 Phan Bội Châu, phường Thạch Thang, quận Hải Châu, thành phố Đà Nẵng</p>
                    <p className="mb-2"><span className="font-semibold">Phone:</span> (0236) 3895 593 - 3889 627 - 3887678</p>
                </div>
            </div>

            {/* Nhân sự */}
            <div className="mb-12">
                <h2 className="text-2xl font-semibold mb-6 text-blue-500">Human Resources</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Nhật Long:</span> Team Leader, Auctioneer</p>
                    </div>
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Trần Thư: </span> Member</p>
                    </div>
                    <div className="bg-white shadow-md rounded-lg p-4">
                        <p><span className="font-semibold">Nguyễn Trường Hiếu: </span> Member, Auctioneer</p>
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
                <h2 className="text-2xl font-semibold mb-6 text-blue-500">Function and task</h2>
                <p className="mb-4">Danang Property Auction Service Center is a unit under the Department of Justice of Danang City, established under Decision No. 1540/QD-UB dated June 3, 1997 of the People's Committee of Danang City and was assigned new tasks under Decision No. 01/2006/QD-UBND dated January 4, 2006 of the People's Committee of Danang City.</p>
                <ul className="list-disc list-inside bg-white shadow-lg rounded-lg p-6 space-y-2">
                    <li>State assets according to the provisions of law on management and use of state assets;</li>
                    <li>Property is established as public ownership according to the provisions of law;</li>
                    <li>Assets subject to execution of judgments according to the provisions of law on civil execution;</li>
                    <li>Assets are national reserves according to the provisions of law on national reserves;</li>
                    <li>Assets are exhibits and means of administrative violations confiscated and added to the state budget;</li>
                    <li>Collateral according to the law on secured transactions Goods stored by sea, road, and air carriers in Vietnam;</li>
                    <li>Assets owned by individuals, organizations, and businesses that request to auction assets;</li>
                    <li>State assets must be auctioned according to the law on State asset management: liquidated assets of administrative agencies, public service units, liquidated assets of State enterprises, ...</li>
                </ul>
            </div>
        </div>
    );
};

export default About;