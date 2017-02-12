using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    public GameObject Jet, Owner;
    bool isAnimationCompleted = true;

    SpriteRenderer sRenderer;
    float t = 0;

    [SerializeField]
    Color colorIn,colorOut;

    void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
    }

    public void EnableJet()
    {
        if (isAnimationCompleted)
        {
            Jet.SetActive(true);
            Owner.GetComponent<Rocket>().SetActiveRocket();
        }
        else
            Destroy(Owner.gameObject);

    }

    public void DisableJetAfterAnimation()
    {

        isAnimationCompleted = false;
        GameplayManager.OnUpdateDuringGame += FadeOut;
    }

    public void FadeOut()
    {
        if(sRenderer != null)
        {
            t += Time.deltaTime;
            sRenderer.color = Color.Lerp(colorIn, colorOut, t);

            if (t >= 1)
            {
                GameplayManager.OnUpdateDuringGame -= FadeOut;
                Destroy(Owner.gameObject);
            }
        }
        else
        {
            GameplayManager.OnUpdateDuringGame -= FadeOut;
        }
     
    }
}
