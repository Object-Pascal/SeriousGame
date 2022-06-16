using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerController : RoomController
{
    [SerializeField] private List<Order> ordersTotal;
    [SerializeField] private List<Order> ordersRound;
    private int roundCurrent = 0;

    public override void Initialize(Room game)
    {
        base.Initialize(game);

        roundCurrent = 1;
        CreateComputerOrders();
    }

    public override void MakeOrder(Order order)
    {
        ordersTotal.Add(order);
        ordersRound.Add(order);

        if (ordersRound.Count >= 4)
        {
            roundCurrent++;
            List<Order> orders = new List<Order>();
            orders.AddRange(ordersRound);
            room.AdvanceRound(orders, roundCurrent);
            ordersRound.Clear();
            CreateComputerOrders();
        }
    }

    private void CreateComputerOrders()
    {
        Supplier[] suppliers = room.Suppliers;
        Supplier supplierPlayer = room.SupplierPlayer;

        for (int i = 0; i < suppliers.Length; i++)
        {
            if (suppliers[i] == supplierPlayer)
            { 
                continue;
            }

            int backLog = suppliers[i].BackLog;
            int stock = suppliers[i].Stock;
            int amountToGive = 0;

            if (backLog > 0)
            {
                if (stock <= backLog)
                {
                    amountToGive = stock;
                }
                else
                {
                    amountToGive = backLog;
                }

                if (amountToGive > 0)
                {
                    suppliers[i].MakeOrder(amountToGive, OrderType.Receive);
                    continue;
                }
            }

            int backLogLeft = backLog - amountToGive;
            suppliers[i].MakeOrder(backLogLeft, OrderType.Request);
        }
    }
}
