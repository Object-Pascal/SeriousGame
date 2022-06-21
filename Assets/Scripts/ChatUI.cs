using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUI : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private GameObject chatBox;
    [SerializeField] private Transform parentChatMessages;
    [SerializeField] private TMP_InputField txtMessageInput;
    [SerializeField] private ChatMessage chatMessageResource;
    [SerializeField] private ScrollRect scrollRect;
    private List<ChatMessage> chatMessages;
    private Room room;

    private void Awake()
    {
        chatMessages = new List<ChatMessage>();
        gameController.OnRoomConnectionSuccess += GameController_OnRoomConnectionSuccess;
    }

    private void GameController_OnRoomConnectionSuccess(Room room)
    {
        this.room = room;
        room.OnMessageReceived += Room_OnMessageReceived;
        room.OnMessageSent += Room_OnMessageReceived;
        room.OnGameStarted += Room_OnGameStarted;
    }

    private void Room_OnGameStarted(bool isChatEnabled)
    {
        if (isChatEnabled)
        {
            chatBox.SetActive(true);
        }    
    }

    private void Room_OnMessageReceived(string message, string sender, string sentiment)
    {
        UnityThread.executeInLateUpdate(() =>
        {
            ChatMessage chatMessageInstance = Instantiate(chatMessageResource, parentChatMessages);
            chatMessageInstance.Initialize(message, sender + ": ", sentiment);
            chatMessages.Add(chatMessageInstance);

            Canvas.ForceUpdateCanvases();

            parentChatMessages.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            //parentChatMessages.GetComponent<ContentSizeFitter>().SetLayoutVertical();

            scrollRect.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            //scrollRect.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();

            scrollRect.verticalNormalizedPosition = 0;
        });
    }

    public void SendChatMessage()
    {
        room.SendChatMessage(txtMessageInput.text, room.RolePlayer);
    }
}
