import Session from "../Session";
import { MsgBox } from "../packet/packet";
import { PacketHandler } from "../PacketManager";

export const MsgBoxHandler: PacketHandler = {
    handle(data: Buffer, session: Session): void
    {
        let box = MsgBox.deserialize(data);
        console.log(`[MsgBox] Context: ${box.context}`);
    }
}