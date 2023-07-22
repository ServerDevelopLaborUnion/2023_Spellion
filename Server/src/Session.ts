import { RawData, WebSocket } from "ws";
import { GameProperty } from "./Utills";
import { packet } from "./packet/packet";
import PacketManager from "./packet/PacketManager";
import Room from "./Room";

let idCnt = 0;

export default class Session
{
    socket: WebSocket;
    id: number;

    state: SessionState;

    isLogin: boolean;
    name?: string;
    level?: number;
    money?: number;

    room?: Room;

    isReady: boolean;

    gameProp?: GameProperty;

    constructor(socket: WebSocket) {
        this.socket = socket;
        this.id = idCnt++;
        this.isLogin = false;
        this.state = SessionState.NONE;
        this.isReady = false;
    }

    processPacket(data: RawData) {
        let code: number = this.getInt16LEFromBuffer(data.slice(2, 4) as Buffer);
        PacketManager.Instance.handleMsg(this, code, data.slice(4) as Buffer);
        console.log(`[Session.ts] Packet recieved. ID: ${code}`);
    }

    private getInt16LEFromBuffer(buffer:Buffer): number
    {
        return buffer.readInt16LE();
    }

    sendData(code: packet.MSGID, data: Uint8Array) {
        let len: number = data.length + 4;

        let lenBuffer: Uint8Array = new Uint8Array(2); 
        new DataView(lenBuffer.buffer).setUint16(0, len, true);

        let codeBuffer: Uint8Array = new Uint8Array(2);
        new DataView(codeBuffer.buffer).setUint16(0, code, true);

        let sendBuffer: Uint8Array = new Uint8Array(len);
        sendBuffer.set(lenBuffer, 0);
        sendBuffer.set(codeBuffer, 2);
        sendBuffer.set(data, 4);

        this.socket.send(sendBuffer);
    }

    get user(): packet.User 
    {
        let {name, level, money} = this;
        return new packet.User({name, level, money});
    }
}

export enum SessionState
{
    NONE,
    LOBBY,
    ROOM,
    GAME
}