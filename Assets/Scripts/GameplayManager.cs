using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    static GameplayManager instance;
    public static GameplayManager Instance {
        get { if (instance == null) instance = FindObjectOfType<GameplayManager>(); return instance; }
    }

    [SerializeField] GameObject gamePrefab = null;
    GameObject instantiatedGame;

    const byte BET_MADE = 1;
    const byte NEW_SPIN = 2;
    const byte CHIP_UPDATE = 3;

    int numOfPlayersBetted = 0;

    //public enum GameState
    //{
    //    WaitingForBets,
    //    Spinning,
    //    Results
    //}

    //public event Action<GameState> OnGameStateChange;
    public event Action<int> OnPlayerBet;
    public event Action<int, int, bool> OnOpponentUpdate;
    public event Action<bool> OnNewSpin;
    public event Action<bool, int, bool> OnRoundEnd;


    bool isCurrentRoundGreen;
    bool isPlayerBetGreen;
    int totalChips = 1000;
    int betAmount = 0;

    private void Start() {
        PhotonManager.Instance.OnReadyForNewGame += StartGame;
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    private void OnDestroy() {
        PhotonManager.Instance.OnReadyForNewGame -= StartGame;
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void StartGame() {
        Debug.Log("Starting Game!");
        instantiatedGame = GameObject.Instantiate(gamePrefab);
        instantiatedGame.transform.position = new Vector3(0, 0, 0);
    }

    public void OnBetSelected(bool isBetGreen, int betAmount, int leftChips) {
        isPlayerBetGreen = isBetGreen;
        this.betAmount = betAmount;
        totalChips = leftChips;

        OnPlayerBet?.Invoke(betAmount);

        numOfPlayersBetted++;
        object[] data = new object[] { betAmount, leftChips, isBetGreen };
        PhotonNetwork.RaiseEvent(BET_MADE, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
        CheckReadyToSpin();
    }

    void OnOtherBetSelected(int betAmount, int leftChips, bool isGreen) {
        OnOpponentUpdate?.Invoke(betAmount, leftChips, isGreen);
        numOfPlayersBetted++;
        CheckReadyToSpin();
    }

    void CheckReadyToSpin() {
        if (PhotonNetwork.IsMasterClient && numOfPlayersBetted == PhotonNetwork.CurrentRoom.PlayerCount) {
            // Spin!
            isCurrentRoundGreen = UnityEngine.Random.value > 0.5f;
            OnNewSpin?.Invoke(isCurrentRoundGreen);

            object[] data = new object[] { isCurrentRoundGreen };
            PhotonNetwork.RaiseEvent(NEW_SPIN, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
        }
    }

    public void OnSpinOver() {
        bool isWin = false;
        bool isToppingPlayerUp = false;
        if (isPlayerBetGreen == isCurrentRoundGreen) {
            isWin = true;
            totalChips += betAmount * 2; 
        }

        if (totalChips == 0) {
            totalChips = 100;
            isToppingPlayerUp = true;
        }


        numOfPlayersBetted = 0;
        OnRoundEnd?.Invoke(isWin, totalChips, isToppingPlayerUp);

        object[] data = new object[] { totalChips };
        PhotonNetwork.RaiseEvent(CHIP_UPDATE, data, Photon.Realtime.RaiseEventOptions.Default, ExitGames.Client.Photon.SendOptions.SendReliable);
    }



    private void NetworkingClient_EventReceived(ExitGames.Client.Photon.EventData obj) {
        object[] data = (object[])obj.CustomData;

        switch (obj.Code) {
            case BET_MADE:
                int betAmount = (int)data[0];
                int leftChips = (int)data[1];
                bool isOpponentBetGreen = (bool)data[2];
                OnOtherBetSelected(betAmount, leftChips, isOpponentBetGreen);
                break;
            case NEW_SPIN:
                isCurrentRoundGreen = (bool)data[0];
                OnNewSpin?.Invoke(isCurrentRoundGreen);
                break;
            case CHIP_UPDATE:
                int chipCount = (int)data[0];
                OnOpponentUpdate?.Invoke(0, chipCount, true);
                break;
            default:
                break;
        }
    }
}