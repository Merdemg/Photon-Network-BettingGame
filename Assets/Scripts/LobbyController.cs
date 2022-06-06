using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] JoinRoomButton joinRoomButtonPrefab;
    [SerializeField] Transform buttonContainer;

    List<JoinRoomButton> buttons = new List<JoinRoomButton>();

    public override void OnEnable() {
        base.OnEnable();
        PhotonManager.Instance.JoinLobby();
    }

    public void OnBackButton() {
        PhotonManager.Instance.LeaveLobbyIfInLobby();
        CleanPreviousButtons();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        base.OnRoomListUpdate(roomList);

        foreach (var roomInfo in roomList) {
            JoinRoomButton button = Instantiate(joinRoomButtonPrefab, buttonContainer);
            button.Init(roomInfo.Name, this);
            buttons.Add(button);
        }
    }

    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    void CleanPreviousButtons() {
        foreach (var item in buttons) {
            Destroy(item.gameObject);
        }
        buttons = new List<JoinRoomButton>();
    }
}
