using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Room room;
    [SerializeField] private TMP_Text txtRole;
    [SerializeField] private TMP_Text txtRound;
    [SerializeField] private TMP_Text txtIncomingValue;
    [SerializeField] private TMP_InputField inputOutgoingValue;
    [SerializeField] private TMP_Text txtStock;
    [SerializeField] private TMP_Text txtBacklog;

    private void Awake()
    {
        room.OnSetRole += GameController_OnSetRole;
        room.OnAdvanceRound += Game_OnAdvanceRound;
    }

    private void Game_OnAdvanceRound(int roundCurrent)
    {
        UpdateRoundText(roundCurrent);
        UpdateStockAndBacklogAndRoundText();
    }

    private void GameController_OnSetRole(SupplierRole role)
    {
        UpdateRoleText(role);
    }

    private void UpdateRoleText(SupplierRole role)
    {
        txtRole.text = role.ToString();
    }

    private void UpdateStockAndBacklogAndRoundText()
    {
        txtStock.text = "Stock: " + room.SupplierPlayer.Stock.ToString();
        txtBacklog.text = "Backlog: " + room.SupplierPlayer.BackLog.ToString();
    }

    private void UpdateRoundText(int round)
    {
        txtRound.text = "Round: " + round;
    }

    public void MakePlayerOrder()
    {
        room.MakeOrder(room.SupplierPlayer.SupplierRight, int.Parse(inputOutgoingValue.text), OrderType.Request);
    }
}
