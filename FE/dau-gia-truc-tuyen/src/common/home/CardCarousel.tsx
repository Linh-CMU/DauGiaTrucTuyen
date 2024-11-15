import React, { useState, useEffect } from 'react';
import Slider from 'react-slick';

const CardCarousel = () => {
  const [reverse, setReverse] = useState(false);
  const cards = [
    { id: 1, title: "Card 1", content: "Content for Card 1" },
    { id: 2, title: "Card 2", content: "Content for Card 2" },
    { id: 3, title: "Card 3", content: "Content for Card 3" },
    { id: 4, title: "Card 4", content: "Content for Card 4" },
  ];

  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    autoplay: true,
    autoplaySpeed: 3000,
    slidesToShow: 1,
    slidesToScroll: 1,
    beforeChange: (current, next) => {
      // Kiểm tra hướng di chuyển
      if (next === cards.length - 1 && !reverse) {
        setReverse(true);
      } else if (next === 0 && reverse) {
        setReverse(false);
      }
    },
    // Sử dụng reverse direction khi đến cuối danh sách
    slidesToShow: 1,
    swipeToSlide: true,
    adaptiveHeight: true,
    vertical: reverse, // Bật chế độ di chuyển theo chiều dọc
    verticalSwiping: reverse,
  };

  return (
    <div>
      <Slider {...settings}>
        {cards.map(card => (
          <div key={card.id} className="p-4 bg-gray-200 rounded-lg">
            <h3 className="font-bold">{card.title}</h3>
            <p>{card.content}</p>
          </div>
        ))}
      </Slider>
    </div>
  );
};

export default CardCarousel;
