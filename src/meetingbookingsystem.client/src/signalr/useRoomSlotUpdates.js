import { useEffect } from 'react';
import { getRoomHubConnection, startRoomHubConnection } from './roomHubConnection';

export function useRoomSlotUpdates(meetingRoomId, onSlotBooked) {
    useEffect(() => {
        if (!meetingRoomId) {
            return undefined;
        }

        const hub = getRoomHubConnection();
        let isJoined = false;
        let isCancelled = false;

        const handleSlotBooked = (timeSlotDto) => {
            if (timeSlotDto.meetingRoomId === meetingRoomId) {
                onSlotBooked(timeSlotDto);
            }
        };

        hub.on('SlotBooked', handleSlotBooked);

        startRoomHubConnection()
            .then(() => {
                if (isCancelled) {
                    return;
                }
                return hub.invoke('JoinGroup', meetingRoomId).then(() => {
                    isJoined = true;
                });
            })
            .catch(() => {});

        return () => {
            isCancelled = true;
            hub.off('SlotBooked', handleSlotBooked);

            if (isJoined) {
                hub.invoke('LeaveGroup', meetingRoomId).catch(() => {});
            }
        };
    }, [meetingRoomId, onSlotBooked]);
}
