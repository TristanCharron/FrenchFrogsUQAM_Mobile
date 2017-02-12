using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{

    static List<Rocket> RocketList;
    public static Rocket currentRocket { get { return RocketList[RocketList.Count - 1]; } }

    private static RocketManager instance;
    public static RocketManager Instance { get { return instance; } }
    
    public GameObject[] RocketFire;
    public Sprite[] RocketSprite; 
    public GameObject RocketPrefab,Upperbound;

    public static Transform UpperBound_Check;

    public delegate void RocketEvent();
    public static event RocketEvent OnInstantiateRocket, OnDestroyRocket;




    void Awake()
    {
        instance = this;
        UpperBound_Check = Upperbound.transform;
        GameplayManager.OnBeginGame += OnBegin;
        MouseManager.OnLeftMouseClick += InstantiateRocket;
        MouseManager.OnHoldMouse += UpdateRocket;
        MouseManager.OnReleaseMouse += ReleaseRocket;
    }

    void OnBegin()
    {
        RocketList = new List<Rocket>();
    }

    public static void InstantiateRocket()
    {
        int random = Random.Range(0, instance.RocketSprite.Length);

        GameObject RocketPrefab = Instantiate(instance.RocketPrefab, instance.transform.root, false) as GameObject;
        RocketPrefab.GetComponent<Rocket>().Jet.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = instance.RocketSprite[random];
        GameObject RocketFire = Instantiate(instance.RocketFire[random], RocketPrefab.transform.position, Quaternion.identity) as GameObject;
        RocketFire.transform.SetParent(RocketPrefab.transform.GetChild(0), true);
        RocketFire.transform.localPosition = new Vector3(-2, 0, 0);
        RocketFire.transform.localEulerAngles = new Vector3(0, 270, 270);
        RocketList.Add(RocketPrefab.GetComponent<Rocket>());

        if (OnInstantiateRocket != null)
            OnInstantiateRocket();
    }

    public static void DestroyRocket()
    {
        if (OnDestroyRocket != null)
            OnDestroyRocket();

     

    }

    static void UpdateRocket()
    {
        if (currentRocket != null)
        {
            currentRocket.UpdateRocket();
        }

    }

    static void ReleaseRocket()
    {
        if (currentRocket != null && currentRocket.IsActive)
        {
            currentRocket.ReleaseRocket();
        }

    }


}
