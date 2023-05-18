import http from 'http';
import Express, { Application } from "express";
import ws from "ws";
import Session from './Session';
import crypto from 'crypto'
import SessionManager from './SessionManager';
import PacketManager from './PacketManager';
import UpdateTimer from './UpdateTimer';
import { MSGID, PlayerInfo, PlayerInfoList } from './packet/packet';

const PORT = 50000;

const App: Application = Express();
const httpServer: http.Server = App.listen(PORT, () => {
    console.log(`[Server.ts] Http Server 작동 중. 포트: ${PORT}`);
});

const wsServer: ws.Server = new ws.Server({
    server: httpServer
});

SessionManager.Instance = new SessionManager();
PacketManager.Instance = new PacketManager();

wsServer.on("listening",  () => {
    console.log(`[Server.ts] Websocket Server 작동 중. 포트: ${PORT}`);
});

wsServer.on("connection", (socket, request) => {
    const session = new Session(socket, crypto.randomUUID(), (code: number, reason: Buffer) => {
        // TODO: 클라이언트 로그아웃 시 작업
        SessionManager.Instance.removeSession(session.uuid);
        console.log(`[Server.ts] Session 로그아웃됨. ID: ${session.uuid}`);
    });
    SessionManager.Instance.addSession(session);
    console.log(`[Server.ts] 새로운 Session 로그인. ID: ${session.uuid}`);

    // For Debugs

    socket.on("message", (data, isBinary) => {
        if(isBinary)
            session.processPacket(data);
    });
});

let updateTimer: UpdateTimer = new UpdateTimer(50, () => {
    let list: PlayerInfo[] = SessionManager.Instance.getAllSessionInfo();
    if(list.length <= 0) return;
    let infoList: PlayerInfoList = new PlayerInfoList({list});
    SessionManager.Instance.broadcast(MSGID.PLAYERINFOLIST, infoList.serialize(), "");
});
updateTimer.startTimer();