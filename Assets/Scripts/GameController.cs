using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SocketIOClient;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class GameController : MonoBehaviour
{
    private Room room;
    private Supplier[] suppliers;
    private IRoomDAO roomDao;

    public delegate void DelRoomConnection(Room room);
    public event DelRoomConnection OnRoomConnectionSuccess;

    private void Awake()
    {
        UnityThread.initUnityThread();
        Supplier[] suppliers = Suppliers;

        roomDao = new HttpRoomDAO();
    }

    private void Start()
    {
        TryJoinRoom();
    }

    private async Task<Room[]> GetAvailableRooms()
    {
        return await roomDao.GetRoomsAvailable();
    }

    private async void TryJoinRoom()
    {
        Room[] roomsAvailable = await GetAvailableRooms();

        if (roomsAvailable.Length == 0)
        {
            Debug.Log("No rooms available");
            return;
        }

        Room roomToConnectTo = roomsAvailable[0];

        IRoomConnection roomConnection = new SocketIORoomConnection(roomToConnectTo);

        roomConnection.OnRoomConnectionSuccess += RoomConnection_OnRoomConnectionSuccess;
        roomConnection.OnRoomConnectionFail += RoomConnection_OnRoomConnectionError;
        roomConnection.OnError += RoomConnection_OnError;

        roomConnection.ConnectToRoom(roomToConnectTo);
    }

    private void RoomConnection_OnRoomConnectionSuccess(Room room, IRoomConnection connection)
    {
        UnityThread.executeInUpdate(() =>
        {
            connection.OnRoomConnectionSuccess -= RoomConnection_OnRoomConnectionSuccess;
            connection.OnRoomConnectionFail -= RoomConnection_OnRoomConnectionError;

            Debug.Log("Connection success to: " + room.Id);
            room.SetConnection(connection);
            room.SetGameController(this);
            this.room = room;

            for (int i = 0; i < Suppliers.Length; i++)
            {
                Suppliers[i].SetRoom(room);
            }
            Debug.Log("boop");

            OnRoomConnectionSuccess?.Invoke(room);
            Debug.Log("boop2");
        });
    }

    private void RoomConnection_OnRoomConnectionError(Room room, IRoomConnection connection)
    {
        UnityThread.executeInUpdate(() =>
        {
            connection.OnRoomConnectionSuccess -= RoomConnection_OnRoomConnectionSuccess;
            connection.OnRoomConnectionFail -= RoomConnection_OnRoomConnectionError;
            connection.OnError -= RoomConnection_OnError;

            Debug.Log("Connected fail to: " + room.Id);
        });
    }

    private void RoomConnection_OnError(string message)
    {
        Debug.Log(message);
    }

    public void SelectClientRole(SupplierRole role)
    {
        room.SelectRole(role);
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
}
