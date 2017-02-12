using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{

    public delegate void MouseEvent();
    public static event MouseEvent OnLeftMouseClick, OnHoldMouse, OnReleaseMouse;

    static bool isMouseHeld;
    static float mouseHoldingLength;
    public const float mouseThreshold = 0.2f;

    public static Vector3 MouseToWorldCoordinate
    {
        get
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
            return cursorPosition;
        }
    }


    void Awake()
    {
        GameplayManager.OnBeginGame += OnBegin;
        GameplayManager.OnUpdateDuringGame += OnUpdate;
    }

    static void OnBegin()
    {
        Debug.Log("Mouse Manager enabled");
        isMouseHeld = false;
        mouseHoldingLength = 0;
    }

    static void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            PressMouseClick();



    }

    static void PressMouseClick()
    {
        if (OnLeftMouseClick != null)
            OnLeftMouseClick();

        GameplayManager.OnUpdateDuringGame += isHoldingMouse;
    }

    static void isHoldingMouse()
    {
        isMouseHeld = Input.GetMouseButton(0);

        if (isMouseHeld)
        {
            //Debug.Log("Holding Mouse");
            OnHoldMouse();
            mouseHoldingLength += Time.deltaTime;
        }
        else
        {
            //Debug.Log("is Not Holding Mouse");
            OnReleaseMouse();
            GameplayManager.OnUpdateDuringGame -= isHoldingMouse;
            mouseHoldingLength = 0;
        }
            

    }

}
