using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    bool canMove = false;
    bool isActive = false;
    bool isLaunched = false;

    [SerializeField]
    private Color[] ColorList;

    int currentSprite = 0;
    public int CurrentSprite { get { return currentSprite; } }

    public bool IsActive { get { return isActive; } }

    public GameObject Launch, Jet, VFX_Explosion;
    Vector2 finalVelocity;

    public const float Speed = 1600;
    float t = 0;



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
        if (!isLaunched)
        {
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Jet.transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Jet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }


    }



    public void SetCurrentSpriteIndex(int index)
    {
        currentSprite = index;
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
            Vector2 direction = heading / heading.magnitude;

            if (direction == Vector2.zero)
                direction = new Vector2(0,1);

            finalVelocity = direction.normalized * Speed;


            VFX_Explosion.SetActive(true);
            VFX_Explosion.transform.SetParent(transform.root);
            VFX_Explosion.GetComponent<ParticleSystem>().startColor = ColorList[CurrentSprite];
            VFX_Explosion.AddComponent<DelayDeath>().delay = 5;




            isLaunched = true;
            GameplayManager.OnUpdateDuringGame += EaseVelocityTo;
        }

        if (!canMove)
            GetComponentInChildren<AnimationManager>().DisableJetAfterAnimation();

        isActive = false;



    }

    public void EaseVelocityTo()
    {
        t += Time.deltaTime;

        if (t >= 1 || Jet == null)
        {
            GameplayManager.OnUpdateDuringGame -= EaseVelocityTo;
        }
        else
        {
            Rigidbody2D rigidbody = Jet.GetComponent<Rigidbody2D>();
            rigidbody.AddForce(Vector2.Lerp(finalVelocity / 10, finalVelocity, t));

        }

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
