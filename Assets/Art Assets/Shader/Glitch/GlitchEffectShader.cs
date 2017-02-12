using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/GlitchEffect")]
public class GlitchEffectShader : ImageEffectBase
{

    public Texture2D displacementMap;

    //Shader parameters transfered to Shader.

    [Range(0.1f, 10f)]
    public float intensity;
    [Range(1f, 10f)]
    public float filterRadius;
    public bool flipDown = false, flipUp = false;
    public bool isGlitching = false;

    void Awake()
    {
        isGlitching = false;
    }

    public IEnumerator OnGlitch(float length)
    {
        isGlitching = true;

        yield return new WaitForSeconds(length);

        isGlitching = false;
        yield break;
    }



    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        float _intensity = isGlitching ? intensity : 0;
        float _filterRadius = isGlitching ? Random.Range(-filterRadius, filterRadius) : 0;
        

        material.SetFloat("_Intensity", _intensity);
        material.SetFloat("_Chroma", Random.Range(0,2f));
        material.SetTexture("_DispTex", displacementMap);
        material.SetFloat("filterRadius", _filterRadius);



        //Tweak displacement map displacement probability
        if (Random.value < 0.1f)
        {
            material.SetFloat("displace", 0);
            material.SetFloat("scale", isGlitching ? 1 - Random.Range(0, _intensity) : 0);
        }
        else
            material.SetFloat("displace", 0);



        Graphics.Blit(source, destination, material);
    }
}

