import Session from "./Session";

export default class SessionManager
{
    static Instance: SessionManager;

    private sessionMap: SessionMap = {}

    constructor() {

    }

    addSession(session: Session): void {
        this.sessionMap[session.uuid] = session;
    }
}

interface SessionMap
{
    [key: string]: Session;
}