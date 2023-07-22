import Session from "./Session";

export default class SessionManager
{
    static Instance: SessionManager;

    sessionMap: SessionMap;

    constructor() {
        this.sessionMap = {};
    }

    addSession(session: Session): void
    {
        this.sessionMap[session.id] = session;
    }

    removeSession(id: number): void 
    {
        delete this.sessionMap[id];
    }

    getSession(id: number): Session | undefined
    {
        return this.sessionMap[id];
    }

    getAllSessionInfo()
    {
        return Object.values(this.sessionMap).map(s => {
            let {name, id, isLogin, isReady, state, level, money, gameProp} = s;
            return {name, id, isLogin, isReady, state, level, money, gameProp};
        });
    }
}

type SessionMap = { [key: number]: Session; }