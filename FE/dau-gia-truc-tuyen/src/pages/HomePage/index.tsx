import SearchBox from "../../components/SearchBox";

const HomePage = () => {
  return (
    <>
      <div className="flex flex-col">
        <div className="w-full h-[450px] relative">
          <img
            src="banner.jpg"
            alt="banner-img"
            className="object-cover h-full w-full"
          />
          <SearchBox />
        </div>
        <div className="p-14">body here</div>
      </div>
    </>
  );
};
export default HomePage;
