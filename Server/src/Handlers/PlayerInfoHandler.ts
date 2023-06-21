import { PacketHandler } from "../PacketManager";
import { MSGID, PlayerInfo } from "../packet/packet";

const PlayerInfoHandler: PacketHandler = {
    code: MSGID.PLAYERINFO,
    handle(data, session) {
        let info = PlayerInfo.deserialize(data);
        info.uuid = session.uuid;
        session.info = info;
    },
}

module.exports = PlayerInfoHandler;