using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SocketIOClient;
using System;
using Newtonsoft.Json;

public class GameController : MonoBehaviour
{
    [SerializeField] private Room room;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private RoleSelectUIManager roleSelectionUI;
    private Supplier[] suppliers;
    private SocketIOUnity socket;

    private void Awake()
    {
        UnityThread.initUnityThread();
        Supplier[] suppliers = Suppliers;

        for (int i = 0; i < suppliers.Length; i++)
        {
            suppliers[i].SetRoom(room);
        }
    }

    private void Start()
    {
        TryJoinRoom();
    }

    private void TryJoinRoom()
    {
        var uri = new Uri(@"https://seahorse-app-artn4.ondigitalocean.app/?roomUri=2942d066-596d-4325-97d5-e9dede90357a");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    { "roomUri", "2942d066-596d-4325-97d5-e9dede90357a" }
                }
            ,
            EIO = 4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });

        Debug.Log(socket.ServerUri);

        socket.OnConnected += Socket_OnConnected;
        socket.OnError += Socket_OnError1;
        //socket.On("error", Socket_OnError);

        socket.OnUnityThread("error", (response) =>
        {
            Debug.Log("Error3: " + response.ToString());
        });

        socket.Connect();
    }

    private void Socket_OnError1(object sender, string e)
    {
        Debug.Log("Error2: " + sender.ToString());
    }

    private void Socket_OnError(SocketIOResponse obj)
    {
        Debug.Log("Error: " + obj.ToString());
    }

    private void Socket_OnConnected(object sender, EventArgs e)
    {
        Debug.Log("Connected socket");
        Debug.Log("1");
        Debug.Log("2");

        UnityThread.executeInUpdate(() => 
        {
            room.Initialize(socket);
            roleSelectionUI.gameObject.SetActive(true);
        });

        Debug.Log("3");
        Debug.Log("test");
    }

    public void SelectClientRole(SupplierRole role)
    {
        room.SetClientRole(role);
    }

    public Supplier GetSupplier(SupplierRole role)
    {
        for (int i = 0; i < Suppliers.Length; i++)
        {
            if (Suppliers[i].Role == role)
            {
                return Suppliers[i];
            }
        }

        return null;
    }

    public Supplier[] Suppliers
    {
        get
        {
            if (suppliers == null)
            {
                suppliers = GetComponentsInChildren<Supplier>();
            }

            return suppliers;
        }
    }

    public void SetGameUIActive(bool isActive)
    {
        gameUI.gameObject.SetActive(isActive);
    }

    public void SetRoleSelectionUIActive(bool isActive)
    {
        roleSelectionUI.gameObject.SetActive(isActive);
    }
}
