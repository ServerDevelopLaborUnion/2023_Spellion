import { PacketHandler } from "../PacketManager";
import { PlayerInfo } from "../packet/packet";

export const PlayerInfoHandler: PacketHandler = {
    handle(data, session) {
        let info = PlayerInfo.deserialize(data);
        info.uuid = session.uuid;
        session.info = info;
    },
}