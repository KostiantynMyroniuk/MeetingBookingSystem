import { useNavigate, useParams } from 'react-router-dom';
import { NavBar } from '../components/NavBar';
import { RoomList } from '../components/RoomList';
import { RoomDetails } from '../components/RoomDetails';

export default function RoomsPage() {
    const { roomId } = useParams();
    const navigate = useNavigate();

    return (
        <div className="App">
            <NavBar />
            <h1>Система бронювання</h1>
            <RoomList selectedRoomId={roomId} onSelectRoom={(id) => navigate(`/rooms/${id}`)} />
            {roomId && <RoomDetails key={roomId} meetingRoomId={roomId} />}
        </div>
    );
}
