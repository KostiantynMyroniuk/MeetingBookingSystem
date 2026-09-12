import { useEffect, useState } from 'react';
import { NavBar } from '../components/NavBar';
import {
    createMeetingRoom,
    deleteMeetingRoom,
    fetchMeetingRooms,
    updateMeetingRoom,
} from '../rooms/roomsApi';

const emptyForm = { name: '', description: '' };

export default function AdminRoomsPage() {
    const [rooms, setRooms] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [editingRoomId, setEditingRoomId] = useState(null);
    const [form, setForm] = useState(emptyForm);
    const [isSaving, setIsSaving] = useState(false);

    function loadRooms() {
        return fetchMeetingRooms()
            .then((data) => setRooms(data.items))
            .catch(() => setError('Не вдалося завантажити кімнати'));
    }

    useEffect(() => {
        let isMounted = true;

        fetchMeetingRooms()
            .then((data) => {
                if (isMounted) {
                    setRooms(data.items);
                }
            })
            .catch(() => {
                if (isMounted) {
                    setError('Не вдалося завантажити кімнати');
                }
            })
            .finally(() => {
                if (isMounted) {
                    setIsLoading(false);
                }
            });

        return () => {
            isMounted = false;
        };
    }, []);

    function startCreate() {
        setEditingRoomId('new');
        setForm(emptyForm);
    }

    function startEdit(room) {
        setEditingRoomId(room.id);
        setForm({ name: room.name, description: room.description ?? '' });
    }

    function cancelEdit() {
        setEditingRoomId(null);
        setForm(emptyForm);
    }

    async function handleSubmit(event) {
        event.preventDefault();
        setIsSaving(true);
        setError('');

        try {
            if (editingRoomId === 'new') {
                await createMeetingRoom(form.name, form.description || null);
            } else {
                await updateMeetingRoom(editingRoomId, form.name, form.description || null);
            }
            cancelEdit();
            await loadRooms();
        } catch {
            setError('Не вдалося зберегти кімнату');
        } finally {
            setIsSaving(false);
        }
    }

    async function handleDelete(roomId) {
        setError('');
        try {
            await deleteMeetingRoom(roomId);
            await loadRooms();
        } catch {
            setError('Не вдалося видалити кімнату');
        }
    }

    return (
        <div className="App">
            <NavBar />
            <h1>Керування кімнатами</h1>
            {error && <p role="alert">{error}</p>}

            {editingRoomId ? (
                <form onSubmit={handleSubmit}>
                    <div>
                        <label>
                            Назва
                            <input
                                value={form.name}
                                onChange={(e) => setForm({ ...form, name: e.target.value })}
                                required
                            />
                        </label>
                    </div>
                    <div>
                        <label>
                            Опис
                            <input
                                value={form.description}
                                onChange={(e) => setForm({ ...form, description: e.target.value })}
                            />
                        </label>
                    </div>
                    <div className="form-actions">
                        <button type="submit" disabled={isSaving}>
                            Зберегти
                        </button>
                        <button type="button" className="btn-secondary" onClick={cancelEdit} disabled={isSaving}>
                            Скасувати
                        </button>
                    </div>
                </form>
            ) : (
                <button type="button" onClick={startCreate}>
                    Додати кімнату
                </button>
            )}

            {isLoading ? (
                null
            ) : (
                <ul className="admin-rooms-list">
                    {rooms.map((room) => (
                        <li key={room.id}>
                            <span>
                                <strong>{room.name}</strong> {room.description}
                            </span>
                            <span className="admin-rooms-list__actions">
                                <button type="button" className="btn-secondary" onClick={() => startEdit(room)}>
                                    Редагувати
                                </button>
                                <button type="button" className="btn-danger" onClick={() => handleDelete(room.id)}>
                                    Видалити
                                </button>
                            </span>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}
