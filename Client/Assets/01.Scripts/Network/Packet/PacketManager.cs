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
        _OnRecv.Add((ushort)MSGID.Msgbox, MakePacket<MsgBox>);
        _Handlers.Add((ushort)MSGID.Msgbox, new MsgBoxHandler());

        _OnRecv.Add((ushort)MSGID.Playerinfolist, MakePacket<PlayerInfoList>);
        _Handlers.Add((ushort)MSGID.Playerinfolist, new PlayerInfoListHandler());

        _OnRecv.Add((ushort)MSGID.Initlist, MakePacket<PlayerInfoList>);
        _Handlers.Add((ushort)MSGID.Initlist, new InitListHandler());

        _OnRecv.Add((ushort)MSGID.Newsession, MakePacket<PlayerInfo>);
        _Handlers.Add((ushort)MSGID.Newsession, new NewSessionHandler());

        _OnRecv.Add((ushort)MSGID.Startfire, MakePacket<UUID>);
        _Handlers.Add((ushort)MSGID.Startfire, new StartFIreHandler());

        _OnRecv.Add((ushort)MSGID.Stopfire, MakePacket<UUID>);
        _Handlers.Add((ushort)MSGID.Stopfire, new StopFireHandler());
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
