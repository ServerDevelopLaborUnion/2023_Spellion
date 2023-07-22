import Room from "./Room";
import Session from "./Session";
import { packet } from "./packet/packet";

export default class RoomManager
{
    static Instance: RoomManager;
    roomList: Room[];

    constructor() 
    {
        this.roomList = [];
    }

    createRoom(owner: Session, mode: packet.GameMode): Room
    {
        let room = new Room(owner, mode);
        this.roomList.push(room);
        return room;
    }

    getRoom(id: number): Room | undefined
    {
        return this.roomList.find(r => r.id == id);
    }

    getAllRoomInfo()
    {
        return this.roomList.map(room => {
            let {id, state, mode, readyCnt} = room;
            let members = room.members.map(s => {
                let {name, id, isLogin, isReady, state, level, money, gameProp} = s;
                return {name, id, isLogin, isReady, state, level, money, gameProp};
            });
            return {id, members, state, mode, readyCnt};
        });
    }

    deleteRoom(id: number): void
    {
        let idx = this.roomList.findIndex(r => r.id == id);
        if(idx == -1) return;
        this.roomList.splice(idx, 1);
    }

    tryJoinRoom(session: Session): boolean
    {
        for(let i = 0; i < this.roomList.length; i++)
        {
            try
            {
                this.roomList[i].addMember(session);
                return true;
            }
            catch(err)
            {
                console.log(err);
                continue;
            }
        }
        return false;
    }
}