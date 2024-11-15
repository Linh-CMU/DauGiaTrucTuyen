
import { useParams } from 'react-router-dom';
import { joinRoomAuction } from '../../queries/AdminAPI';
import React, { useEffect, useState } from 'react';

const Room = () => {

    const [data, setData] = useState();
    const { id } = useParams<{ id: string }>();
    
    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await joinRoomAuction(id);
                console.log(response);
            } catch (error) {
                
            }
        }

    })
    return (
        <>
            <h1></h1>
        </>
    )
}

export default Room;