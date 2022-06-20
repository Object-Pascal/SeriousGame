using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SupplierRole
{
    customer,
    retailer,
    wholesaler,
    distributor,
    manufacturer
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
        if (orderType == OrderType.provided)
        {
            if (supplierLeft)
            {
                room.MakeOrder(supplierLeft, amount, OrderType.provided, done);
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
                room.MakeOrder(supplierRight, amount, OrderType.requested, done);
            }
            else
            {
                room.MakeOrder(this, amount, OrderType.provided, done);
            }
        }
    }

    private void Room_OnOrderOK(int amount, SupplierRole role, OrderType orderType)
    {
        Debug.Log("onOk at " + this.role + ": " + role.ToString() + ":" + orderType.ToString() + ":" + amount);

        if (orderType == OrderType.provided)
        {
            stock -= amount;
            backLog -= amount;
        }
    }

    public void ReceiveOrder(Order order)
    {
        Debug.Log($"Receved order: to {order.Supplier.role}, amount {order.Amount}, type {order.OrderType}, done {order.Done}");

        if (order.Amount != 0)
        {
            if (order.OrderType == OrderType.provided)
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
                MakeOrder(stock, OrderType.provided, false);
            }
            else
            {
                MakeOrder(backLog, OrderType.provided, false);
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
