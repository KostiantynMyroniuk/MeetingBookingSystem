import { useNavigate, useParams } from 'react-router-dom';
import { useAuth } from '../auth/useAuth';
import { RoomList } from '../components/RoomList';
import { RoomDetails } from '../components/RoomDetails';

export default function RoomsPage() {
    const { user } = useAuth();
    const { roomId } = useParams();
    const navigate = useNavigate();

    return (
        <div className="App">
            <h1>Система бронювання</h1>
            <p>Вітаємо, {user.email}</p>
            <RoomList selectedRoomId={roomId} onSelectRoom={(id) => navigate(`/rooms/${id}`)} />
            {roomId && <RoomDetails key={roomId} meetingRoomId={roomId} />}
        </div>
    );
}
