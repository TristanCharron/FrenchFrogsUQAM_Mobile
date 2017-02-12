using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    bool canMove = false;
    bool isActive = false;
    bool isLaunched = false;

    int currentSprite = 0;
    public int CurrentSprite { get { return currentSprite; } }

    public bool IsActive { get { return isActive; } }

    private Vector3 ScaleOrigin = Vector3.zero;
    private Vector3 ScaleEnd = Vector3.one;

    public GameObject Launch, Jet;

    float t = 0;
    const float scaleLength = 1.2f;
    public const float Speed = 1600;



    // Use this for initialization
    void Awake()
    {
        canMove = false;
        isActive = true;
        t = 0;

        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        transform.position = Camera.main.ScreenToWorldPoint(cursorPoint);
        transform.localRotation = Quaternion.Euler(0, 0, 0);

    }

  

    public void Update()
    {
        if (isLaunched)
        {
            RotateToVelocity();
        }
    }

    public void UpdateRocket()
    {
        if (!canMove)
        {
            t += Time.deltaTime;
            if (t >= scaleLength)
                canMove = true;
            Jet.transform.LookAt(MouseManager.MouseToWorldCoordinate - Jet.transform.position);
        }
        else
        {
            ReleaseRocket();
        }


    }

    public void ReleaseRocket()
    {
        if (!isLaunched)
        {
            Vector3 heading = MouseManager.MouseToWorldCoordinate - Jet.transform.position;

            if (heading.magnitude > 0)
            {
                Vector2 direction = heading / heading.magnitude;
                Jet.GetComponent<Rigidbody2D>().AddForce(direction.normalized * Speed);
                isLaunched = true;
              
            }

        }
        if (!canMove)
        {
            Debug.Log("FadeOut");
            Invoke("DestroyRocket", 1);

        }

    
        isActive = false;



    }

    public void DestroyRocket()
    {
        RocketManager.DestroyRocket();

        if (Jet.transform.position.y > RocketManager.UpperBound_Check.position.y)
            TransferRocketToServer();

        Destroy(gameObject);
    }

    void TransferRocketToServer()
    {
        NetworkObjectManager[] NetworkList = FindObjectsOfType(typeof(NetworkObjectManager)) as NetworkObjectManager[];
        if (NetworkList.Length > 0)
        {
            NetworkList[0].CmdTransferRocket(Jet.transform.position.x,
          Jet.transform.eulerAngles.z - 90,
          800,
          CurrentSprite
          );
        }
    }

    public void RotateToVelocity()
    {
        Vector2 dir = Jet.GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Jet.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


}
