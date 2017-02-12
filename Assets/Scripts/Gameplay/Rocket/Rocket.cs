using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    bool canMove = false;
    bool isActive = false;
    bool isLaunched = false;

    public bool IsActive { get { return isActive; } }

    private Vector3 ScaleOrigin = Vector3.zero;
    private Vector3 ScaleEnd = Vector3.one;

    public GameObject Launch, Jet;

    float t = 0;
    const float scaleLength = 1;


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
        }


    }

    public void ReleaseRocket()
    {
        if (!isLaunched)
        {
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
            Vector3 heading = cursorPosition - Jet.transform.position;

            if (heading.magnitude > 0)
            {
                Vector2 direction = heading / heading.magnitude;
                Jet.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 800);
                isLaunched = true;
                Destroy(gameObject, 3);
            }

        }
        if (!canMove)
        {
            Debug.Log("FadeOut");
            Destroy(gameObject, 1);

        }
        isActive = false;



    }


    public void RotateToVelocity()
    {
        Debug.Log("Rotate");
        Vector2 dir = Jet.GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Jet.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


}
