using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject connectingView = null;
    [SerializeField] GameObject gameTypeSelectionView = null;
    [SerializeField] GameObject hostView = null;
    [SerializeField] GameObject waitingForPlayerView = null;

    Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        PhotonManager.Instance.OnNetworkStatusChange += OnNetworkStatusChange;
        PhotonManager.Instance.OnReadyForNewGame += OnReadyForNewGame;
    }

    private void OnDestroy() {
        PhotonManager.Instance.OnNetworkStatusChange -= OnNetworkStatusChange;
        PhotonManager.Instance.OnReadyForNewGame -= OnReadyForNewGame;
    }

    void OnNetworkStatusChange(PhotonManager.NetworkStatus newStatus) {
        // Show connection options here. Join a lobby or join random game

        switch (newStatus) {
            case PhotonManager.NetworkStatus.Disconnected:
                connectingView.SetActive(true);
                gameTypeSelectionView.SetActive(false);
                waitingForPlayerView.SetActive(false);
                hostView.SetActive(false);
                break;
            case PhotonManager.NetworkStatus.ConnectedToPhotonServers:
                connectingView.SetActive(false);
                gameTypeSelectionView.SetActive(true);
                waitingForPlayerView.SetActive(false);
                hostView.SetActive(false);
                break;
            case PhotonManager.NetworkStatus.JoinedRoom:
                connectingView.SetActive(false);
                gameTypeSelectionView.SetActive(false);
                waitingForPlayerView.SetActive(true);
                hostView.SetActive(false);
                break;
            default:
                break;
        }
    }

    private void OnReadyForNewGame() {
        canvas.enabled = false;
    }

    // UI Button calls
    public void OnRandomGameSelected() {
        PhotonManager.Instance.FindRandomMatch();
    }

    public void OnBackButton() {
        PhotonManager.Instance.LeaveRoomIfInRoom();
    }
}
