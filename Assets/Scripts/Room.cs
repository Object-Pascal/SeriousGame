using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class Room
{
    [SerializeField] private RoomUI roomUI;
    private SupplierRole roleClient;
    private IRoomConnection roomConnection;

    public delegate void DelRole(string message);
    public event DelRole OnRoleAssigned;
    public event DelRole OnRoleAssignOk;
    public event DelRole OnRoleAssignFail;

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
    }

    private void RoomConnection_OnRoleAssignOK(string message)
    {
        roomConnection.OnRoleAssigned -= RoomConnection_OnRoleAssigned;
        roomConnection.OnRoleAssignOK -= RoomConnection_OnRoleAssignOK;
        roomConnection.OnRoleAssignFail -= RoomConnection_OnRoleAssignFail;

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

    public void SelectRole(SupplierRole role)
    {
        roomConnection.SelectRole(role);
    }

    public void MakeOrder(Supplier supplier, int amount, OrderType orderType)
    {
        //multiplayerController.MakeOrder(new Order(supplier, amount, orderType));
    }

    public void AdvanceRound(List<Order> orders, int roundCurrent)
    {
        
    }

    public SupplierRole RolePlayer { get { return roleClient; } }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Uri { get; set; }
}
