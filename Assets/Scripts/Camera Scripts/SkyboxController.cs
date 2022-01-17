using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public float RotateSpeed = 30f;
    

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);
        RenderSettings.skybox.SetFloat("_Exposure", Mathf.PerlinNoise (1f, Time.time));
    }
}
