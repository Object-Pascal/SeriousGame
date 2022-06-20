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
    [SerializeField] private Button btnSendOrder;
    [SerializeField] private TMP_Text txtStock;
    [SerializeField] private TMP_Text txtBacklog;
    [SerializeField] private GameObject roleSelectionObj;
    [SerializeField] private GameObject waitForPlayersObj;
    [SerializeField] private GameObject hudObj;
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
        room.OnGameStarted += Room_OnGameStarted;
        room.OnOrderReceived += Room_OnOrderReceived;
        room.OnOrderOK += Room_OnOrderOK;
        room.OnOrderFail += Room_OnOrderFail;
        roleSelectionObj.SetActive(true);
        DisableTakenRoles();
    }

    private void Room_OnOrderReceived(int roundCurrent, int amount, OrderType orderType)
    {
        UnityThread.executeInUpdate(() =>
        {
            UpdateStockAndBacklogAndRoundText();
            UpdateRoundText(roundCurrent);
            btnSendOrder.GetComponentInChildren<TMP_Text>().text = "Send";
            btnSendOrder.interactable = true;
        });
    }

    private void Room_OnOrderOK(int amount, SupplierRole role, OrderType orderType)
    {
        UnityThread.executeInUpdate(() =>
        {
            Debug.Log("RoomUI: OK");
            btnSendOrder.GetComponentInChildren<TMP_Text>().text = "SENT!";
            UpdateStockAndBacklogAndRoundText();
        });
    }

    private void Room_OnOrderFail()
    {
        btnSendOrder.GetComponentInChildren<TMP_Text>().text = "Send";
        btnSendOrder.interactable = true;
    }

    private void Room_OnGameStarted(string message)
    {
        roleSelectionObj.SetActive(false);
        waitForPlayersObj.SetActive(false);
        hudObj.SetActive(true);
        UpdateHud();
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
        waitForPlayersObj.SetActive(true);
    }

    private void Room_OnRoleAssignFail(string message)
    {
        Debug.Log("Role assign fail: " + message);
    }

    private void UpdateHud()
    {
        UpdateStockAndBacklogAndRoundText();
    }

    private void UpdateStockAndBacklogAndRoundText()
    {
        Supplier supplierPlayer = GetSupplierPlayer();
        txtStock.text = "Stock: " + supplierPlayer.Stock.ToString();
        txtBacklog.text = "Backlog: " + supplierPlayer.BackLog.ToString();
    }

    private void UpdateRoundText(int round)
    {
        txtRound.text = "Round: " + round;
    }

    public void MakePlayerOrder()
    {
        btnSendOrder.GetComponentInChildren<TMP_Text>().text = "Sending...";
        btnSendOrder.interactable = false;
        Supplier supplierPlayer = GetSupplierPlayer();
        supplierPlayer.MakeOrder(int.Parse(inputOutgoingValue.text), OrderType.requested, true);
    }

    public void SelectRole(string role)
    {
        SupplierRole roleSelected = Enum.Parse<SupplierRole>(role.ToLower());
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
            txt = txt.ToLower();

            for (int i2 = 0; i2 < roles.Length; i2++)
            {
                try
                {
                    SupplierRole role = Enum.Parse<SupplierRole>(txt);

                    if (role == roles[i2])
                    {
                        btnsRoleSelect[i].interactable = false;
                    }
                }
                catch
                {
                    Debug.Log("Error; cant parse button role to enum: " + txt);
                }
            }
        }
    }

    public void ForceStartGame()
    {
        UnityThread.executeInUpdate(() =>
        {
            room.ForceStartGame();
        });
    }

    private Supplier GetSupplierPlayer()
    {
        return room.SupplierPlayer;
    }
}
