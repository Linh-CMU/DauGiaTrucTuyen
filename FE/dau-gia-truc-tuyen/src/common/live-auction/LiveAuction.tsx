import CardList1 from "@common/card-list/CardList1";
import LiveAuction1 from "./LiveAuction1";


const LiveAuction = () => {
    const cards = Array.from({ length: 8 }, (_, index) => ({
        id: index + 1, // Assuming the id is just the index + 1
      }));
    return (
        <>  
            <LiveAuction1 />
            <div className="container mx-auto p-6">
                <div className="text-center">
                    <h2 className="text-center text-3xl font-bold">PHIEN DAU GIA MOI</h2>
                    <h6 className="font-light pt-3 mb-12">Feel free adapt this based on the specific managed services, features</h6>
                </div>
                <div className="grid grid-cols-4 gap-4">
                {cards?.map((card) => (
                    <CardList1 />
                ))}
                </div>
                <div className="flex justify-center mt-14">
                <button className="relative overflow-hidden bg-white border-sky-700 border-r-4 border-b-4 text-black font-bold py-3 px-6  transition duration-300 ease-in-out group">
                <span className="absolute inset-0 bg-blue-700 transform translate-x-full transition duration-300 ease-in-out group-hover:translate-x-0" />
                <span className="relative z-10">VIEW ALL AUCTION</span>
            </button>
                </div>
            </div>

            
        </>
    )
}

export default LiveAuction;