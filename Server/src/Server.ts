import Express from "express";
import dotenv from "dotenv";
import { WebSocket } from "ws";
import Session from "./Session";
import SessionManager from "./SessionManager";
import PacketManager from "./packet/PacketManager";
import RoomManager from "./RoomManager";
// dotenv.config();

const app = Express();

app.get("/", (req, res) => {
    res.json(SessionManager.Instance.getAllSessionInfo());
});

app.get("/room", (req, res) => {
    res.json(RoomManager.Instance.getAllRoomInfo());
});

const httpServer = app.listen(50000, () => {
    console.log(`Web Server is listening on port ${50000}`);
});

SessionManager.Instance = new SessionManager();
PacketManager.Instance = new PacketManager();
RoomManager.Instance = new RoomManager();

const wsServer = new WebSocket.Server({
    server: httpServer
});

wsServer.on("listening", () => {
    console.log(`Socket Server is listening on port ${50000}`);
});

wsServer.on("connection", (socket, req) => {
    const session = new Session(socket);
    SessionManager.Instance.addSession(session);

    socket.on("message", (data, isBinary) => {
        if(isBinary)
            session.processPacket(data);
    });

    socket.on("close", (code, reason) => {
        if(session.room)
        {
            session.room.removeMember(session.id);
        }
        SessionManager.Instance.removeSession(session.id);
    })
});
