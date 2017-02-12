using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{

    static List<Rocket> RocketList;
    static Rocket currentRocket { get { return RocketList[RocketList.Count - 1]; } }

    private static RocketManager instance;
    public static RocketManager Instance { get { return instance; } }

    public GameObject RocketPrefab;

    public delegate void RocketEvent();
    public static event RocketEvent OnInstantiateRocket, OnDestroyRocket;

    void Awake()
    {
        instance = this;
        GameplayManager.OnBeginGame += OnBegin;
        MouseManager.OnLeftMouseClick += InstantiateRocket;
        MouseManager.OnHoldMouse += UpdateRocket;
        MouseManager.OnReleaseMouse += ReleaseRocket;
    }

    void OnBegin()
    {
        RocketList = new List<Rocket>();
        Debug.Log("Rocket Generated");
    }

    public static void InstantiateRocket()
    {
        GameObject RocketPrefab = Instantiate(instance.RocketPrefab, instance.transform.root, false) as GameObject;
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
