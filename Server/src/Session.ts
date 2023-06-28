import ws from "ws";
import { MSGID, MsgBox, PlayerInfo, PlayerInfoList, Vector2, Vector3 } from "./packet/packet";
import { Message } from "google-protobuf";
import PacketManager from "./PacketManager";
import SessionManager from "./SessionManager";

export default class Session
{
    private socket: ws;
    uuid: string;
    info: PlayerInfo;
    isLogin: boolean = false;
    
    constructor(socket: ws, uuid:string, closeListener: (code: number, reason: Buffer) => void) {
        this.socket = socket;
        this.uuid = uuid;
        this.socket.on("close", closeListener);
        this.info = new PlayerInfo({uuid: uuid});
        this.info.pos = new Vector3({x: 0, y: 1, z: 0});
        this.info.rot = new Vector2({x: 0, y: 0});
        this.info.isGround = true;
    }

    Init(list: PlayerInfoList) {
        this.sendData(MSGID.INITLIST, list.serialize());
    }

    processPacket(data: ws.RawData) {
        let code: number = this.getInt16LEFromBuffer(data.slice(2, 4) as Buffer);
        PacketManager.Instance.handleMsg(this, code, data.slice(4) as Buffer);
        console.log(`[Session.ts] Packet recieved. ID: ${code}`);
    }

    private getInt16LEFromBuffer(buffer:Buffer): number
    {
        return buffer.readInt16LE();
    }

    sendData(code: MSGID, data: Uint8Array) {
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
}
