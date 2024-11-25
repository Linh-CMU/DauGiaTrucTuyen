import { UpdatePayment } from '../queries/AuctionAPI';
import React, { useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';

const CancelPage = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const queryParams = new URLSearchParams(location.search);
  const id = queryParams.get('orderCode');
  const status = queryParams.get('status');
  const [isFetch, setIsFetch] = useState(true);

  const handleReturnHome = () => {
    navigate('/', {state : {data: '4'}});
  };

  useEffect(() => {
    if (isFetch) {
      const updatePaymentStatus = async () => {
        try {
          if (id && status === 'CANCELLED') {
            await UpdatePayment(Number(id), 'cancel');
          }
        } catch (error) {
          console.error('Lỗi khi cập nhật trạng thái thanh toán', error);
        }
      };

      updatePaymentStatus();
      setIsFetch(false);
    }
  }, [id, status, isFetch]);

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-gray-100 p-6">
      <h1 className="text-4xl font-semibold text-red-600 mb-4">Thanh toán đã bị hủy!</h1>
      <p className="text-lg text-gray-700 mb-8 text-center max-w-lg">
        Giao dịch thanh toán của bạn đã bị hủy. Nếu có thắc mắc, vui lòng liên hệ với chúng tôi để được hỗ trợ.
      </p>
      <button
        onClick={handleReturnHome}
        className="px-6 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600 transition duration-300"
      >
        Quay về trang chủ
      </button>
    </div>
  );
};

export default CancelPage;
