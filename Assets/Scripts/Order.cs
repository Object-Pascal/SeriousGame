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

    public Order(Supplier supplier, int amount, OrderType orderType)
    {
        this.supplier = supplier;
        this.amount = amount;
        this.orderType = orderType;
    }

    public Supplier Supplier { get { return supplier; } }
    public int Amount { get { return amount; } }
    public OrderType OrderType { get { return orderType; } }
}

