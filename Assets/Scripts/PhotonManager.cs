using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    static PhotonManager instance;
    public static PhotonManager Instance {
        get { if (instance == null) instance = FindObjectOfType<PhotonManager>(); return instance; }
    }

    const int playerCountNeeded = 2;

    public enum NetworkStatus
    {
        Disconnected,
        ConnectedToPhotonServers,
        JoinedRoom
    }

    public NetworkStatus networkStatus { private set; get; } = NetworkStatus.Disconnected;
    public event Action<NetworkStatus> OnNetworkStatusChange;
    public event Action OnReadyForNewGame;


    private void Start() {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        Debug.Log("Connected.");
        UpdateNetworkStatus(NetworkStatus.ConnectedToPhotonServers);
    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected: " + cause.ToString());

    }

    public override void OnJoinedRoom() {
        base.OnJoinedRoom();
        UpdateNetworkStatus(NetworkStatus.JoinedRoom);

        if (PhotonNetwork.CurrentRoom.PlayerCount == playerCountNeeded) {
            OnReadyForNewGame?.Invoke();
        }
    }

    public override void OnCreatedRoom() {
        base.OnCreatedRoom();
        UpdateNetworkStatus(NetworkStatus.JoinedRoom);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.CurrentRoom.PlayerCount == playerCountNeeded) {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            OnReadyForNewGame?.Invoke();
        }
    }

    void UpdateNetworkStatus(NetworkStatus newStatus) {
        networkStatus = newStatus;
        OnNetworkStatusChange?.Invoke(newStatus);
    }

    public void FindRandomMatch() {
        PhotonNetwork.JoinRandomOrCreateRoom(null, 0, MatchmakingMode.FillRoom, null, null, "Random Game");
    }

    public void StartHosting(string roomName) {
        PhotonNetwork.CreateRoom(roomName);
    }

    public void LeaveRoomIfInRoom() {
        if (PhotonNetwork.InRoom) {
            PhotonNetwork.LeaveRoom();
        }
    }

    public void JoinLobby() {
        PhotonNetwork.JoinLobby();
    }

    public void LeaveLobbyIfInLobby() {
        if (PhotonNetwork.InLobby) {
            PhotonNetwork.LeaveLobby();
        }
    }
}
