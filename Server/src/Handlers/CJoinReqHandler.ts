import { TeamAndIndex } from "../Room";
import RoomManager from "../RoomManager";
import Session, { SessionState } from "../Session";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CJoinReqHandler implements PacketHandler
{
    code = packet.MSGID.C_JOIN_REQ;
    async handleMsg(session: Session, buffer: Buffer)
    {
        if(!session.isLogin || session.state != SessionState.LOBBY) 
        {
            // 비정상적 상황이므로 클라이언트 강제 종료
            return;
        }
        let req = packet.C_Join_Req.deserialize(buffer);
        let res = new packet.S_Join_Res();
        if(RoomManager.Instance.tryJoinRoom(session))
        {
            console.log("success");
            res.success = true;
            res.isOwner = false;
            res.index = session.room?.getTeamAndIndex(session).index as number;
            res.members = [];
            session.room?.members.forEach(s => {
                if(s.id != session.id)
                {
                    let {team, index} = session.room?.getTeamAndIndex(s) as TeamAndIndex;
                    res.members.push(new packet.RoomMember({user: s.user, team, index}));
                }
            });
        }
        else 
        {
            let room = RoomManager.Instance.createRoom(session, req.mode);
            res.isOwner = true;
            res.index = room.getTeamAndIndex(session).index;
        }
        res.success = true;
        session.sendData(packet.MSGID.S_JOIN_RES, res.serialize());
    }
}