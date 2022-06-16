using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class Room : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private bool isSinglePlayer;
    private Supplier[] suppliers;

    private MultiplayerController multiplayerController;
    private SupplierRole roleClient;

    public delegate void DelSetRole(SupplierRole role);
    public event DelSetRole OnSetRole;

    public delegate void DelAdvanceRound(int roundNumber);
    public event DelAdvanceRound OnAdvanceRound;

    private void Awake()
    {
        multiplayerController = GetComponent<MultiplayerController>();
    }

    public void Initialize(SocketIOUnity socket)
    {
        multiplayerController.Initialize(socket);
    }

    public void MakeOrder(Supplier supplier, int amount, OrderType orderType)
    {
        multiplayerController.MakeOrder(new Order(supplier, amount, orderType));
    }

    public void AdvanceRound(List<Order> orders, int roundCurrent)
    {
        GiveOrdersToSuppliers(orders);
        OnAdvanceRound?.Invoke(roundCurrent);
    }

    private void GiveOrdersToSuppliers(List<Order> orders)
    {
        Supplier[] suppliers = gameController.Suppliers;

        for (int i = 0; i < orders.Count; i++)
        {
            for (int i2 = 0; i2 < suppliers.Length; i2++)
            {
                if (orders[i].Supplier == suppliers[i2])
                {
                    suppliers[i2].ReceiveOrder(orders[i]);
                }
            }
        }
    }

    public void SetClientRole(SupplierRole role)
    {
        roleClient = role;
        multiplayerController.SelectRole(role);
    }

    public SupplierRole RolePlayer { get { return roleClient; } }

    public Supplier SupplierPlayer 
    { 
        get 
        {
            return gameController.GetSupplier(RolePlayer);
        } 
    }
    public Supplier[] Suppliers
    {
        get
        {
            return gameController.Suppliers;
        }
    }
}
