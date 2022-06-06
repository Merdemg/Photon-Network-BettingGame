using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestConnect : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.GameVersion =
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster() {
        base.OnConnectedToMaster();
        Debug.Log("Connected.");

    }

    public override void OnDisconnected(DisconnectCause cause) {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected: " + cause.ToString());

    }
}
