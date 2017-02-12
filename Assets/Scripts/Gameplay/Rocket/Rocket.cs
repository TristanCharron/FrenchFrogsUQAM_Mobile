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

    public GameObject Launch, Jet;

    public const float Speed = 1600;



    // Use this for initialization
    void Awake()
    {
        canMove = false;
        isActive = true;

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
        else
        {
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Jet.transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Jet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
    }

    public void SetActiveRocket()
    {
        canMove = true;
    }

    public void UpdateRocket()
    {

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
            GetComponentInChildren<AnimationManager>().DisableJetAfterAnimation();



        isActive = false;



    }

    public void DestroyRocket()
    {
        RocketManager.DestroyRocket();

        if (Jet.transform.position.y > RocketManager.UpperBound_Check.position.y)
            TransferRocketToServer();


        Jet.transform.GetChild(1).gameObject.AddComponent<DelayDeath>().delay = 1;
        Jet.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop();
        Jet.transform.GetChild(1).SetParent(transform.root, true);
        
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
