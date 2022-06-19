using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SupplierRole
{
    Customer,
    Retailer,
    WholeSaler,
    Distributor,
    Manufacturer
}

public class Supplier : MonoBehaviour
{
    [SerializeField] private SupplierRole role;
    [SerializeField] private int stock;
    [SerializeField] private int backLog;
    [SerializeField] private Supplier supplierLeft;
    [SerializeField] private Supplier supplierRight;
    private Room room;

    public void SetRoom(Room room)
    {
        this.room = room;
    }

    public void SetAsPlayer()
    {
        room.OnOrderOK += Room_OnOrderOK;
    }

    public void MakeOrder(int amount, OrderType orderType, bool done)
    {
        if (orderType == OrderType.Receive)
        {
            if (supplierLeft)
            {
                room.MakeOrder(supplierLeft, amount, OrderType.Receive, done);
            }
            else
            {
                //??
            }
        }
        else
        {
            if (supplierRight)
            {
                room.MakeOrder(supplierRight, amount, OrderType.Request, done);
            }
            else
            {
                room.MakeOrder(this, amount, OrderType.Receive, done);
            }
        }
    }

    private void Room_OnOrderOK(int amount, SupplierRole role, OrderType orderType)
    {
        Debug.Log("onOk at " + this.role + ": " + role.ToString() + ":" + orderType.ToString() + ":" + amount);

        if (orderType == OrderType.Receive)
        {
            stock -= amount;
            backLog -= amount;
        }
    }

    public void ReceiveOrder(Order order)
    {
        if (order.Amount != 0)
        {
            if (order.OrderType == OrderType.Receive)
            {
                Debug.Log("added stock: " + order.Amount);
                stock += order.Amount;
            }
            else
            {
                Debug.Log("added backlog: " + order.Amount);
                backLog += order.Amount;
            }
        }

        HandleBacklog();
    }

    private void HandleBacklog()
    {
        Debug.Log("Handle backlog: stock: " + Stock + ", backlog: " + backLog);

        if (backLog > 0 && stock > 0)
        {
            if (stock <= backLog)
            {
                MakeOrder(stock, OrderType.Receive, false);
            }
            else
            {
                MakeOrder(backLog, OrderType.Receive, false);
            }
        }
    }

    public SupplierRole Role { get { return role; } }
    public int Stock { get { return stock; } set { stock = value; } }
    public int BackLog { get { return backLog; } set { backLog = value; } }
    public Room Game { get { return room; } }
    public Supplier SupplierLeft { get { return supplierLeft; } }
    public Supplier SupplierRight { get { return supplierRight; } }
}
