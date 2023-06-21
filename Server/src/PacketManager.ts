import fs from "fs/promises"
import path from "path"
import { Message } from "google-protobuf";
import Session from "./Session";
import { MSGID } from "./packet/packet";

export default class PacketManager
{
    static Instance: PacketManager;

    private handlerMap: Map<MSGID, PacketHandler>

    constructor() {
        this.handlerMap = new Map<MSGID, PacketHandler>();
        this.registerPacket();
    }

    async registerPacket() {
        let dir = await fs.readdir(path.join(__dirname, "Handlers"));
        dir.forEach(async fileName => {
            let handler: PacketHandler = await import(path.join(__dirname, "Handlers", fileName));
            this.handlerMap.set(handler.code, handler);
        });
    }

    handleMsg(session: Session, code: number, data: Buffer) {
        let handler = this.handlerMap.get(code);
        if(handler)
            handler.handle(data, session);
        else throw Error("Handler Not Found");
    }
}

export interface PacketHandler
{
    code: MSGID;
    handle(data: Buffer, session: Session): void;
}