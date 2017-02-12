using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetManager : MonoBehaviour {

    bool isOutOfScreen = false;

    void OnBecameInvisible()
    {
        if(!isOutOfScreen && GetComponentInParent<Rocket>() != null)
        {
            isOutOfScreen = true;
            GetComponentInParent<Rocket>().DestroyRocket();

        }
     
    }


}
