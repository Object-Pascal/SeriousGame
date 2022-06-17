using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TMP_Text txtRole;
    [SerializeField] private TMP_Text txtRound;
    [SerializeField] private TMP_Text txtIncomingValue;
    [SerializeField] private TMP_InputField inputOutgoingValue;
    [SerializeField] private TMP_Text txtStock;
    [SerializeField] private TMP_Text txtBacklog;
    [SerializeField] private GameObject roleSelectionObj;
    private Button[] btnsRoleSelect;
    private Room room;

    private void Awake()
    {
        btnsRoleSelect = roleSelectionObj.GetComponentsInChildren<Button>(true);
        gameController.OnRoomConnectionSuccess += GameController_OnRoomConnectionSuccess;
    }

    private void GameController_OnRoomConnectionSuccess(Room room)
    {
        this.room = room;
        room.OnRoleAssigned += Room_OnRoleAssigned;
        room.OnRoleAssignOk += Room_OnRoleAssignOk;
        room.OnRoleAssignFail += Room_OnRoleAssignFail;
        roleSelectionObj.SetActive(true);
        DisableTakenRoles();
    }

    private void Room_OnRoleAssigned(string message)
    {
        Debug.Log("Role assigned");
        DisableTakenRoles();
    }

    private async void DisableTakenRoles()
    {
        DisableAllRoleBtns();

        IRoomDAO roomDao = new HttpRoomDAO();
        SupplierRole[] rolesTaken = await roomDao.GetRolesTakenInRoom(room);
        SetRoleBtnsInactive(rolesTaken);
    }

    private void Room_OnRoleAssignOk(string message)
    {
        Debug.Log("Role assign ok");
        roleSelectionObj.SetActive(false);
    }

    private void Room_OnRoleAssignFail(string message)
    {
        Debug.Log("Role assign fail: " + message);
    }

    private void UpdateStockAndBacklogAndRoundText()
    {
        //txtStock.text = "Stock: " + room.SupplierPlayer.Stock.ToString();
        //txtBacklog.text = "Backlog: " + room.SupplierPlayer.BackLog.ToString();
    }

    private void UpdateRoundText(int round)
    {
        txtRound.text = "Round: " + round;
    }

    public void MakePlayerOrder()
    {
        //room.MakeOrder(room.SupplierPlayer.SupplierRight, int.Parse(inputOutgoingValue.text), OrderType.Request);
    }

    public void SelectRole(string role)
    {
        SupplierRole roleSelected = Enum.Parse<SupplierRole>(role);
        room.SelectRole(roleSelected);
    }

    private void DisableAllRoleBtns()
    {
        for (int i = 0; i < btnsRoleSelect.Length; i++)
        {
            btnsRoleSelect[i].interactable = true;
        }
    }

    private void SetRoleBtnsInactive(SupplierRole[] roles)
    {
        for (int i = 0; i < btnsRoleSelect.Length; i++)
        {
            string txt = btnsRoleSelect[i].GetComponentInChildren<TextMeshProUGUI>().text;

            for (int i2 = 0; i2 < roles.Length; i2++)
            {
                try
                {
                    SupplierRole role = Enum.Parse<SupplierRole>(txt);

                    Debug.Log(txt + ":" + role.ToString());

                    if (role == roles[i2])
                    {
                        btnsRoleSelect[i].interactable = false;
                    }
                }
                catch
                {
                    Debug.Log("Error; cant parse button role to enum");
                }
            }
        }
    }
}
