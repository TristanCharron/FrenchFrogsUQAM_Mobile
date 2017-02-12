using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkObjectManager : NetworkBehaviour {

    // Use this for initialization
    void Start () {
        Debug.Log("Delegate Created");
        RocketManager.OnDestroyRocket += SendCommand;
    }

    void SendCommand()
    {
        Rocket curRocket = RocketManager.currentRocket;
        CmdTransferRocket(curRocket.gameObject.transform.position.x,
            curRocket.gameObject.transform.rotation.x, 
            curRocket.gameObject.transform.rotation.y,
            Rocket.Speed,
            curRocket.CurrentSprite
            );
    }


    
   [Command]
    void CmdTransferRocket(float x, float rotX, float rotY, float speed, int currentSprite)
    {
        Debug.Log(x);
        Debug.Log(rotX);
        Debug.Log(rotY);
        Debug.Log(speed);
        Debug.Log(currentSprite);
    }
}
