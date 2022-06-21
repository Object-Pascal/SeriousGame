using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text txtSentiment;
    [SerializeField] private TMP_Text txtSender;
    [SerializeField] private TMP_Text txtMessage;

    private string message;
    private string from;
    private string sentiment;

    public void Initialize(string message, string from, string sentiment)
    {
        this.message = message;
        this.from = from;
        this.sentiment = sentiment;

        txtSentiment.text = sentiment;
        txtSender.text = from;
        txtMessage.text = message;

        if (from == "You")
        {
            txtSender.color = Color.yellow;
        }

        switch (sentiment)
        {
            case "Positive":
                txtSentiment.color = Color.green;
                break;
            case "Negative":
                txtSentiment.color = Color.red;
                break;
            case "Neutral":
                txtSentiment.color = Color.yellow;
                break;
            default:
                break;
        }
    }
}
