import { Box, Button, Grid, MenuItem, TextField, Typography } from '@mui/material';
import React, { useEffect, useState } from 'react';
import { Controller, SubmitHandler, useForm } from 'react-hook-form';
import axios from 'axios';
import { getCategory } from '../../queries/index';

// Define the interface for the form data
interface AuctionItemFormData {
  nameAuction: string;
  description: string;
  startingPrice: number;
  categoryID: string;
  imageAuction: FileList | null;
  imageVerification: FileList | null;
  signatureImg: FileList | null;
}

const AuctionItemForm: React.FC = () => {
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<AuctionItemFormData>({
    defaultValues: {
      nameAuction: '',
      description: '',
      startingPrice: 0,
      categoryID: '',
      imageAuction: null,
      imageVerification: null,
      signatureImg: null,
    },
  });

  const [listCategory, setCategory] = useState<any[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

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
  useEffect(() => {
    fetchListCategory();
  }, []);

  const onSubmit: SubmitHandler<AuctionItemFormData> = async (data) => {
    try {
      const formData = new FormData();
      formData.append('nameAuction', data.nameAuction);
      formData.append('description', data.description);
      formData.append('startingPrice', data.startingPrice.toString());
      formData.append('categoryID', data.categoryID);

      if (data.imageAuction && data.imageAuction.length > 0) {
        formData.append('imageAuction', data.imageAuction[0]);
      }

      if (data.imageVerification && data.imageVerification.length > 0) {
        formData.append('imageVerification', data.imageVerification[0]);
      }

      if (data.signatureImg && data.signatureImg.length > 0) {
        formData.append('signatureImg', data.signatureImg[0]);
      }
      console.log('signatureImg', data.signatureImg);

      const response = await axios.post('/api/addAuctionItem', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      console.log('Auction item created successfully:', response.data);
      reset();
    } catch (error) {
      console.error('Error creating auction item:', error);
    }
  };

  return (
    <Box sx={{ p: 3, height: '90vh' }}>
      <Typography variant="h4" gutterBottom>
        Create Auction Item
      </Typography>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Grid container spacing={2}>
          {/* NameAuctioneer */}
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
                />
              )}
            />
          </Grid>

          {/* StartingPrice */}
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
                />
              )}
            />
          </Grid>

          {/* CategoryID */}
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
          <Grid item xs={6}>
            <Controller
              name="imageAuction"
              control={control}
              rules={{ required: 'Image is required' }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Image Auction"
                  type="file"
                  fullWidth
                  error={!!errors.imageAuction}
                  helperText={errors.imageAuction?.message}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  inputProps={{
                    accept: 'image/*',
                    onChange: (e: React.ChangeEvent<HTMLInputElement>) =>
                      field.onChange(e.target.files),
                  }}
                />
              )}
            />
          </Grid>

          {/* Image Verification */}
          <Grid item xs={6}>
            <Controller
              name="imageVerification"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Image Verification"
                  type="file"
                  fullWidth
                  InputLabelProps={{
                    shrink: true,
                  }}
                  inputProps={{
                    accept: 'image/*',
                    onChange: (e: React.ChangeEvent<HTMLInputElement>) =>
                      field.onChange(e.target.files),
                  }}
                />
              )}
            />
          </Grid>

          {/* Signature Image */}
          <Grid item xs={6}>
            <Controller
              name="signatureImg"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Signature Image"
                  type="file"
                  fullWidth
                  InputLabelProps={{
                    shrink: true,
                  }}
                  inputProps={{
                    accept: 'image/*',
                    onChange: (e: React.ChangeEvent<HTMLInputElement>) =>
                      field.onChange(e.target.files),
                  }}
                />
              )}
            />
          </Grid>

          {/* Submit Button */}
          <Grid item xs={12}>
            <Button type="submit" variant="contained" color="primary" fullWidth>
              Create Auction Item
            </Button>
          </Grid>
        </Grid>
      </form>
    </Box>
  );
};

export default AuctionItemForm;
