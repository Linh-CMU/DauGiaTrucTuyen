import { Typography } from '@mui/material';
import CardList from '../../common/card-list/CardList';

const NewsList = () => {
   const cards = Array.from({ length: 12 }, (_, index) => ({
    id: index + 1, // Assuming the id is just the index + 1
  }));
  return (
    <>
      <Typography variant="h5" component="h2" fontWeight="bold">
        TIN TỨC MỚI NHẤT
      </Typography>
      <div className="grid grid-cols-4 gap-4 p-6">
        {cards.map((card) => (
          <CardList isProperties={false} key={card.id} id={card.id.toString()} />
        ))}
      </div>
    </>
  );
};
export default NewsList;
