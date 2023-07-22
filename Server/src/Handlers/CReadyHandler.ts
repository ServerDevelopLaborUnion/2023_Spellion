import Session, { SessionState } from "../Session";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CReadyHandler implements PacketHandler
{
    code = packet.MSGID.C_READY;
    async handleMsg(session: Session, buffer: Buffer) 
    {
        if(!session.isLogin || !session.room || session.state != SessionState.ROOM)
        {
            // 비정상적인 상황이므로 클라이언트 강제 종료
            return;
        }
        if(session.room.owner.id == session.id) return;

        session.isReady = !session.isReady;
        session.room.changeReady(session);
    }
}