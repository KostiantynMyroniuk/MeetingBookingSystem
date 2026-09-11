import { useEffect } from 'react';
import { getRoomHubConnection, startRoomHubConnection } from './roomHubConnection';

export function useRoomSlotUpdates(meetingRoomId, onSlotStatusChanged) {
    useEffect(() => {
        if (!meetingRoomId) {
            return undefined;
        }

        const hub = getRoomHubConnection();
        let isJoined = false;
        let isCancelled = false;

        const handleSlotStatusChanged = (timeSlotDto) => {
            if (timeSlotDto.meetingRoomId === meetingRoomId) {
                onSlotStatusChanged(timeSlotDto);
            }
        };

        hub.on('SlotStatusChanged', handleSlotStatusChanged);

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
            hub.off('SlotStatusChanged', handleSlotStatusChanged);

            if (isJoined) {
                hub.invoke('LeaveGroup', meetingRoomId).catch(() => {});
            }
        };
    }, [meetingRoomId, onSlotStatusChanged]);
}
