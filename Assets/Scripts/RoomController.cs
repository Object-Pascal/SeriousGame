using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomController : MonoBehaviour
{
    protected Room room;

    public virtual void Initialize(Room room)
    {
        this.room = room;
    }

    public abstract void MakeOrder(Order order);
}
