import fs from "fs/promises";
import path from "path";
import Session from "../Session";
import { packet } from "./packet";

export default class PacketManager
{
    static Instance: PacketManager;
    handlerMap: HandlerMap;
    
    constructor() 
    {
        this.handlerMap = {};
        this.registerHandlers();
    }
    
    async registerHandlers()
    {
        console.log("Start Register Handlers...");
        try
        {
            let handlerPath = path.join(__dirname, "..", "Handlers");
            let files = await fs.readdir(handlerPath);
            files.forEach(async name => {
                let handlerC = await import(path.join(handlerPath, name));
                let handler = new handlerC.default() as PacketHandler;
                this.handlerMap[handler.code] = handler;
            });
        }
        catch(err)
        {
            console.error(err);
            return;
        }
    }

    handleMsg(session: Session, code: number, data: Buffer) {
        if(typeof this.handlerMap[code].handleMsg !== "function")
        {
            console.error(`Handler Not Found! code: ${code}`);
        }
        this.handlerMap[code].handleMsg(session, data);
    }
}

type HandlerMap = { [key: number]: PacketHandler };

export interface PacketHandler
{
    code: packet.MSGID;
    handleMsg(session : Session, buffer:Buffer): Promise<void>;
}