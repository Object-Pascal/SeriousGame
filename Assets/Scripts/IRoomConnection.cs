using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IRoomConnection
{
    public delegate void DelRoomConnect(Room room, IRoomConnection connection);
    public event DelRoomConnect OnRoomConnectionSuccess;
    public event DelRoomConnect OnRoomConnectionFail;

    public delegate void DelRoleAssign(string message);
    public event DelRoleAssign OnRoleAssignOK;
    public event DelRoleAssign OnRoleAssignFail;
    public event DelRoleAssign OnRoleAssigned;

    public delegate void DelGameStarted(bool isChatEnabled);
    public event DelGameStarted OnGameStarted;
    public delegate void DelOrderReceived(int roundCurrent, int amount, OrderType orderType);
    public event DelOrderReceived OnOrderReceived;

    public delegate void DelOrderMade();
    public delegate void DelOrderOk(int amount, SupplierRole role, OrderType orderType, bool done);
    public event DelOrderMade OnOrderMade;
    public event DelOrderOk OnOrderOK;
    public event DelOrderMade OnOrderFail;

    public delegate void DelMessageReceived(string message, string sender, string sentiment);
    public event DelMessageReceived OnMessageReceived;
    public event DelMessageReceived OnMessageSent;

    public delegate void DelGameEnded(GameHistoryDTO history);
    public event DelGameEnded OnGameEnded;

    public delegate void DelError(string message);
    public event DelError OnError;

    public void ConnectToRoom(Room room);
    public void SelectRole(SupplierRole role);
    public void MakeOrder(Order order);
    public void SendChatMessage(string message, SupplierRole role);
    public void DisconnectFromRoom();
    public void ForceStartGame(bool isChatEnabled);
    public void EndGame();
}
