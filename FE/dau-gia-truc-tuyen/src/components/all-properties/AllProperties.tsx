import CardList from '../../common/card-list/CardList';

const AllProperties = (props: object) => {
  const cards = Array.from({ length: 12 }, (_, index) => ({
    id: index + 1, // Assuming the id is just the index + 1
  }));

  return (
    <div className="grid grid-cols-4 gap-4">
      {cards.map((card) => (
        <CardList
        id={card.id.toString()} 
        isProperties 
        key={card.id} />
      ))}
    </div>
  );
};
export default AllProperties;
