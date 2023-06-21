import { PacketHandler } from "../PacketManager";
import SessionManager from "../SessionManager";
import { MSGID, UUID } from "../packet/packet";

const StartFireHandler: PacketHandler = {
    code: MSGID.STARTFIRE,
    handle(data, session) {
        let uuid = UUID.deserialize(data);
        uuid.value = session.uuid;
        SessionManager.Instance.broadcast(MSGID.STARTFIRE, uuid.serialize(), session.uuid, true);
    },
}

module.exports = StartFireHandler;