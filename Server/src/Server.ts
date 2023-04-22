import http from 'http';
import Express, { Application } from "express";
import ws from "ws";
import Session from './Session';

const PORT = 50000;

const App: Application = Express();
const httpServer: http.Server = App.listen(PORT, () => {
    console.log(`[Server.ts] Http Server 작동 중. 포트: ${PORT}`);
});

const wsServer: ws.Server = new ws.Server({
    server: httpServer
});

wsServer.on("listening",  () => {
    console.log(`[Server.ts] Websocket Server 작동 중. 포트: ${PORT}`);
});

wsServer.on("connection", (socket, request) => {
    const session = new Session(socket, (code: number, reason: Buffer) => {
        // TODO: 클라이언트 로그아웃 시 작업
    });
    console.log(`[Server.ts] 새로운 Session 로그인. IP: ${request.socket.remoteAddress}`);

    socket.on("message", (data, isBinary) => {
        if(isBinary)
            session.processPacket(data);
    });
});