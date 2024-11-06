import React, { useState, useRef } from 'react';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';

interface CarouselDetailProps {
  imgList?: string; // Make this optional if it may be undefined
}

const CarouselDetail: React.FC<CarouselDetailProps> = ({ imgList }) => {
  
  // Image URLs (Replace with your actual image paths)
 const images = [
  `http://capstoneauctioneer.runasp.net/api/read?filePath=${imgList}`
];

  const [selectedIndex, setSelectedIndex] = useState(0); // Start with the first image
  const [isZoomed, setIsZoomed] = useState(false); // State to manage zoom modal visibility
  const sliderRef = useRef<HTMLDivElement>(null); // Reference for the slider container

  // Handler to change the displayed image
  const handleImageClick = (index: number) => {
    setSelectedIndex(index);
  };

  // Handler to open and close the zoom modal
  const toggleZoom = () => {
    setIsZoomed(!isZoomed);
  };

  // Handler for next and previous buttons
  const handleNext = () => {
    setSelectedIndex((prevIndex) => (prevIndex + 1) % images.length);
  };

  const handlePrevious = () => {
    setSelectedIndex((prevIndex) => (prevIndex - 1 + images.length) % images.length);
  };

  // Function to scroll the slider to the left
  const scrollLeft = () => {
    if (sliderRef.current) {
      sliderRef.current.scrollBy({
        left: -100, // Adjust scroll distance as needed
        behavior: 'smooth',
      });
    }
  };

  // Function to scroll the slider to the right
  const scrollRight = () => {
    if (sliderRef.current) {
      sliderRef.current.scrollBy({
        left: 100, // Adjust scroll distance as needed
        behavior: 'smooth',
      });
    }
  };

  return (
    <div className="container flex flex-col items-center">
      {/* Display selected image */}
      <div className="w-full h-auto mb-4">
        <img
          src={images[selectedIndex]}
          alt="Selected"
          className="w-full h-full object-contain cursor-pointer"
          onClick={toggleZoom} // Click to zoom
        />
      </div>

      {/* Slider with images */}
      <div className="relative w-full flex items-center">
        {/* Left scroll button */}
        {images.length > 4 && (
          <button
            className="absolute left-0 z-10 bg-transparent text-white p-2 rounded-full hover:bg-gray-300 hover:border-none focus:outline-none"
            onClick={scrollLeft}
          >
            <ChevronLeftIcon />
          </button>
        )}

        <div
          ref={sliderRef}
          className="flex space-x-4 overflow-x-auto w-full"
          style={{ scrollbarWidth: 'none' }} // Hide scrollbar in Firefox
        >
          {images.map((image, index) => (
            <img
              key={index}
              src={image}
              alt={`Thumbnail ${index + 1}`}
              className={`w-20 h-20 object-cover cursor-pointer rounded-md transition-all duration-300 focus:outline-dashed`}
              onClick={() => handleImageClick(index)}
            />
          ))}
        </div>

        {/* Right scroll button */}
        {images.length > 4 && (
          <button
            className="absolute right-0 z-10 bg-transparent text-white p-2 rounded-full hover:bg-gray-300 hover:border-none focus:outline-none"
            onClick={scrollRight}
          >
            <ChevronRightIcon />
          </button>
        )}
      </div>

      {/* Zoom Modal */}
      {isZoomed && (
        <div
          className="fixed inset-0 flex justify-center items-center bg-black bg-opacity-75 z-50"
          onClick={toggleZoom} // Close modal on outside click
        >
          {/* Image Display */}
          <img
            src={images[selectedIndex]}
            alt="Zoomed"
            className="max-w-full max-h-full object-contain"
            onClick={(e) => e.stopPropagation()} // Prevent closing when clicking on the image
          />

          {/* Navigation Buttons */}
          <button
            className="absolute left-4 focus:outline-none text-white text-2xl p-2 rounded-full bg-transparent hover:bg-gray-700 cursor-pointer"
            onClick={(e) => {
              e.stopPropagation();
              handlePrevious();
            }}
          >
            <ChevronLeftIcon />
          </button>
          <button
            className="absolute right-4 focus:outline-none text-white text-2xl p-2 rounded-full bg-transparent hover:bg-gray-700 cursor-pointer"
            onClick={(e) => {
              e.stopPropagation();
              handleNext();
            }}
          >
            <ChevronRightIcon />
          </button>
        </div>
      )}
    </div>
  );
};

export default CarouselDetail;
