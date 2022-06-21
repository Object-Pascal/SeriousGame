using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class Room
{
    private GameController gameController;
    [SerializeField] private RoomUI roomUI;
    private SupplierRole roleClient;
    private IRoomConnection roomConnection;

    public delegate void DelRole(string message);
    public event DelRole OnRoleAssigned;
    public event DelRole OnRoleAssignOk;
    public event DelRole OnRoleAssignFail;

    public delegate void DelGameStarted(bool isChatEnabled);
    public event DelGameStarted OnGameStarted;
    public delegate void DelGameEnded(GameHistoryDTO history);
    public event DelGameEnded OnGameEnded;
    public delegate void DelOrderReceived(int roundCurrent, int amount, OrderType orderType);
    public event DelOrderReceived OnOrderReceived;

    public delegate void DelMessageReceived(string message, string sender, string sentiment);
    public event DelMessageReceived OnMessageReceived;
    public event DelMessageReceived OnMessageSent;

    public delegate void DelOrderOk(int amount, SupplierRole role, OrderType orderType, bool done);
    public event DelOrderOk OnOrderOK;
    public delegate void DelOrder();
    public event DelOrder OnOrderFail;
    private SupplierRole rolePending;

    public void SetConnection(IRoomConnection roomConnection)
    {
        this.roomConnection = roomConnection;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        roomConnection.OnRoleAssigned += RoomConnection_OnRoleAssigned;
        roomConnection.OnRoleAssignOK += RoomConnection_OnRoleAssignOK;
        roomConnection.OnRoleAssignFail += RoomConnection_OnRoleAssignFail;
        roomConnection.OnGameStarted += RoomConnection_OnGameStarted;
        roomConnection.OnGameEnded += RoomConnection_OnGameEnded;
        roomConnection.OnOrderReceived += RoomConnection_OnOrderReceived;
        roomConnection.OnOrderOK += RoomConnection_OnOrderOK;
        roomConnection.OnOrderFail += RoomConnection_OnOrderFail;
        roomConnection.OnMessageReceived += RoomConnection_OnMessageReceived;
        roomConnection.OnMessageSent += RoomConnection_OnMessageSent;
    }

    private void RoomConnection_OnMessageSent(string message, string sender, string sentiment)
    {
        OnMessageSent?.Invoke(message, sender, sentiment);
    }

    private void RoomConnection_OnMessageReceived(string message, string sender, string sentiment)
    {
        OnMessageReceived?.Invoke(message, sender, sentiment);
    }

    private void RoomConnection_OnGameEnded(GameHistoryDTO history)
    {
        OnGameEnded?.Invoke(history);
    }

    private void RoomConnection_OnOrderReceived(int roundCurrent, int amount, OrderType orderType)
    {
        SupplierPlayer.ReceiveOrder(new Order(SupplierPlayer, amount, orderType, false));
        OnOrderReceived?.Invoke(roundCurrent, amount, orderType);
    }

    private void RoomConnection_OnOrderOK(int amount, SupplierRole role, OrderType orderType, bool done)
    {
        OnOrderOK?.Invoke(amount, role, orderType, done);
    }

    private void RoomConnection_OnOrderFail()
    {
        OnOrderFail?.Invoke();
    }

    private void RoomConnection_OnRoleAssignOK(string message)
    {
        roomConnection.OnRoleAssigned -= RoomConnection_OnRoleAssigned;
        roomConnection.OnRoleAssignOK -= RoomConnection_OnRoleAssignOK;
        roomConnection.OnRoleAssignFail -= RoomConnection_OnRoleAssignFail;

        roleClient = rolePending;
        SupplierPlayer.SetAsPlayer();

        OnRoleAssignOk?.Invoke(message);
    }

    private void RoomConnection_OnRoleAssignFail(string message)
    {
        OnRoleAssignFail?.Invoke(message);
    }

    private void RoomConnection_OnRoleAssigned(string message)
    {
        OnRoleAssigned?.Invoke(message);
    }

    private void RoomConnection_OnGameStarted(bool isChatEnabled)
    {
        OnGameStarted?.Invoke(isChatEnabled);
    }

    public void SelectRole(SupplierRole role)
    {
        rolePending = role;
        roomConnection.SelectRole(role);
    }

    public Order MakeOrder(Supplier supplier, int amount, OrderType orderType, bool done)
    {
        Debug.Log($"Making order: to {supplier.Role}, amount {amount}, type {orderType}, done {done}");
        Order order = new Order(supplier, amount, orderType, done);
        roomConnection.MakeOrder(order);
        return order;
    }

    public void AdvanceRound(List<Order> orders, int roundCurrent)
    {
        
    }

    public void SendChatMessage(string message, SupplierRole roleSender)
    {
        roomConnection.SendChatMessage(message, roleSender);
    }

    public void ForceStartGame(bool isChatEnabled)
    {
        roomConnection.ForceStartGame(isChatEnabled);
    }

    public void EndGame()
    {
        roomConnection.EndGame();
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public SupplierRole RolePlayer { get { return roleClient; } }
    public Supplier SupplierPlayer { get { return gameController.GetSupplier(RolePlayer); } }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
    public GameController GameController { get { return gameController; } set { gameController = value; } }
}
