using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using System;
using Newtonsoft;
using Newtonsoft.Json;

public class MultiplayerController : MonoBehaviour
{
    private string connectionString;
    private SocketIOUnity socket;
    private Room room;

    public void Initialize(SocketIOUnity socket)
    {
        this.socket = socket;
        room = GetComponent<Room>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        socket.OnUnityThread("game:next", ClientOnNextRound);
    }

    private void UnSubscribeFromServer()
    {

    }

    public void SelectRole(SupplierRole role)
    {
        string json = JsonConvert.SerializeObject(new { role = role.ToString().ToLower() });
        socket.Emit("role:assign", json);
    }

    public void MakeOrder(Order order)
    {
        
    }

    private void EmitOrder(Order order)
    {

    }

    private void ClientOnNextRound(SocketIOResponse obj)
    {
        Debug.Log("Next round");
    }
}
