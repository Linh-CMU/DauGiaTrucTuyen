import { listData } from '../../types/auth.type';
import { joinRoomAuction, listOfRegisteredbidders } from '../../queries/AdminAPI';
import { useEffect, useState } from 'react';
import Room from './Room';
import { useNavigate } from 'react-router-dom';

const Test = () => {
    const [openIndex, setOpenIndex] = useState<number | null>(null);
    const [list, setList] = useState<listData[]>([]);
    const [socket, setSocket] = useState<WebSocket | null>(null);
    const [price, setPrice] = useState<number | null>(null);
    const [idJohnRoom, setIdJohnRoom] = useState<string | null>(null);
    const navigate = useNavigate();

    const toggleAccordion = (index: number, id: string) => {
        setOpenIndex(openIndex === index ? null : index);

        if (socket) {
            socket.close();
        }

        if (openIndex != index) {
            const newSocket = new WebSocket(`ws://capstoneauctioneer.runasp.net/api/viewBidHistory?id=${id}`);
            setSocket(newSocket);
            newSocket.onopen = () => console.log(`WebSocket connection opened for ID: ${id}`);
            newSocket.onmessage = (event) => {
                const data = JSON.parse(event.data);

                if (Array.isArray(data) && data.length > 0) {
                    const latestPrice = data[0].Price;
                    setPrice(latestPrice);
                    console.log(`Message from server - Latest Price: ${latestPrice}`);
                } else {
                    console.log("Data is not in expected array format or is empty.");
                }
            }
            newSocket.onclose = () => console.log(`WebSocket connection closed for ID: ${id}`);
        }
    };

    const handleJohnRoom = async (id: string) => {
        setIdJohnRoom(id);
        navigate(`/room/${id}`);
    }

    useEffect(() => {
        const fetchList = async () => {
            try {
                const resultData = await listOfRegisteredbidders();
                setList(resultData); 
                console.log(resultData);
            } catch (error) {
                console.error("Error fetching list data:", error);
            }
        };
        fetchList();
    }, []);



    useEffect(() => {
        return () => {
            if (socket) {
                socket.close();
            }
        };
    }, [socket]);

    return (
        <div className="p-4 max-w-xl mx-auto">
            {list.map((item, index) => (
                <div key={index} className="border border-gray-300 rounded mb-2">
                    <button
                        onClick={() => toggleAccordion(index, item.id)}
                        className="w-full p-4 text-left bg-gray-100 hover:bg-gray-200 transition flex justify-between items-center"
                    >
                        <span>Time: {item.startTime} Start {item.startDay} End Time {item.endTime} Day {item.endDay}</span>
                        <span>{openIndex === index ? '-' : '+'}</span>
                    </button>
                    {openIndex === index && (
                        <div className="p-4 border-t border-gray-300 bg-white">
                            <img 
                                src={`http://capstoneauctioneer.runasp.net/api/read?filePath=${item.img}`} 
                                className='w-14'
                            />
                            <span>Price: {price !== null ? `${price.toLocaleString()} VND` : 'Loading...'}</span>
                            <button onClick={() => handleJohnRoom(item.id)}>Join Room</button>
                        </div>
                    )}
                </div>
            ))}
        </div>
    );
};

export default Test;
