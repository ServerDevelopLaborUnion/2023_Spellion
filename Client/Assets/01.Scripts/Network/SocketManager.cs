using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net.WebSockets;
using UnityEngine;
using Packet;
using Google.Protobuf;
using System.Collections.Generic;

public class SocketManager : MonoBehaviour
{
    private static SocketManager _instance = null;
    public static SocketManager Instance 
    {
        get
        {
            if(_instance == null)
                _instance = FindObjectOfType<SocketManager>();
            return _instance;
        }
    }

    private string _url;
    private RecvBuffer _recvBuffer;
    private PacketManager _packetManager;
    private Queue<PacketMessage> _sendQueue;
    private ClientWebSocket _socket = null;
    private bool _isReadyToSend = true;

    public void Init(string url)
    {
        _url = url;
        _recvBuffer = new RecvBuffer(1024 * 10);
        _packetManager = new PacketManager();
        _sendQueue = new Queue<PacketMessage>();
    }

    private void Update()
    {
        if (PacketQueue.Instance.Count > 0)
        {
            List<PacketMessage> list = PacketQueue.Instance.PopAll();
            foreach (PacketMessage pmsg in list)
            {
                IPacketHandler handler = _packetManager.GetPacketHandler(pmsg.Id);
                if (handler != null)
                {
                    handler.Process(pmsg.Message);
                }
                else
                {
                    Debug.LogError($"There is no handler for this packet : {pmsg.Id}");
                }
            }
        }

        if (_isReadyToSend && _sendQueue.Count > 0)
        {
            SendMessages(); 
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    public void RegisterSend(MSGID code, IMessage msg)
    {
        _sendQueue.Enqueue(new PacketMessage { Id = (ushort)code, Message = msg });
    }

    private async void SendMessages()
    {
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            _isReadyToSend = false; 

            List<PacketMessage> sendList = new List<PacketMessage>();
            while (_sendQueue.Count > 0)
            {
                sendList.Add(_sendQueue.Dequeue());
            }

            byte[] sendBuffer = new byte[1024 * 10];
            foreach (PacketMessage pmsg in sendList)
            {
                int len = pmsg.Message.CalculateSize(); 

                Array.Copy(BitConverter.GetBytes((ushort)(len + 4)), 0, sendBuffer, 0, sizeof(ushort));
                Array.Copy(BitConverter.GetBytes(pmsg.Id), 0, sendBuffer, 2, sizeof(ushort));
                Array.Copy(pmsg.Message.ToByteArray(), 0, sendBuffer, 4, len);

                ArraySegment<byte> segment = new ArraySegment<byte>(sendBuffer, 0, len + 4);
                await _socket.SendAsync(segment, WebSocketMessageType.Binary, true, CancellationToken.None);
            }

            _isReadyToSend = true; 
        }
    }

    public async void Connection(Action callback = null)
    {
        if(_socket != null && _socket.State == WebSocketState.Open)
        {
            Debug.LogWarning("Socket is already open!");
            return;
        }

        _socket = new ClientWebSocket();
        Uri serverUri = new Uri(_url);

        try
        {
            await _socket.ConnectAsync(serverUri, CancellationToken.None);
            callback?.Invoke();
            ReceiveLoop();
        }
        catch(Exception ex)
        {
            Debug.LogError("Connection Error : check server status... " + ex.Message);
            throw;
        }
    }

    private async void ReceiveLoop()
    {
        while (_socket != null && _socket.State == WebSocketState.Open)
        {
            try
            {
                WebSocketReceiveResult result
                    = await _socket.ReceiveAsync(_recvBuffer.WriteSegment, CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Binary)
                {
                    if (result.EndOfMessage == true)
                    {
                        _recvBuffer.OnWrite(result.Count);
                        int readByte = ProcessPacket(_recvBuffer.ReadSegment);
                        if (readByte == 0)
                        {
                            Disconnect();
                            break;
                        }

                        _recvBuffer.OnRead(readByte);
                        _recvBuffer.Clean();
                    }
                    else
                    {
                        _recvBuffer.OnWrite(result.Count);
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    Debug.LogError("Socket closed by server in normal status");
                    break;
                }
            }
            catch (WebSocketException we)
            {
                Debug.LogError(we.Message);
                break;
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.GetType()} : {e.Message}");
                break;
            }
        }
    }

    private int ProcessPacket(ArraySegment<byte> buffer)
    {
        return _packetManager.OnRecvPacket(buffer);
    }

    public void Disconnect()
    {
        if (_socket != null && _socket.State == WebSocketState.Open)
        {
            _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "quit Client", CancellationToken.None);
        }
    }
}
