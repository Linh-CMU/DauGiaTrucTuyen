import { Box, Button, Grid, MenuItem, TextField, Typography } from '@mui/material';
import React from 'react';
import { Controller, SubmitHandler, useForm } from 'react-hook-form';
import axios from 'axios';

// Define the interface for the form data
interface AuctionItemFormData {
  nameAuctioneer: string;
  description: string;
  startingPrice: number;
  categoryID: string;
  image: FileList | null;
  file: FileList | null;
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
      nameAuctioneer: '',
      description: '',
      startingPrice: 0,
      categoryID: '',
      image: null,
      file: null,
      signatureImg: null,
    },
  });

  const onSubmit: SubmitHandler<AuctionItemFormData> = async (data) => {
    try {
      // Create a FormData object to handle file uploads
      const formData = new FormData();
      formData.append('nameAuctioneer', data.nameAuctioneer);
      formData.append('description', data.description);
      formData.append('startingPrice', data.startingPrice.toString());
      formData.append('categoryID', data.categoryID);

      // Add files to FormData if they exist
      if (data.image && data.image.length > 0) {
        formData.append('image', data.image[0]);
      }

      if (data.file && data.file.length > 0) {
        formData.append('file', data.file[0]);
      }

      if (data.signatureImg && data.signatureImg.length > 0) {
        formData.append('signatureImg', data.signatureImg[0]);
      }

      // Send the form data to the API endpoint
      const response = await axios.post('/api/auction-items', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      // Handle success response
      console.log('Auction item created successfully:', response.data);

      // Reset the form after successful submission
      reset();
    } catch (error) {
      // Handle error response
      console.error('Error creating auction item:', error);
    }
  };

  return (
    <Box sx={{ p: 3, height: "90vh" }}>
      <Typography variant="h4" gutterBottom>
        Create Auction Item
      </Typography>
      <form onSubmit={handleSubmit(onSubmit)}>
        <Grid container spacing={2}>
          {/* NameAuctioneer */}
          <Grid item xs={12}>
            <Controller
              name="nameAuctioneer"
              control={control}
              rules={{ required: 'Name is required' }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Name Auctioneer"
                  fullWidth
                  error={!!errors.nameAuctioneer}
                  helperText={errors.nameAuctioneer?.message}
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
              rules={{ required: 'Category ID is required' }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Category ID"
                  select
                  fullWidth
                  error={!!errors.categoryID}
                  helperText={errors.categoryID?.message}
                >
                  <MenuItem value="1">Category 1</MenuItem>
                  <MenuItem value="2">Category 2</MenuItem>
                </TextField>
              )}
            />
          </Grid>

          {/* Image */}
          <Grid item xs={6}>
            <Controller
              name="image"
              control={control}
              rules={{ required: 'Image is required' }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Image"
                  type="file"
                  fullWidth
                  error={!!errors.image}
                  helperText={errors.image?.message}
                  InputLabelProps={{
                    shrink: true,
                  }}
                  inputProps={{
                    accept: 'image/*',
                  }}
                />
              )}
            />
          </Grid>

          {/* File */}
          <Grid item xs={6}>
            <Controller
              name="file"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="File"
                  type="file"
                  fullWidth
                  InputLabelProps={{
                    shrink: true,
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