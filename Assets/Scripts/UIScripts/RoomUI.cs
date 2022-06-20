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
        room.OnGameEnded += Room_OnGameEnded;
        room.OnOrderReceived += Room_OnOrderReceived;
        room.OnOrderOK += Room_OnOrderOK;
        room.OnOrderFail += Room_OnOrderFail;
        roleSelectionObj.SetActive(true);
        DisableTakenRoles();
    }

    private void Room_OnGameEnded(GameHistoryDTO history)
    {
        Debug.Log("---OnGameEnded:---");

        Debug.Log(history.message);

        Debug.Log("Round count:" + history.history.Length);

        for (int i = 0; i < history.history.Length; i++)
        {
            RoundDTO round = history.history[i];
            Debug.Log("Round: " + i);

            for (int i2 = 0; i2 < round.orders.Length; i2++)
            {
                OrderDTO order = round.orders[i2];
                Debug.Log("Order: to " + order.role + ", amount " + order.order + ", type " + order.type);
            }
        }
    }

    private void Room_OnOrderReceived(int roundCurrent, int amount, OrderType orderType)
    {
        UnityThread.executeInLateUpdate(() =>
        {
            RoomOnOrderReceived(roundCurrent, amount, orderType);
        });
    }

    private void RoomOnOrderReceived(int roundCurrent, int amount, OrderType orderType)
    {
        txtIncomingValue.text = "0";
        UpdateStockAndBacklogAndRoundText();
        UpdateRoundText(roundCurrent);

        if (orderType == OrderType.requested)
        {
            txtIncomingValue.text = amount.ToString();
        }

        //btnSendOrder.GetComponentInChildren<TMP_Text>().text = "Send";
        btnSendOrder.interactable = true;

        inputOutgoingValue.text = "0";
        //inputOutgoingValue.interactable = true;
    }

    private void Room_OnOrderOK(int amount, SupplierRole role, OrderType orderType)
    {
        UnityThread.executeInLateUpdate(() =>
        {
            RoomOnOrderOk(amount, role, orderType);
        });
    }

    private void RoomOnOrderOk(int amount, SupplierRole role, OrderType orderType)
    {
        Debug.Log("RoomUI: OK");
        //btnSendOrder.GetComponentInChildren<TMP_Text>().text = "SENT!";
        //inputOutgoingValue.interactable = false;
        UpdateStockAndBacklogAndRoundText();
    }

    private void Room_OnOrderFail()
    {
        //btnSendOrder.GetComponentInChildren<TMP_Text>().text = "Send";
        //btnSendOrder.interactable = true;
    }

    private void Room_OnGameStarted(string message)
    {
        roleSelectionObj.SetActive(false);
        waitForPlayersObj.SetActive(false);
        hudObj.SetActive(true);
        txtRound.text = "Round: 1";
        txtIncomingValue.text = "-";
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
        string roleName = room.RolePlayer.ToString();
        roleName = char.ToUpper(roleName[0]) + roleName.Substring(1);
        txtRole.text = roleName;
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
        //btnSendOrder.GetComponentInChildren<TMP_Text>().text = "Sending...";
        btnSendOrder.interactable = false;
        Supplier supplierPlayer = GetSupplierPlayer();
        supplierPlayer.MakeOrder(int.Parse(inputOutgoingValue.text), OrderType.requested, true);
    }

    public void SelectRole(string role)
    {
        SupplierRole roleSelected = Enum.Parse<SupplierRole>(role.ToLower());
        room.SelectRole(roleSelected);
    }

    public void EndGame()
    {
        room.EndGame();
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
