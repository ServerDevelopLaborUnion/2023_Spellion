import JobTimer from "../JobTimer";
import { RoomState } from "../Room";
import Session, { SessionState } from "../Session";
import { Vector3, Vector3Packet } from "../Utills";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CGameStartHandler implements PacketHandler
{
    code = packet.MSGID.C_GAME_START;
    async handleMsg(session: Session, buffer: Buffer) {
        if(!session.isLogin || session.state != SessionState.ROOM 
            || !session.room || session.room.owner.id != session.id)
        {
            // 비정상적 상황이므로 클라이언트 강제 종료
            return;
        }

        const room = session.room;

        room.state = RoomState.INGAME;
        room.gameUpdater = new JobTimer(20, () => {
            
        });

        room.members.forEach(s => {
            let { team, index } = room.getTeamAndIndex(s);
            let pInfo = new packet.RoomMember({user: s.user, team, index});
            let spawnPos = Vector3Packet(room.getSpawnPosition({team, index}));
            let start = new packet.S_Game_Start({pInfo, spawnPos});
            s.sendData(packet.MSGID.S_GAME_START, start.serialize());

            let eulerAngle: Vector3 = {x: 0, y: 0, z: 0};
            s.gameProp = {eulerAngle, position: spawnPos};
        });
    }
    
}