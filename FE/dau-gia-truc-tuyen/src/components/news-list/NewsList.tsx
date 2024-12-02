import { Typography, Button } from '@mui/material';
import CardList from '../../common/card-list/CardList';
import { useState, useEffect } from 'react';

const NewsList = () => {
  const cards = Array.from({ length: 34 }, (_, index) => ({
    id: index + 1, // Assuming the id is just the index + 1
  }));

  const [currentPage, setCurrenPage] = useState(1);
  const pageSize = 4;
  const [newsList, setNewsList] = useState<
    { article_id: string; title: string; image_url: string; description: string; link: string }[]
  >([]);

  const totalPages = Math.ceil(cards.length / pageSize);

  const currentCards = cards.slice((currentPage - 1) * pageSize, currentPage * pageSize);

  useEffect(() => {
    const fetchNews = async () => {
      try {
        const response = await fetch(
          'https://newsdata.io/api/1/news?apikey=pub_4058515151c616834f37ed50f02107c4e64cf&country=vi&language=vi&category=business'
        );
        if (!response.ok) {
          throw new Error('Failed to fetch news');
        }
        const data = await response.json();

        // Filter news items where "business" exists in the category array
        const filteredNews = data.results.filter((item: any) =>
          item.category?.includes('business')
        );

        setNewsList(filteredNews);
      } catch (error) {
        console.error('Error fetching news:', error);
      }
    };

    fetchNews();
  }, []);

  console.log(newsList, 'newsList');

  const handlePrev = () => {
    if (currentPage > 1) {
      setCurrenPage((prev) => prev - 1);
    }
  };

  const handleNext = () => {
    if (currentPage < totalPages) {
      setCurrenPage((prev) => prev + 1);
    }
  };

  const renderPaginationBtn = () => {
    const btns = [];
    const startPage = Math.max(1, currentPage - 1);
    const endPage = Math.min(totalPages, currentPage + 1);

    for (let i = startPage; i <= endPage; i++) {
      btns.push(
        <button
          key={i}
          type="button"
          className={` min-h-[38px] min-w-[38px] flex justify-center items-center border py-2 px-3 text-sm rounded-lg focus:outline-none ${
            i === currentPage
              ? 'border-blue-600 text-blue-600 bg-blue-100'
              : 'border-gray-200 text-gray-800 hover:bg-gray-100'
          }`}
          onClick={() => setCurrenPage(i)}
        >
          {i}
        </button>
      );
    }

    return btns;
  };

  console.log(currentCards, 'currentCards');

  return (
    <>
      <div className="container mx-auto p-6">
        <h2 className="text-2xl font-bold">TIN TỨC MỚI NHẤT</h2>
        <div className="grid grid-cols-4 gap-4 mt-4">
          {newsList.length > 0 ? (
            newsList.slice(0, 8).map((card) => (
              <CardList
                isProperties={false}
                key={card.article_id?.toString()} // Use title as fallback if id is missing
                id={card.article_id?.toString() || ''}
                title={card.title || 'No Title'}
                imgSrc={card.image_url || 'default-image.png'} // Provide a default image URL
                description={card.description || 'No description available.'}
                link={card.link}
                priceStart={''}
                endDay={''}
                startDay={''}
                endTime={''}
                targetDate={new Date()}
                isApproved={''}
                url={''}
              />
            ))
          ) : (
            <p className="col-span-4 text-center">No news available.</p>
          )}
        </div>
        {/* <div className="flex justify-center gap-4 mt-4">
                <button 
                  type="button" 
                  className="min-h-[38px] min-w-[38px] py-2 px-2.5 inline-flex justify-center items-center gap-x-2 text-sm rounded-lg border border-transparent text-gray-800 hover:bg-gray-100 focus:outline-none focus:bg-gray-100 disabled:opacity-50 disabled:pointer-events-none dark:border-transparent dark:text-white dark:hover:bg-white/10 dark:focus:bg-white/10" 
                  aria-label="Previous"
                  onClick={handlePrev}
                  disabled={currentPage === 1}
                  >
                  <svg className="shrink-0 size-3.5" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <path d="m15 18-6-6 6-6"></path>
                  </svg>
                </button>
                <div className="flex items-center gap-x-1">
                  {renderPaginationBtn()} 
                </div>
                <button 
                  type="button" 
                  className="min-h-[38px] min-w-[38px] py-2 px-2.5 inline-flex justify-center items-center gap-x-2 text-sm rounded-lg border border-transparent text-gray-800 hover:bg-gray-100 focus:outline-none focus:bg-gray-100 disabled:opacity-50 disabled:pointer-events-none dark:border-transparent dark:text-white dark:hover:bg-white/10 dark:focus:bg-white/10" 
                  aria-label="Next"
                  onClick={handleNext}
                  disabled={currentPage === totalPages}
                  >
                  <svg className="shrink-0 size-3.5" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <path d="m9 18 6-6-6-6"></path>
                  </svg>
                </button>
        </div> */}
      </div>
    </>
  );
};
export default NewsList;
