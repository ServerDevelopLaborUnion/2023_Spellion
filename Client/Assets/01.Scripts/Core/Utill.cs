using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utill
{
    public static Packet.Vector3 ToPacket(this Vector3 vector3)
    {
        return new Packet.Vector3
        {
            X = vector3.x,
            Y = vector3.y,
            Z = vector3.z
        };
    }

    public static Packet.Vector2 ToPacket(this Vector2 vector2)
    {
        return new Packet.Vector2
        {
            X = vector2.x,
            Y = vector2.y
        };
    }

    public static Vector3 ToUnityVector(this Packet.Vector3 packet)
    {
        return new Vector3(packet.X, packet.Y, packet.Z);
    }

    public static Quaternion ToUnityRotation(this Packet.Vector3 packet)
    {
        return Quaternion.Euler(packet.X, packet.Y, packet.Z);
    }
}
