using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoleSelectUIManager : MonoBehaviour
{
    public TextMeshProUGUI header;

    public TextMeshProUGUI customer;
    public Button customerButton;

    public TextMeshProUGUI retailer;
    public Button retailerButton;

    public TextMeshProUGUI distributor;
    public Button distributorButton;

    public TextMeshProUGUI wholesaler;
    public Button wholesalerButton;

    public void ChangeHeaderText(string text)
    {
        header.text = text;
    }

    public void ChangeCustomerText(string text)
    {
        customer.text = text;
    }

    public void DisableCustomer()
    {
        customerButton.gameObject.SetActive(false);
    }

    public void ChangeRetailerText(string text)
    {
        retailer.text = text;
    }

    public void DisableRetailer()
    {
        retailerButton.gameObject.SetActive(false);
    }

    public void ChangeDistributorText(string text)
    {
        distributor.text = text;
    }

    public void DisableDistributor()
    {
        distributorButton.gameObject.SetActive(false);
    }

    public void ChangeWholesalerText(string text)
    {
        wholesaler.text = text;
    }

    public void DisableWholesaler()
    {
        wholesalerButton.gameObject.SetActive(false);
    }

    public void SelectRole(string role)
    {
        Debug.Log($"{role} Selected");
    }
}
