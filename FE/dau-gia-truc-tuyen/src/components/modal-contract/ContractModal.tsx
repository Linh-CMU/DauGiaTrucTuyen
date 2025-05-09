// ContractModal.tsx
import React, { useEffect, useState } from 'react';
import { Box, Button, Grid, Modal, Typography } from '@mui/material';
import {
  cityResponse,
  districtResponse,
  profileResponse,
  wardResponse,
} from '../../types/auth.type';
import { getCity, getDistrict, getWard, inforUser, profileUser } from '../../queries/AdminAPI';
import { submitAuctionForm } from '../../queries/AuctionAPI';
import { useNavigate } from 'react-router-dom';
import { useMessage } from '@contexts/MessageContext';
import sign from '../../../public/tao-chu-ky-dep-theo-ten.jpg'

export interface AuctionItemFormData {
  nameAuction: string;
  description: string;
  startingPrice: number;
  categoryID: string;
  imageAuction: File | null;
  imageVerification: File | null;
}

interface ContractModalProps {
  isOpen: boolean;
  onClose: () => void;
  formData?: AuctionItemFormData;
  detailAuction?: any;
}

const ContractModal: React.FC<ContractModalProps> = ({
  isOpen,
  onClose,
  formData,
  detailAuction,
}) => {
  const [profile, setProfile] = useState<profileResponse | null>();
  const { setErrorMessage, setSuccessMessage } = useMessage();
  const [filteredDistrict, setFilteredDistrict] = useState<districtResponse[]>([]);
  const [filteredWards, setFilteredWards] = useState<cityResponse[]>([]);
  const [locationData, setLocationData] = useState({
    cities: [] as cityResponse[],
    districts: [] as districtResponse[],
    wards: [] as wardResponse[],
  });
  const navigate = useNavigate();
  useEffect(() => {
    const fetchData = async () => {
      try {
        if (detailAuction) {
          const response = await inforUser(detailAuction.user.accountId);
          setProfile(response.result);
        } else {
          const response = await profileUser();
          console.log(response.result);
          setProfile(response.result);
        }
      } catch (error) {}
    };
    fetchData();
  }, []);
  const onSubmit = async () => {
    try {
      const response = await submitAuctionForm(formData);
      if (response.isSucceed) {
        setSuccessMessage('You have successfully created a product.');
        navigate('/listYourAuction');
        onClose();
      }
    } catch (error) {
      setErrorMessage('Error creating auction item: ' + error);
    }
  };
  const fetchData = async () => {
    try {
      const [cityData, districtData, wardData] = await Promise.all([
        getCity(),
        getDistrict(),
        getWard(),
      ]);
      setLocationData({ cities: cityData, districts: districtData, wards: wardData });
    } catch (error) {
      setErrorMessage('Error fetching data');
    }
  };

  useEffect(() => {
    fetchData();
  }, []);
  useEffect(() => {
    if (profile) {
      const districts = locationData.districts.filter(
        (district) => district.code.toString() === profile.city
      );
      const wards = locationData.cities.filter((ward) => ward.code.toString() === profile.city);

      setFilteredDistrict(districts);
      setFilteredWards(wards);
    }
  }, [profile, locationData]);
  return (
    <Modal
      open={isOpen}
      onClose={onClose}
      aria-labelledby="contract-modal-title"
      aria-describedby="contract-modal-description"
    >
      <Box
        sx={{
          width: '80%',
          maxWidth: 700,
          maxHeight: '80vh', // Giới hạn chiều cao của modal
          p: 4,
          backgroundColor: 'white',
          borderRadius: 2,
          boxShadow: 3,
          margin: 'auto',
          mt: '5vh',
          overflow: 'auto', // Cho phép cuộn khi nội dung dài
        }}
      >
        <Typography
          variant="body1"
          align="center"
          sx={{ mb: 2, fontWeight: 'bold', textTransform: 'uppercase' }}
        >
          Cộng hòa Xã hội Chủ nghĩa Việt Nam
        </Typography>
        <Typography
          variant="body1"
          align="center"
          sx={{ mb: 4, fontWeight: 'bold', textTransform: 'uppercase' }}
        >
          Độc lập - Tự do - Hạnh phúc
        </Typography>
        <Typography
          id="contract-modal-title"
          variant="h6"
          sx={{ mb: 2, fontWeight: 'bold', textAlign: 'center' }}
        >
          HỢP ĐỒNG ĐẤU GIÁ
        </Typography>
        <Typography sx={{ mb: 2, textAlign: 'center' }}>
          Số hợp đồng: #123456 - Ngày lập: {detailAuction ? detailAuction.createDate : new Date().toLocaleDateString()}
        </Typography>

        {/* Thông tin các bên */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Thông Tin Các Bên
        </Typography>
        <Typography variant="body1" sx={{ mb: 1 }}>
          <strong>Bên A (Bên tạo đấu giá):</strong> Công ty Đấu giá trực tuyến
          <br />
          Địa chỉ: Tổ 5, phường Hòa Thọ Tây, Quận Cẩm Lệ, Thành Phố Đà Nẵng
          <br />
          Số điện thoại: 0779460350
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          <strong>Bên B (Bên tham gia đấu giá):</strong>{' '}
          {detailAuction ? detailAuction.nameAuction : formData?.nameAuction}
          <br />
          Địa chỉ: {filteredWards[0]?.name}-{filteredDistrict[0]?.name}-{profile?.ward}-
          {profile?.city}
          <br />
          Số điện thoại: {profile?.phone}
        </Typography>

        {/* Điều khoản hợp đồng */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Điều Khoản Hợp Đồng
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          1. <strong>Sản phẩm đấu giá:</strong>{' '}
          {detailAuction ? detailAuction.nameAuction : formData?.nameAuction}
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          2. <strong>Mô tả sản phẩm:</strong>{' '}
          {detailAuction ? detailAuction.description : formData?.description}
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          3. <strong>Starting price:</strong>{' '}
          {detailAuction
            ? detailAuction.startingPrice?.toLocaleString()
            : formData?.startingPrice?.toLocaleString()}{' '}
          VNĐ
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          4. <strong>Thời gian đấu giá:</strong> Ngày bắt đầu - Ngày kết thúc sẽ được cập nhật khi
          tạo sản phẩm
        </Typography>

        {/* Quyền và nghĩa vụ của các bên */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Quyền và Nghĩa Vụ Của Các Bên
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          5. <strong>Bên A:</strong> Kiểm tra và đăng sản phẩm theo đúng thời gian quy định, đảm bảo
          đăng đúng theo nội dung bên B đã cung cấp. Đảm bảo tính minh bạch và bảo mật cho thông tin
          bên B.
          <br />
          6. <strong>Bên B:</strong> Cung cấp đầy đủ thông tin về sản phẩm đấu giá, thẩm định chính
          xác và rõ ràng, không có hành vi gian lận và tuân thủ theo các điều kiện trong hợp đồng .
        </Typography>

        {/* Thời hạn hợp đồng */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Thời Hạn Hợp Đồng
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          Hợp đồng có hiệu lực start date bắt đầu đấu giá và kết thúc khi sản phẩm đấu giá được bán
          hoặc khi có thông báo từ hệ thống.
        </Typography>

        {/* Chữ ký */}
        <Grid container spacing={2} sx={{ mt: 4 }}>
          <Grid item xs={6}>
            <Typography variant="body1" align="center" sx={{ mb: 2 }}>
              <strong>Đại diện Bên A</strong>
            </Typography>
            <Box
              sx={{
                border: '1px solid #000',
                height: '100px',
                width: '100%',
                textAlign: 'center',
                p: 2,
              }}
            >
              <img
                src={sign}
                alt="Signature"
                style={{
                  width: '100px',
                  height: '50px',
                  marginLeft: 'auto',
                  marginRight: 'auto',
                }}
                className="signature-image"
              />
              <p className="font-bold">Phòng đấu giá trực tuyến</p>
            </Box>
          </Grid>
          <Grid item xs={6}>
            <Typography variant="body1" align="center" sx={{ mb: 2 }}>
              <strong>Đại diện Bên B</strong>
            </Typography>
            <Box
              sx={{
                border: '1px solid #000',
                height: '100px',
                width: '100%',
                textAlign: 'center',
                p: 2,
              }}
            >
              Chữ ký người tham gia
              <img
                src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${profile?.signature}`}
                alt="Signature"
                style={{
                  width: '100px',
                  height: '50px',
                  marginLeft: 'auto',
                  marginRight: 'auto',
                }}
                className="signature-image"
              />
            </Box>
          </Grid>
        </Grid>

        {/* Nút xác nhận */}
        <Grid container spacing={2} sx={{ mt: 4 }}>
          {detailAuction ? (
            <>
            </>
          ) : (
            <>
              <Grid item xs={6}>
                <Button onClick={onClose} variant="outlined" color="secondary" fullWidth>
                  Cancel
                </Button>
              </Grid>
              <Grid item xs={6}>
                <Button onClick={() => onSubmit()} variant="contained" color="primary" fullWidth>
                  Accept
                </Button>
              </Grid>
            </>
          )}
        </Grid>
      </Box>
    </Modal>
  );
};

export default ContractModal;
