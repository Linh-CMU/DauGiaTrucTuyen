import { Box, Button, Grid, MenuItem, TextField, Typography } from '@mui/material';
import { DatePicker, LocalizationProvider, TimePicker } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import React from 'react';
import { Controller, SubmitHandler, useForm } from 'react-hook-form';

// Define the interface for the form data
interface AuctionItemFormData {
  nameAuctioneer: string;
  description: string;
  startingPrice: number;
  categoryID: string;
  startDay: Date | null;
  startTime: Date | null;
  endDay: Date | null;
  endTime: Date | null;
  priceStep: number;
  image: FileList | null;
  file: FileList | null;
  signatureImg: FileList | null;
}

const AuctionItemForm: React.FC = () => {
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<AuctionItemFormData>({
    defaultValues: {
      nameAuctioneer: '',
      description: '',
      startingPrice: 0,
      categoryID: '',
      startDay: null,
      startTime: null,
      endDay: null,
      endTime: null,
      priceStep: 0,
      image: null,
      file: null,
      signatureImg: null,
    },
  });

  const onSubmit: SubmitHandler<AuctionItemFormData> = (data) => {
    console.log(data);
    // Handle form submission, e.g., sending data to an API
  };

  return (
    <Box sx={{ p: 3 }}>
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
                  {/* Example category options */}
                  <MenuItem value="1">Category 1</MenuItem>
                  <MenuItem value="2">Category 2</MenuItem>
                </TextField>
              )}
            />
          </Grid>

          {/* StartDay */}
          <Grid item xs={6}>
            <Controller
              name="startDay"
              control={control}
              rules={{ required: 'Start Day is required' }}
              render={({ field }) => (
                <LocalizationProvider dateAdapter={AdapterDayjs}>
                  <DatePicker {...field} label="Start Day" />
                </LocalizationProvider>
              )}
            />
          </Grid>

          {/* StartTime */}
          <Grid item xs={6}>
            <Controller
              name="startTime"
              control={control}
              rules={{ required: 'Start Time is required' }}
              render={({ field }) => (
                <LocalizationProvider dateAdapter={AdapterDayjs}>
                  <TimePicker {...field} label="Start Time" />
                </LocalizationProvider>
              )}
            />
          </Grid>

          {/* EndDay */}
          <Grid item xs={6}>
            <Controller
              name="endDay"
              control={control}
              rules={{ required: 'End Day is required' }}
              render={({ field }) => (
                <LocalizationProvider dateAdapter={AdapterDayjs}>
                  <DatePicker {...field} label="End Day" />
                </LocalizationProvider>
              )}
            />
          </Grid>

          {/* EndTime */}
          <Grid item xs={6}>
            <Controller
              name="endTime"
              control={control}
              rules={{ required: 'End Time is required' }}
              render={({ field }) => (
                <LocalizationProvider dateAdapter={AdapterDayjs}>
                  <TimePicker {...field} label="End Time" />
                </LocalizationProvider>
              )}
            />
          </Grid>

          {/* PriceStep */}
          <Grid item xs={6}>
            <Controller
              name="priceStep"
              control={control}
              rules={{ required: 'Price Step is required', min: 1 }}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Price Step"
                  type="number"
                  fullWidth
                  error={!!errors.priceStep}
                  helperText={errors.priceStep?.message}
                />
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
