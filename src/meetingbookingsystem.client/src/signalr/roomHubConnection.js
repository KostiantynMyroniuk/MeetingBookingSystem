import { HubConnectionBuilder, HttpTransportType, LogLevel } from '@microsoft/signalr';

let connection = null;
let startPromise = null;

function getConnection() {
    if (!connection) {
        connection = new HubConnectionBuilder()
            .withUrl('/hubs/meeting-rooms', {
                transport: HttpTransportType.WebSockets,
                withCredentials: true,
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Warning)
            .build();
    }
    return connection;
}

export { getConnection as getRoomHubConnection };

export function startRoomHubConnection() {
    const hub = getConnection();

    if (!startPromise) {
        startPromise = hub.start().catch((error) => {
            startPromise = null;
            throw error;
        });
    }

    return startPromise;
}
