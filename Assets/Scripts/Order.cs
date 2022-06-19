using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OrderType
{
    Request,
    Receive
}

[System.Serializable]
public class Order
{
    [SerializeField] private Supplier supplier;
    [SerializeField] private int amount;
    [SerializeField] private OrderType orderType;
    [SerializeField] private bool done;

    public Order(Supplier supplier, int amount, OrderType orderType, bool done)
    {
        this.supplier = supplier;
        this.amount = amount;
        this.orderType = orderType;
        this.done = done;
    }

    public Supplier Supplier { get { return supplier; } }
    public int Amount { get { return amount; } }
    public OrderType OrderType { get { return orderType; } }
    public bool Done { get { return done; } }
}

