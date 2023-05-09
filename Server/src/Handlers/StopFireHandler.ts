import { PacketHandler } from "../PacketManager";
import SessionManager from "../SessionManager";
import { MSGID, UUID } from "../packet/packet";

export const StopFireHandler: PacketHandler = {
    handle(data, session) {
        let uuid = UUID.deserialize(data);
        uuid.value = session.uuid;
        SessionManager.Instance.broadcast(MSGID.STOPFIRE, uuid.serialize(), session.uuid, true);
    },
}