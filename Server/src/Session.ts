import ws from "ws";
import { MSGID, MsgBox, PlayerInfo, Vector2, Vector3 } from "./packet/packet";

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
    }

    processPacket(data: ws.RawData) {
        
    }

    //#region Get & Set
    get pos(): Vector3 | undefined {
        return this.info.pos;
    }
    set pos(pos: Vector3) {
        this.info.pos = pos;
    }

    get rot(): Vector2 | undefined {
        return this.info.rot;
    }
    set rot(rot: Vector2) {
        this.info.rot = rot;
    }
    //#endregion
}