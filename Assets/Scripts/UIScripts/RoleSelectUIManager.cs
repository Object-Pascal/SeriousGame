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

    public void SelectRole(string role)
    {
        Debug.Log($"{role} Selected");
    }
}
