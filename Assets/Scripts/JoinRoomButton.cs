using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinRoomButton : MonoBehaviour
{
    string roomName;
    LobbyController lobbyController;

    [SerializeField] TMP_Text buttonText;

    public void Init(string roomName, LobbyController lobbyController) {
        this.roomName = roomName;
        this.lobbyController = lobbyController;

        buttonText.text = roomName;
    }

    public void OnButtonClicked() {
        lobbyController.JoinRoom(roomName);
    }
}
