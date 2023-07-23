using Packet;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPacketHandler
{
    public void Process(IMessage packet);
}

public class PacketManager
{
    private Dictionary<ushort, Action<ArraySegment<byte>, ushort>> _OnRecv;
    private Dictionary<ushort, IPacketHandler> _Handlers;

    public PacketManager()
    {
        _OnRecv = new Dictionary<ushort, Action<ArraySegment<byte>, ushort>>();
        _Handlers = new Dictionary<ushort, IPacketHandler>();
        Register();
    }

    private void Register()
    {
        _OnRecv.Add((ushort)MSGID.SRegisterRes, MakePacket<S_Register_Res>);
        _Handlers.Add((ushort)MSGID.SRegisterRes, new SRegisterResHandler());

        _OnRecv.Add((ushort)MSGID.SLoginRes, MakePacket<S_Login_Res>);
        _Handlers.Add((ushort)MSGID.SLoginRes, new SLoginResHandler());

        _OnRecv.Add((ushort)MSGID.SJoinRes, MakePacket<S_Join_Res>);
        _Handlers.Add((ushort)MSGID.SJoinRes, new SJoinResHandler());

        _OnRecv.Add((ushort)MSGID.SJoined, MakePacket<S_Joined>);
        _Handlers.Add((ushort)MSGID.SJoined, new SJoinedHandler());

        _OnRecv.Add((ushort)MSGID.SExitMember, MakePacket<S_Exit_Member>);
        _Handlers.Add((ushort)MSGID.SExitMember, new SExitMemberHandler());

        _OnRecv.Add((ushort)MSGID.SAllReady, MakePacket<S_All_Ready>);
        _Handlers.Add((ushort)MSGID.SAllReady, new SAllReadyHandler());

        _OnRecv.Add((ushort)MSGID.SGameStart, MakePacket<S_Game_Start>);
        _Handlers.Add((ushort)MSGID.SGameStart, new SGameStartHandler());

        _OnRecv.Add((ushort)MSGID.SMoveData, MakePacket<S_Move_Data>);
        _Handlers.Add((ushort)MSGID.SMoveData, new SMoveDataHandler());
    }

    public IPacketHandler GetPacketHandler(ushort id)
    {
        IPacketHandler hanlder = null;
        if (_Handlers.TryGetValue(id, out hanlder))
        {
            return hanlder;
        }
        else
        {
            return null;
        }
    }

    public int OnRecvPacket(ArraySegment<byte> buffer)
    {
        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset); //2바이트 긁는다.
        ushort code = BitConverter.ToUInt16(buffer.Array, buffer.Offset + 2); // 뒤에 2바이트 긁는다.

        if (_OnRecv.ContainsKey(code))
        {
            _OnRecv[code].Invoke(buffer, code);
        }
        else
        {
            Debug.LogError($"There is no packet handler for this packet : {((MSGID)code).ToString()}, ({size}");
            return 0;
        }
        Debug.Log($"패킷 받음. 길이: {size}, 프로토콜: {((MSGID)code).ToString()}");
        return size;
    }

    private void MakePacket<T>(ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
    {
        T pkt = new T();
        pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        PacketQueue.Instance.Push(id, pkt);
    }
}
