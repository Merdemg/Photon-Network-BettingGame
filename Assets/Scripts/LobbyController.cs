using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    [SerializeField] JoinRoomButton joinRoomButtonPrefab;
    [SerializeField] Transform buttonContainer;

    private void Start() {
        PhotonManager.Instance.JoinLobby();
    }

    public void OnBackButton() {
        PhotonManager.Instance.LeaveLobbyIfInLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        base.OnRoomListUpdate(roomList);


        foreach (var roomInfo in roomList) {
            JoinRoomButton button = Instantiate(joinRoomButtonPrefab, buttonContainer);
            button.Init(roomInfo.Name, this);
        }
    }

    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }
}
