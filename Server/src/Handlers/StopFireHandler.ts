import { PacketHandler } from "../PacketManager";
import SessionManager from "../SessionManager";
import { MSGID, UUID } from "../packet/packet";

const StopFireHandler: PacketHandler = {
    code: MSGID.STOPFIRE,
    handle(data, session) {
        let uuid = UUID.deserialize(data);
        uuid.value = session.uuid;
        SessionManager.Instance.broadcast(MSGID.STOPFIRE, uuid.serialize(), session.uuid, true);
    },
}

module.exports = StopFireHandler