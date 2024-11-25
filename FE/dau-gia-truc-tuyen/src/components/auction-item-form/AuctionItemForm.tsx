import { Box, Button, Grid, MenuItem, TextField, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Controller, useForm } from 'react-hook-form';
import { getCategory, submitAuctionForm } from '../../queries/index';
import ContractModal, { AuctionItemFormData } from '../modal-contract/ContractModal';
import avt from '../../../public/2937095.png'

const AuctionItemForm: React.FC = () => {

  const [listCategory, setCategory] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  // Lấy danh sách danh mục
  const fetchListCategory = async () => {
    try {
      const response = await getCategory();
      if (response?.isSucceed) {
        setCategory(response?.result);
      } else {
        console.error('Failed to fetch categories');
      }
    } catch (error) {
      console.error('Error fetching categories:', error);
    } finally {
      setLoading(false);
    }
  };

  const { handleSubmit, control, formState: { errors } } = useForm<AuctionItemFormData>();
  const [previewImageAuction, setPreviewImageAuction] = useState('');
  const [previewImageVerification, setPreviewImageVerification] = useState('');
  const [previewSignatureImg, setPreviewSignatureImg] = useState('');
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formData, setFormData] = useState<AuctionItemFormData | null>(null);

  const handleImageClick = (field: any, setFieldValue: any) => {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/*';
    input.onchange = (e: any) => {
      const files = e.target.files;
      if (files && files[0]) {
        const reader = new FileReader();
        reader.onload = (event: any) => {
          setFieldValue(event.target.result);
          field.onChange(files[0]);
        };
        reader.readAsDataURL(files[0]);
      }
    };
    input.click();
  };

  useEffect(() => {
    fetchListCategory();
  }, []);

  const onSubmits = (data: AuctionItemFormData) => {
    setFormData(data);
    setIsModalOpen(true);
  };

  return (
    <Box sx={{ p: 4, height: '100%', backgroundColor: '#f9f9f9', borderRadius: 2, boxShadow: 3, top: '10', display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
      <Box sx={{ maxWidth: 600, width: '100%' }}>
        <Typography variant="h6" gutterBottom sx={{ fontWeight: 'bold', color: '#3f51b5', textAlign:'center', paddingBottom:'10px', marginTop:'50px'}}>
          Create Auction Item
        </Typography>
        <Box component="form" onSubmit={handleSubmit(onSubmits)} noValidate autoComplete="off">
          <Grid container spacing={3}>
            {/* Name Auction */}
            <Grid item xs={12}>
              <Controller
                name="nameAuction"
                control={control}
                rules={{ required: 'Name is required' }}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Name Auctioneer"
                    fullWidth
                    error={!!errors.nameAuction}
                    helperText={errors.nameAuction?.message}
                    sx={{
                      borderRadius: 1,
                      boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                      '.MuiInputBase-input': { padding: 1.5 },
                    }}
                  />
                )}
              />
            </Grid>

            {/* Description */}
            <Grid item xs={12}>
              <Controller
                name="description"
                control={control}
                rules={{ required: 'Description is required' }}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Description"
                    multiline
                    rows={4}
                    fullWidth
                    error={!!errors.description}
                    helperText={errors.description?.message}
                    sx={{
                      borderRadius: 1,
                      boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                      '.MuiInputBase-input': { padding: 1.5 },
                    }}
                  />
                )}
              />
            </Grid>

            {/* Starting Price */}
            <Grid item xs={6}>
              <Controller
                name="startingPrice"
                control={control}
                rules={{ required: 'Starting Price is required', min: 1 }}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Starting Price"
                    type="number"
                    fullWidth
                    error={!!errors.startingPrice}
                    helperText={errors.startingPrice?.message}
                    sx={{
                      borderRadius: 1,
                      boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                      '.MuiInputBase-input': { padding: 1.5 },
                    }}
                  />
                )}
              />
            </Grid>

            {/* Category */}
            <Grid item xs={6}>
              <Controller
                name="categoryID"
                control={control}
                rules={{ required: 'Category is required' }}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Category"
                    select
                    fullWidth
                    error={!!errors.categoryID}
                    helperText={errors.categoryID?.message}
                    disabled={loading}
                    sx={{
                      borderRadius: 1,
                      boxShadow: '0px 2px 10px rgba(0,0,0,0.1)',
                      '.MuiInputBase-input': { padding: 1.5 },
                    }}
                  >
                    {listCategory.map((category) => (
                      <MenuItem key={category.categoryID} value={category.categoryID}>
                        {category.nameCategory}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            {/* Image Auction */}
            <Grid item xs={4}>
              <Typography variant="subtitle1" gutterBottom>
                Image Auction
              </Typography>
              <Controller
                name="imageAuction"
                control={control}
                rules={{ required: 'Image is required' }}
                render={({ field }) => (
                  <div 
                    onClick={() => handleImageClick(field, setPreviewImageAuction)} 
                    className="w-full h-full cursor-pointer border border-gray-300 rounded flex items-center justify-center bg-gray-100">
                    <img 
                      style={{ height: '200px' }}
                      src={previewImageAuction ? previewImageAuction : avt} 
                      alt="Image Auction Preview" 
                      className="w-full h-full object-cover rounded" 
                    />
                  </div>
                )}
              />
            </Grid>

            {/* Image Verification */}
            <Grid item xs={4}>
              <Typography variant="subtitle1" gutterBottom>
                Image Verification
              </Typography>
              <Controller
                name="imageVerification"
                control={control}
                render={({ field }) => (
                  <div 
                    onClick={() => handleImageClick(field, setPreviewImageVerification)} 
                    className="w-full h-full cursor-pointer border border-gray-300 rounded flex items-center justify-center bg-gray-100">
                    <img 
                      style={{ height: '200px' }}
                      src={previewImageVerification ? previewImageVerification : avt} 
                      alt="Image Verification Preview" 
                      className="w-full h-full object-cover rounded" 
                    />
                  </div>
                )}
              />
            </Grid>

            {/* Signature Image */}
            <Grid item xs={4}>
              <Typography variant="subtitle1" gutterBottom>
                Signature Image
              </Typography>
              <Controller
                name="signatureImg"
                control={control}
                render={({ field }) => (
                  <div 
                    onClick={() => handleImageClick(field, setPreviewSignatureImg)} 
                    className="w-full h-full cursor-pointer border border-gray-300 rounded flex items-center justify-center bg-gray-100">
                    <img 
                      style={{ height: '200px' }}
                      src={previewSignatureImg ? previewSignatureImg : avt} 
                      alt="Signature Image Preview" 
                      className="w-full h-full object-cover rounded" 
                    />
                  </div>
                )}
              />
            </Grid>
            
            <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'center',marginTop:'40px', marginBottom:'50px' }}>
              <Button type="submit" variant="contained" color="primary">Create Auction Item</Button>
            </Grid>
          </Grid>
        </Box>

        {formData && (
          <ContractModal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)} formData={formData} />
        )}
      </Box>
    </Box>
  );
};

export default AuctionItemForm;
