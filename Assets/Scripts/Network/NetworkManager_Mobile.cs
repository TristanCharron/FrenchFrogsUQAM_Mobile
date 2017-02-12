using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkManager_Mobile : NetworkManager {


    public delegate void NetworkEvent();
    public static event NetworkEvent OnClientConnected, OnClientDisconnected;

    public override void OnClientConnect(NetworkConnection conn)
    {
        if(OnClientConnected != null)
            OnClientConnected();

        base.OnClientConnect(conn);
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        if(OnClientDisconnected != null)
            OnClientDisconnected();

        base.OnClientDisconnect(conn);
    }

    void Awake()
    {
        if (Application.isEditor)
            StartServer();
        else
            StartClient();
    }

}
