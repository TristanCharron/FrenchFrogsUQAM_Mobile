using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkObjectManager : NetworkBehaviour {

   [Command]
    public void CmdTransferRocket(float x, float rotX, float rotY, float speed, int currentSprite)
    {
        Debug.Log(x);
        Debug.Log(rotX);
        Debug.Log(rotY);
        Debug.Log(speed);
        Debug.Log(currentSprite);
    }
}
