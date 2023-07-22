import Session, { SessionState } from "../Session";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CExitRoomHandler implements PacketHandler
{
    code = packet.MSGID.C_EXIT_ROOM;
    async handleMsg(session: Session, buffer: Buffer) {
        if(!session.isLogin || session.state != SessionState.ROOM || !session.room)
        {
            // 비정상적 상황이므로 클라이언트 강제 종료
            return;
        }
        session.room.removeMember(session.id);
    }
}