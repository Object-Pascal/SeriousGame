using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SupplierRole
{
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
    private Room game;

    public void SetRoom(Room room)
    {
        this.game = room;
    }

    public void MakeOrder(int amount, OrderType orderType)
    {
        if (orderType == OrderType.Receive)
        {
            if (supplierLeft)
            {
                game.MakeOrder(supplierLeft, amount, OrderType.Receive);
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
                game.MakeOrder(supplierRight, amount, OrderType.Request);
            }
            else
            {
                game.MakeOrder(this, amount, OrderType.Receive);
            }
        }
    }

    public void ReceiveOrder(Order order)
    {
        if (order.OrderType == OrderType.Receive)
        {
            stock += order.Amount;
        }
        else
        {
            backLog += order.Amount;
        }
    }

    public SupplierRole Role { get { return role; } }
    public int Stock { get { return stock; } }
    public int BackLog { get { return backLog; } }
    public Room Game { get { return game; } }
    public Supplier SupplierLeft { get { return supplierLeft; } }
    public Supplier SupplierRight { get { return supplierRight; } }
}
