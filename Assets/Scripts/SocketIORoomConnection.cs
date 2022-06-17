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

    private void OnOrderMade()
    {

    }

    private void OnRoundAdvance()
    {

    }

    public Task<bool> MakeOrder(Order order)
    {
        return null;
    }

    private void EmitMakeOrder(Order order)
    {

    }

    public void SelectRole(SupplierRole role)
    {
        EmitSelectRole(role);
    }

    private void EmitSelectRole(SupplierRole role)
    {
        string json = JsonConvert.SerializeObject(new { role = role.ToString().ToLower() });
        socket.EmitStringAsJSON("role:assign", json);
    }

    public void DisconnectFromRoom()
    {
        
    }
}
