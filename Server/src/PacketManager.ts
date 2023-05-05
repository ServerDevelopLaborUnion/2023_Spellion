import { Message } from "google-protobuf";
import Session from "./Session";
import { MSGID } from "./packet/packet";
import { MsgBoxHandler } from "./Handlers/MsgBoxHandler";
import { PlayerInfoHandler } from "./Handlers/PlayerInfoHandler";

export default class PacketManager
{
    static Instance: PacketManager;

    private handlerMap: { [key: number]: PacketHandler };

    constructor() {
        this.handlerMap = {};
        this.registerPacket();
    }

    registerPacket(): void {
        this.handlerMap[MSGID.MSGBOX] = MsgBoxHandler;
        this.handlerMap[MSGID.PLAYERINFO] = PlayerInfoHandler;
    }

    handleMsg(session: Session, code: number, data: Buffer) {
        this.handlerMap[code]?.handle(data, session);
    }
}

export interface PacketHandler
{
    handle(data: Buffer, session: Session): void;
}