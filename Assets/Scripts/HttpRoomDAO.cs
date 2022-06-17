using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class HttpRoomDAO : IRoomDAO
{
    private string httpString = @"https://seahorse-app-artn4.ondigitalocean.app/";
    private readonly HttpClient client = new HttpClient();

    public Task<Room> CreateRoom(string name)
    {
        throw new System.NotImplementedException();
    }

    public async Task<SupplierRole[]> GetRolesTakenInRoom(Room room)
    {
        string endPoint = "roles-unavailable?roomUri=" + room.Id;

        HttpResponseMessage response = await client.GetAsync(httpString + endPoint);
        string content = await response.Content.ReadAsStringAsync();
        SupplierRole[] rolesTaken = JsonConvert.DeserializeObject<SupplierRole[]>(content);

        return rolesTaken;
    }

    public async Task<Room[]> GetRoomsAvailable()
    {
        string endPoint = "rooms";

        HttpResponseMessage response = await client.GetAsync(httpString + endPoint);
        string content = await response.Content.ReadAsStringAsync();
        Room[] rooms = JsonConvert.DeserializeObject<Room[]>(content);

        return rooms;
    }
}
