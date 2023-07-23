import Session from "../Session";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CMoveDataHandler implements PacketHandler
{
    code = packet.MSGID.C_MOVE_DATA;
    async handleMsg(session: Session, buffer: Buffer) {
        let moveData = packet.C_Move_Data.deserialize(buffer);
        if(session.gameProp)
        {
            session.gameProp.position = moveData.pos;
            session.gameProp.eulerAngle = moveData.eulurAngle;
        }
    }
    
}