import Session from "../Session";
import { MSGID, MsgBox } from "../packet/packet";
import { PacketHandler } from "../PacketManager";

export const MsgBoxHandler: PacketHandler = {
    handle(data: Buffer, session: Session): void
    {
        let box = MsgBox.deserialize(data);
        console.log(`[MsgBox] Context: ${box.context}`);
        box = new MsgBox({context: "Receive"});
        session.sendData(MSGID.MSGBOX, box.serialize());
    }
}