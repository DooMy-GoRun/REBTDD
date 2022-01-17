using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Image health_UI;


    void Awake()
    {
        health_UI = GameObject.FindWithTag(Tags.HEALTH_UI).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayHealth(float value)
    {
        value /= 6f;

        if(value < 0)
            value = 0;

        health_UI.fillAmount = value;
    }
}
