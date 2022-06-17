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

    public delegate void DelError(string message);
    public event DelError OnError;

    public void ConnectToRoom(Room room);
    public void SelectRole(SupplierRole role);
    public Task<bool> MakeOrder(Order order);
    public void DisconnectFromRoom();
}
