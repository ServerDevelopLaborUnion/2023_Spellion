import { FieldPacket, ResultSetHeader } from "mysql2";
import { Pool, UserVO } from "../DB";
import Session, { SessionState } from "../Session";
import { PacketHandler } from "../packet/PacketManager";
import { packet } from "../packet/packet";

export default class CRegisterReqHandler implements PacketHandler
{
    code = packet.MSGID.C_REGISTER_REQ;
    async handleMsg(session: Session, buffer: Buffer) {
        let req = packet.C_Register_Req.deserialize(buffer);
        let res = new packet.S_Register_Res();
        let sql = `SELECT * FROM users WHERE name = ?`;
        let [rows, fieldInfos]: [UserVO[], FieldPacket[]] = await Pool.query(sql, [req.name]);
        if(rows.length > 0)
        {
            // Already Exist Same Name User
            res.success = false;
            session.sendData(packet.MSGID.S_LOGIN_RES, res.serialize());
            return;
        }
        sql = `INSERT INTO users(name, level, money) VALUES (?, ?, ?)`;
        try {
            let [result, info] : [ResultSetHeader, FieldPacket[]]  = await Pool.query(sql, [req.name, 1, 0]);
            if(result.affectedRows != 1) throw "Error";
            res.success = true;
            res.userData = new packet.User({name: req.name, level: 1, money: 0});
            session.name = req.name;
            session.level = 1;
            session.money = 0;
            session.state = SessionState.LOBBY;
            session.isLogin = true;
            console.log(`[RegisterReq] User ${session.name} logged in!`);
        }
        catch(err)
        {
            console.log(err);
            res.success = false;
            session.isLogin = false;
            session.state = SessionState.NONE;
        }
        session.sendData(packet.MSGID.S_REGISTER_RES, res.serialize());
    }
}