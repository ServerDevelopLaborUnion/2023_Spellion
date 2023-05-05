import Session from "./Session";
import { PlayerInfo } from "./packet/packet";

export default class SessionManager
{
    static Instance: SessionManager;

    private sessionMap: SessionMap = {}

    constructor() {

    }

    addSession(session: Session): void {
        this.sessionMap[session.uuid] = session;
    }

    getAllSessionInfo(): PlayerInfo[] {
        let list: PlayerInfo[] = [];
        for(let key in this.sessionMap)
        {
            list.push(this.sessionMap[key].info);
        }
        return list;
    }

    broadcast(code: number, data: Uint8Array, senderUUID: string, exceptSender: boolean = false): void
    {
        for(let key in this.sessionMap)
        {
            if(key == senderUUID && exceptSender == true) continue;
            this.sessionMap[key].sendData(code, data);
        }
    }
}

interface SessionMap
{
    [key: string]: Session;
}