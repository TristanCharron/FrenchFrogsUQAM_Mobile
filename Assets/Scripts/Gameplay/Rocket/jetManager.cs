using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jetManager : MonoBehaviour {

    bool isOutOfScreen = false,CanLerp = false;
    float t = 0;
    float length = 0.5f;

    SpriteRenderer sRenderer;
    Color cBegin, cEnd;

    void Awake()
    {
        cBegin = new Color(1, 1, 1, 0);
        cEnd = Color.white;
        sRenderer = GetComponent<SpriteRenderer>();
        Invoke("BeginLerp", 1.2f);
    }

    void BeginLerp()
    {
        sRenderer.color = Color.white;
    }

    void OnBecameInvisible()
    {
        if(!isOutOfScreen)
        {
            isOutOfScreen = true;
            GetComponentInParent<Rocket>().DestroyRocket();
        }
     
    }


}
