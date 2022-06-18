using Newtonsoft.Json;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SocketIORoomConnection : IRoomConnection
{
    private SocketIOUnity socket;

    public event IRoomConnection.DelError OnError;
    public event IRoomConnection.DelRoleAssign OnRoleAssignOK;
    public event IRoomConnection.DelRoleAssign OnRoleAssignFail;
    public event IRoomConnection.DelRoleAssign OnRoleAssigned;
    public event IRoomConnection.DelRoomConnect OnRoomConnectionSuccess;
    public event IRoomConnection.DelRoomConnect OnRoomConnectionFail;
    public event IRoomConnection.DelGame OnGameStarted;
    public event IRoomConnection.DelGame OnGameNext;
    public event IRoomConnection.DelOrderMade OnOrderMade;
    public event IRoomConnection.DelOrderMade OnOrderOK;
    public event IRoomConnection.DelOrderMade OnOrderFail;

    private Room room;

    public SocketIORoomConnection(Room room)
    {
        this.room = room;
    }

    public void ConnectToRoom(Room room)
    {
        socket = new SocketIOUnity(@room.Uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    { "roomUri", @room.Id }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        socket.OnError += Socket_OnError;
        socket.OnConnected += Socket_OnConnected;
        socket.OnDisconnected += Socket_OnDisconnected;

        socket.Connect();
    }

    private void Socket_OnConnected(object sender, System.EventArgs e)
    {
        OnRoomConnectionSuccess?.Invoke(room, this);
        SubscribeToEvents();
    }

    private void Socket_OnDisconnected(object sender, string e)
    {
        OnRoomConnectionFail?.Invoke(room, this);
    }

    private void Socket_OnError(object sender, string e)
    {
        OnError?.Invoke(e);
    }

    private void SubscribeToEvents()
    {
        socket.OnUnityThread("role:assign-ok", Socket_OnRoleAssignOK);
        socket.OnUnityThread("role:assign-error", Socket_OnRoleAssignFail);
        socket.OnUnityThread("role:assigned", Socket_OnRoleAssigned);
        socket.OnUnityThread("game:started", Socket_OnGameStarted);
        socket.OnUnityThread("game:next", Socket_OnGameNext);
        socket.OnUnityThread("invoice:added", Socket_OnOrderMade);
        socket.OnUnityThread("round:invoice-ok", Socket_OnOrderOK);
        socket.OnUnityThread("round:invoice-error", Socket_OnOrderFail);
    }

    private void Socket_OnRoleAssignOK(SocketIOResponse response)
    {
        OnRoleAssignOK?.Invoke(response.ToString());
    }

    private void Socket_OnRoleAssignFail(SocketIOResponse response)
    {
        OnRoleAssignFail?.Invoke(response.ToString());
    }

    private void Socket_OnRoleAssigned(SocketIOResponse response)
    {
        OnRoleAssigned?.Invoke(response.ToString());
    }

    private void Socket_OnGameStarted(SocketIOResponse response)
    {
        OnGameStarted?.Invoke();
    }

    private void Socket_OnGameNext(SocketIOResponse response)
    {
        OnGameNext?.Invoke();
    }

    private void Socket_OnOrderMade(SocketIOResponse response)
    {
        OnOrderMade?.Invoke(null);
    }

    private void Socket_OnOrderOK(SocketIOResponse response)
    {
        OnOrderOK?.Invoke(null);
    }

    private void Socket_OnOrderFail(SocketIOResponse response)
    {
        OnOrderFail?.Invoke(null);
    }

    public void MakeOrder(Order order)
    {
        EmitMakeOrder(order);
    }

    private void EmitMakeOrder(Order order)
    {
        string json = MakeJsonString(new { order = order.Amount,
                                           type = order.OrderType.ToString() }) ;
        socket.EmitStringAsJSON("round:invoice", json);
    }

    public void SelectRole(SupplierRole role)
    {
        EmitSelectRole(role);
    }

    private void EmitSelectRole(SupplierRole role)
    {
        string json = MakeJsonString(new { role = role.ToString().ToLower() });
        socket.EmitStringAsJSON("role:assign", json);
    }

    public void DisconnectFromRoom()
    {
        //string json = MakeJsonString(new { role = role.ToString().ToLower() });
        //socket.EmitStringAsJSON("role:assign", json);
    }

    public void ForceStartGame()
    {
        string json = MakeJsonString(new {  });
        socket.EmitStringAsJSON("game:start", json);
    }

    private string MakeJsonString(object jsonObj)
    {
        return JsonConvert.SerializeObject(jsonObj);
    }
}
