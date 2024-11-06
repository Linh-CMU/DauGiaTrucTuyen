// ContractModal.tsx
import React, { useEffect, useState } from 'react';
import { Box, Button, Grid, Modal, Typography } from '@mui/material';
import { profileResponse } from '../../types/auth.type';
import { profileUser } from '../../queries/AdminAPI';
import { submitAuctionForm } from '../../queries/AuctionAPI';

export interface AuctionItemFormData {
  nameAuction: string;
  description: string;
  startingPrice: number;
  categoryID: string;
  imageAuction: File | null;
  imageVerification: File | null;
  signatureImg: File | null;
}

interface ContractModalProps {
  isOpen: boolean;
  onClose: () => void;
  formData: AuctionItemFormData;
}

const ContractModal: React.FC<ContractModalProps> = ({ isOpen, onClose, formData }) => {
  const signatureImgUrl = formData.signatureImg ? URL.createObjectURL(formData.signatureImg) : null;
  const [profile, setProfile] = useState<profileResponse | null>();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await profileUser();
        console.log(response.result);
        setProfile(response.result);
      } catch (error) {}
    };
    fetchData();
  }, []);
  const onSubmit = async () => {
    try {
      const response = await submitAuctionForm(formData);
      console.log('Auction item created successfully:', response);
      onClose();
    } catch (error) {
      console.error('Error creating auction item:', error);
    }
  };
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
          id="contract-modal-title"
          variant="h6"
          sx={{ mb: 2, fontWeight: 'bold', textAlign: 'center' }}
        >
          HỢP ĐỒNG ĐẤU GIÁ
        </Typography>
        <Typography sx={{ mb: 2, textAlign: 'center' }}>
          Số hợp đồng: #123456 - Ngày lập: {new Date().toLocaleDateString()}
        </Typography>

        {/* Thông tin các bên */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Thông Tin Các Bên
        </Typography>
        <Typography variant="body1" sx={{ mb: 1 }}>
          <strong>Bên A (Bên tạo đấu giá):</strong> Công ty XYZ
          <br />
          Địa chỉ: Số 123, Đường ABC, Quận 1, TP.HCM
          <br />
          Số điện thoại: 0123 456 789
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          <strong>Bên B (Bên tham gia đấu giá):</strong> {formData?.nameAuction}
          <br />
          Địa chỉ: {profile?.address}-{profile?.district}-{profile?.ward}-{profile?.city}
          <br />
          Số điện thoại: {profile?.phone}
        </Typography>

        {/* Điều khoản hợp đồng */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Điều Khoản Hợp Đồng
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          1. <strong>Sản phẩm đấu giá:</strong> {formData?.nameAuction}
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          2. <strong>Mô tả sản phẩm:</strong> {formData?.description}
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          3. <strong>Giá khởi điểm:</strong> {formData?.startingPrice?.toLocaleString()} VNĐ
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
          5. <strong>Bên A:</strong> Cung cấp đầy đủ thông tin về sản phẩm đấu giá, đảm bảo tính
          minh bạch và bảo mật cho thông tin của Bên B.
          <br />
          6. <strong>Bên B:</strong> Cam kết tham gia đấu giá đúng theo quy định, không có hành vi
          gian lận và tuân thủ các điều kiện trong hợp đồng.
        </Typography>

        {/* Thời hạn hợp đồng */}
        <Typography variant="h6" sx={{ mt: 2, mb: 1 }}>
          Thời Hạn Hợp Đồng
        </Typography>
        <Typography variant="body1" sx={{ mb: 2 }}>
          Hợp đồng có hiệu lực từ ngày bắt đầu đấu giá và kết thúc khi sản phẩm đấu giá được bán
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
              Chữ ký và đóng dấu
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
              {signatureImgUrl ? (
                <img
                  src={signatureImgUrl}
                  alt="Signature"
                  style={{
                    width: '100px',
                    height: '50px',
                    marginLeft: 'auto',
                    marginRight: 'auto',
                  }}
                  className="signature-image"
                />
              ) : (
                <p>Chưa có chữ ký</p>
              )}
            </Box>
          </Grid>
        </Grid>

        {/* Nút xác nhận */}
        <Grid container spacing={2} sx={{ mt: 4 }}>
          <Grid item xs={6}>
            <Button onClick={onClose} variant="outlined" color="secondary" fullWidth>
              Hủy
            </Button>
          </Grid>
          <Grid item xs={6}>
            <Button
              onClick={() => onSubmit()}
              variant="contained"
              color="primary"
              fullWidth
            >
              Đồng ý
            </Button>
          </Grid>
        </Grid>
      </Box>
    </Modal>
  );
};

export default ContractModal;
