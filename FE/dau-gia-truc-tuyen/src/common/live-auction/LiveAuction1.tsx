import CardList1 from '@common/card-list/CardList1';
import React, { useRef } from 'react';
import Slider from 'react-slick';

const LiveAuction1 = () => {
    const cards = Array.from({ length: 8 }, (_, index) => ({
        id: index + 1,
    }));

    const sliderRef = useRef<Slider| null>(null); 

    const settings = {
        dots: false,
        infinite: true,
        speed: 500,
        slidesToShow: 4, 
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 3000,
        cssEase: 'linear',
        responsive: [
            {
                breakpoint: 640, 
                settings: {
                    slidesToShow: 2, 
                },
            },
        ],
    };

    return (
        <div className="max-w-7xl mx-auto relative pt-20">
            <div className="absolute top-0 right-0 flex justify-between w-full">
                <div className='flex-none'>
                    <h2>LIVE</h2>
                </div>
                <div className='flex space-x-2'>
                    <button
                        className="bg-white text-gray-800 p-2 rounded-full border border-black hover:bg-black hover:text-white flex items-center justify-center"
                        onClick={() => sliderRef.current?.slickPrev()} 
                    >
                        <svg width="10px" height="10px" viewBox="0 0 48 48" version="1" xmlns="http://www.w3.org/2000/svg" enable-background="new 0 0 48 48" className="transition-colors duration-300">
                            <polygon fill="currentColor" points="30.9,43 34,39.9 18.1,24 34,8.1 30.9,5 12,24"/>
                        </svg>
                    </button>
                    <button
                        className="bg-white text-gray-800 p-2 rounded-full border border-black hover:bg-black hover:text-white flex items-center justify-center"
                        onClick={() => sliderRef.current?.slickNext()} 
                    >
                        <svg width="10px" height="10px" viewBox="0 0 48 48" version="1" xmlns="http://www.w3.org/2000/svg" enable-background="new 0 0 48 48">
                            <polygon fill="currentColor" points="17.1,5 14,8.1 29.9,24 14,39.9 17.1,43 36,24"/>
                        </svg>
                    </button>
                </div>
            </div>
            <Slider ref={sliderRef} {...settings}>
                {cards.map((product) => (
                    <div key={product.id} className="flex-none w-1/4 px-2">
                        <CardList1 />
                    </div>
                ))}
            </Slider>
        </div>
    );
};

export default LiveAuction1;
