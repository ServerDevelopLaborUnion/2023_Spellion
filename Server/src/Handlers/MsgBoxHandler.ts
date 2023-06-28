import Session from "../Session";
import { MSGID, MsgBox } from "../packet/packet";
import { PacketHandler } from "../PacketManager";

const MsgBoxHandler: PacketHandler = {
    code: MSGID.MSGBOX,
    handle: function(data: Buffer, session: Session): void {
        let box = MsgBox.deserialize(data);
        console.log(`[MsgBox] Context: ${box.context}, Time: ${box.time}`);
        box.context = "Recieved";
        session.sendData(MSGID.MSGBOX, box.serialize());
    },
}

module.exports = MsgBoxHandler;