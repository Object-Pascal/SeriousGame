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
    public bool ok { get; set; }
    public int order { get; set; }
    public string type { get; set; }
    public string role { get; set; }
    public bool done { get; set; }
}

public class ChatMessageDTO
{
    public bool ok { get; set; }
    public string message { get; set; }
    public string from { get; set; }
    public SentimentDTO sentiment { get; set; }
}

public class SentimentDTO
{
    public string label { get; set; }
}

public class RoleAssignCallbackDTO
{
    public bool ok { get; set; }
    public string message { get; set; }
}

public class GameStartedDTO
{
    public bool chat { get; set; }
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
    public event IRoomConnection.DelGameStarted OnGameStarted;
    public event IRoomConnection.DelGameEnded OnGameEnded;
    public event IRoomConnection.DelOrderReceived OnOrderReceived;
    public event IRoomConnection.DelOrderMade OnOrderMade;
    public event IRoomConnection.DelOrderOk OnOrderOK;
    public event IRoomConnection.DelOrderMade OnOrderFail;
    public event IRoomConnection.DelMessageReceived OnMessageReceived;
    public event IRoomConnection.DelMessageReceived OnMessageSent;

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
        }, SocketIOUnity.UnityThreadScope.LateUpdate);

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
        socket.OnUnityThread("role:assigned", Socket_OnRoleAssigned);
        socket.OnUnityThread("game:started", Socket_OnGameStarted);
        socket.OnUnityThread("game:next", Socket_OnOrderReceived);
        socket.OnUnityThread("game:end", Socket_OnGameEnded);
        socket.OnUnityThread("invoice:added", Socket_OnOrderMade);
        socket.OnUnityThread("round:message-receive", Socket_OnMessageReceive);
    }

    private void Socket_OnMessageReceive(SocketIOResponse response)
    {
        ChatMessageDTO[] dto = JsonConvert.DeserializeObject<ChatMessageDTO[]>(response.ToString());

        string sentiment = dto[0].sentiment.label;

        switch (sentiment)
        {
            case "pos":
                sentiment = "Positive";
                break;
            case "neg":
                sentiment = "Negative";
                break;
            case "neutral":
                sentiment = "Neutral";
                break;
            default:
                break;
        }

        string sender = dto[0].from;
        sender = char.ToUpper(sender[0]) + sender.Substring(1);
        OnMessageReceived?.Invoke(dto[0].message, sender, sentiment);
    }

    private void Socket_OnGameEnded(SocketIOResponse response)
    {
        GameHistoryDTO[] dtos = JsonConvert.DeserializeObject<GameHistoryDTO[]>(response.ToString());

        OnGameEnded?.Invoke(dtos[0]);
    }

    private void Socket_OnRoleAssignCallback(SocketIOResponse response)
    {
        RoleAssignCallbackDTO[] dto = JsonConvert.DeserializeObject<RoleAssignCallbackDTO[]>(response.ToString());

        if (dto[0].ok == true)
        {
            OnRoleAssignOK?.Invoke(response.ToString());
        }
        else
        {
            OnRoleAssignFail?.Invoke(response.ToString());
        }
    }

    private void Socket_OnRoleAssigned(SocketIOResponse response)
    {
        OnRoleAssigned?.Invoke(response.ToString());
    }

    private void Socket_OnGameStarted(SocketIOResponse response)
    {
        GameStartedDTO[] dto = JsonConvert.DeserializeObject<GameStartedDTO[]>(response.ToString());

        OnGameStarted?.Invoke(dto[0].chat);
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

    private void Socket_OnOrderCallback(SocketIOResponse response)
    {
        OrderOnOkDTO[] dto = JsonConvert.DeserializeObject<OrderOnOkDTO[]>(response.ToString());

        if (!dto[0].ok)
        {
            OnOrderFail?.Invoke();
            return;
        }

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

        OnOrderOK?.Invoke(amount, role, orderType, done);
    }

    public void MakeOrder(Order order)
    {
        EmitMakeOrder(order);
    }

    private async void EmitMakeOrder(Order order)
    {
        await socket.EmitAsync("round:invoice", Socket_OnOrderCallback, new[] { new
        {
            order = order.Amount,
            type = order.OrderType.ToString(),
            role = order.Supplier.Role.ToString(),
            done = order.Done
        }
        });
    }

    public void SelectRole(SupplierRole role)
    {
        EmitSelectRole(role);
    }

    private async void EmitSelectRole(SupplierRole role)
    {
        await socket.EmitAsync("role:assign", Socket_OnRoleAssignCallback, new[] { new { role = role.ToString().ToLower() } });
    }

    public void DisconnectFromRoom()
    {
        //string json = MakeJsonString(new { role = role.ToString().ToLower() });
        //socket.EmitStringAsJSON("role:assign", json);
    }

    public void ForceStartGame(bool isChatEnabled)
    {
        string json = MakeJsonString(new { chat = isChatEnabled });
        socket.EmitStringAsJSON("game:start", json);
    }

    public void SendChatMessage(string message, SupplierRole roleSender)
    {
        EmitChatMessage(message, roleSender);
    }

    private async void EmitChatMessage(string message, SupplierRole roleSender)
    {
        await socket.EmitAsync("round:message", Socket_OnSendChatMessageCallback, new[] { new { lang = "dutch",
                                                                                                message = message} });
    }

    private void Socket_OnSendChatMessageCallback(SocketIOResponse response)
    {
        ChatMessageDTO[] dto = JsonConvert.DeserializeObject<ChatMessageDTO[]>(response.ToString());
        string sentiment = dto[0].sentiment.label;

        switch (sentiment)
        {
            case "pos":
                sentiment = "Positive";
                break;
            case "neg":
                sentiment = "Negative";
                break;
            case "neutral":
                sentiment = "Neutral";
                break;
            default:
                break;
        }

        OnMessageSent?.Invoke(dto[0].message, "You", sentiment);
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
