import { packet } from "./packet/packet";

export interface GameProperty
{
    position?: Vector3;
    eulerAngle?: Vector3;
}

export interface Vector3 { x: number, y: number, z: number }
export interface Vector2 { x: number, y: number }

export function Vector2Packet(value: Vector2): packet.Vector2
{
    let { x, y } = value;
    return new packet.Vector2({x, y});
}

export function Vector3Packet(value?: Vector3): packet.Vector3 | undefined
{
    if(!value) return undefined;
    let { x, y, z } = value;
    return new packet.Vector3({x, y, z});
}

export function PacketToVector2(value: packet.Vector2): Vector2
{
    let { x, y } = value;
    return { x, y };
}

export function PacketToVector3(value: packet.Vector3): Vector3
{
    let { x, y, z } = value;
    return { x, y, z };
}