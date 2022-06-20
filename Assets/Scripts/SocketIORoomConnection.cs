using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using UnityEngine;

public class OrderReceiveDTO
{
    public int roundLength { get; set; }
    public int order { get; set; }
    public string type { get; set; }
}

public class OrderOnOkDTO
{
    public int order { get; set; }
    public string type { get; set; }
    public string role { get; set; }
    public bool done { get; set; }
}

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
    public event IRoomConnection.DelGameEnded OnGameEnded;
    public event IRoomConnection.DelOrderReceived OnOrderReceived;
    public event IRoomConnection.DelOrderMade OnOrderMade;
    public event IRoomConnection.DelOrderOk OnOrderOK;
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
        socket.OnUnityThread("game:next", Socket_OnOrderReceived);
        socket.OnUnityThread("game:end", Socket_OnGameEnded);
        socket.OnUnityThread("invoice:added", Socket_OnOrderMade);
        socket.OnUnityThread("round:invoice-ok", Socket_OnOrderOK);
        socket.OnUnityThread("round:invoice-error", Socket_OnOrderFail);
    }

    private void Socket_OnGameEnded(SocketIOResponse response)
    {
        GameHistoryDTO[] dtos = JsonConvert.DeserializeObject<GameHistoryDTO[]>(response.ToString());

        OnGameEnded?.Invoke(dtos[0]);
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
        OnGameStarted?.Invoke(response.ToString());
    }

    private void Socket_OnOrderReceived(SocketIOResponse response)
    {
        OrderReceiveDTO[] dto = JsonConvert.DeserializeObject<OrderReceiveDTO[]>(response.ToString());

        int roundCurrent = dto[0].roundLength;
        int amount = dto[0].order;
        string orderTypeString = dto[0].type;

        OrderType orderType = OrderType.requested;

        if (orderTypeString.ToLower() == "provided")
        {
            orderType = OrderType.provided;
        }

        OnOrderReceived?.Invoke(roundCurrent, amount, orderType);
    }

    private void Socket_OnOrderMade(SocketIOResponse response)
    {
        OnOrderMade?.Invoke();
    }

    private void Socket_OnOrderOK(SocketIOResponse response)
    {
        OrderOnOkDTO[] dto = JsonConvert.DeserializeObject<OrderOnOkDTO[]>(response.ToString());

        int amount = dto[0].order;
        string roleString = dto[0].role;
        string orderTypeString = dto[0].type;
        bool done = dto[0].done;

        SupplierRole role = System.Enum.Parse<SupplierRole>(roleString);
        OrderType orderType = OrderType.requested;

        if (orderTypeString == "provided")
        {
            orderType = OrderType.provided;
        }

        OnOrderOK?.Invoke(amount, role, orderType);
    }

    private void Socket_OnOrderFail(SocketIOResponse response)
    {
        OnOrderFail?.Invoke();
    }

    public void MakeOrder(Order order)
    {
        EmitMakeOrder(order);
    }

    private void EmitMakeOrder(Order order)
    {
        string json = MakeJsonString(new 
        { 
            order = order.Amount,
            type = order.OrderType.ToString(),
            role = order.Supplier.Role.ToString(),
            done = order.Done
        }); 

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

    public void EndGame()
    {
        string json = MakeJsonString(new { });
        socket.EmitStringAsJSON("game:end", json);
    }

    private string MakeJsonString(object jsonObj)
    {
        return JsonConvert.SerializeObject(jsonObj);
    }
}
