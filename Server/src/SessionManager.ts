import Session from "./Session";
import { MSGID, PlayerInfo, PlayerInfoList } from "./packet/packet";

export default class SessionManager
{
    static Instance: SessionManager;

    private sessionMap: SessionMap = {}

    constructor() {

    }

    addSession(session: Session): void {
        let list: PlayerInfoList = new PlayerInfoList({list: this.getAllSessionInfo()});
        this.sessionMap[session.uuid] = session;
        session.Init(list);
        this.broadcast(MSGID.NEWSESSION, session.info.serialize(), session.uuid, true);
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

    removeSession(uuid: string) {
        delete this?.sessionMap[uuid];
    }
}

interface SessionMap
{
    [key: string]: Session;
}