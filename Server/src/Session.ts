import ws from "ws";
import { MSGID, MsgBox } from "./packet/packet";

export default class Session
{
    socket: ws;
    
    constructor(socket: ws, closeListener: (code: number, reason: Buffer) => void) {
        this.socket = socket;
        this.socket.on("close", closeListener);
    }

    processPacket(data: ws.RawData) {
        
    }
}