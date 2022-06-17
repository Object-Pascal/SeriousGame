using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IRoomDAO
{
    public Task<Room> CreateRoom(string name);
    public Task<Room[]> GetRoomsAvailable();
    public Task<SupplierRole[]> GetRolesTakenInRoom(Room room);
}
