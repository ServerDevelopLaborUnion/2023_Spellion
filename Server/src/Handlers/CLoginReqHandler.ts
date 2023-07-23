import { FieldPacket, RowDataPacket } from "mysql2";
import { Pool, UserVO } from "../DB";
import Session, { SessionState } from "../Session";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CLoginHandler implements PacketHandler
{
    code = packet.MSGID.C_LOGIN_REQ;
    async handleMsg(session: Session, buffer: Buffer) {
        let req = packet.C_Login_Req.deserialize(buffer);
        let res = new packet.S_Login_Res();
        let sql = `SELECT * FROM users WHERE name = ?`;
        let [rows, fieldInfos]: [UserVO[], FieldPacket[]] = await Pool.query(sql, [req.name]);
        if(rows.length < 1)
        {
            // Can't found.
            res.success = false;
            session.sendData(packet.MSGID.S_LOGIN_RES, res.serialize());
            return;
        }
        res.success = true;
        let { name, level, money } = rows[0];
        let userInfo = new packet.User({name, level, money});
        res.userData = userInfo;
        session.sendData(packet.MSGID.S_LOGIN_RES, res.serialize());
        
        session.isLogin = true;
        session.name = name;
        session.level = level;
        session.money = money;
        session.state = SessionState.LOBBY;

        console.log(`[LoginReq] User ${session.name} logged in!`);
    }
}