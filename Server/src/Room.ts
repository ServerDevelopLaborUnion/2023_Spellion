import JobTimer from "./JobTimer";
import RoomManager from "./RoomManager";
import Session, { SessionState } from "./Session";
import { Vector3 } from "./Utills";
import { packet } from "./packet/packet";

let idCnt = 0;

export default class Room
{
    mode: packet.GameMode;
    state: RoomState;
    owner: Session;
    members: Session[];
    id: number;

    readyCnt: number;

    gameUpdater?: JobTimer;

    constructor(owner: Session, mode: packet.GameMode) 
    {
        this.id = idCnt++;
        this.mode = mode;
        this.owner = owner;
        this.members = [];
        this.members.push(owner);
        this.readyCnt = 0;
        this.state = RoomState.NONE;
        owner.room = this;
        owner.state = SessionState.ROOM;
    }

    addMember(member: Session): void 
    {
        if(this.members.length >= 6)
        {
            throw "full room";
        }

        // Success
        console.log(this.members.length);
        this.members.push(member);
        member.room = this;
        member.state = SessionState.ROOM;
        
        // Broadcast to Members
        let {team, index} = this.getTeamAndIndex(member);
        let roomMember = new packet.RoomMember({
            user: member.user, team, index
        });
        let joined = new packet.S_Joined({member: roomMember});
        this.broadcast(packet.MSGID.S_JOINED, joined.serialize(), member);
    }

    removeMember(id: number)
    {
        let idx = this.members.findIndex(member => member.id == id);
        if(idx == -1) throw "Not Found";
        let member = this.members[idx];
        this.members.splice(idx, 1);

        if(this.members.length <= 0) 
        {
            member.state = SessionState.LOBBY;
            delete member.room;
            member.isReady = false;
            RoomManager.Instance.deleteRoom(this.id);
            return;
        }

        if(member.isReady) this.readyCnt--;
        if(member.id == this.owner.id) this.owner = this.members[0];

        member.state = SessionState.LOBBY;
        delete member.room;
        member.isReady = false;

        let fixedMembers: packet.RoomMember[] = [];
        this.members.forEach(s => {
            let {team, index} = this.getTeamAndIndex(s);
            fixedMembers.push(new packet.RoomMember({user: s.user, team, index}));
        });
        let exitData = new packet.S_Exit_Member({name: member.name, fixedMembers});
        this.broadcast(packet.MSGID.S_EXIT_MEMBER, exitData.serialize());
    }

    getTeamAndIndex(member: Session): TeamAndIndex
    {
        let index = this.members.indexOf(member);
        if(index == -1) throw "Not Found";
        let team = index % 2 == 0 ? packet.Team.Blue : packet.Team.Red;
        return { team, index };
    }

    changeReady(member: Session): void 
    {
        if(member.isReady)
        {
            this.readyCnt++;
            if(this.readyCnt > 4)
            {
                let allReady = new packet.S_All_Ready({});
                this.owner.sendData(packet.MSGID.S_ALL_READY, allReady.serialize());
            }
        }   
        else
        {
            this.readyCnt--;
        }
    }

    broadcast(code: number, payload: Uint8Array, sender?: Session)
    {
        this.members.forEach((member) => {
            if(sender)
            {
                if(sender.id != member.id)
                {
                    member.sendData(code, payload);
                }
            }
            else
            {
                member.sendData(code, payload);
            }
        })
    }

    getSpawnPosition(teamAndIdx: TeamAndIndex): Vector3
    {
        // This is Hard Coding
        let x: number = 0, y: number = 0, z: number = 0;
        if(teamAndIdx.team == packet.Team.Blue)
        {
            if(teamAndIdx.index == 0)
            {
                x = 10
                y = 8.33
                z = -18
            }
            else if(teamAndIdx.index == 2)
            {
                x = 14.5
                y = 8.33
                z = -16
            }
            else if(teamAndIdx.index == 4)
            {
                x = 14.5
                y = 8.33
                z = -20
            }
        }
        else
        {
            if(teamAndIdx.index == 1)
            {
                x = -63
                y = 11
                z = 50
            }
            else if(teamAndIdx.index == 3)
            {
                x = -59
                y = 11
                z = 52
            }
            else if(teamAndIdx.index == 5)
            {
                x = -59
                y = 11
                z = 48
            }
        }
        return { x, y, z };
    }
}

export type TeamAndIndex = { team: packet.Team, index: number };

export enum RoomState
{
    NONE,
    INGAME
}