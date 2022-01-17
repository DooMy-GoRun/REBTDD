using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private float power = 0.2f;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float slowDownAmount = 1f;

    private bool should_Shake;
    private float initialDuration;

    private Vector3 startPosition;

    public bool ShouldShake
    {
        get {
            return should_Shake;
        }
        set {
            should_Shake = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        initialDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        Shake();
    }

    void Shake()
    {
        //if we should shake the camera
        if (should_Shake)
        {
            if(duration > 0f)
            {
                transform.localPosition = startPosition + Random.insideUnitSphere * power;
                duration -=  Time.deltaTime * slowDownAmount;
            }
            else
            {
                should_Shake = false;
                duration = initialDuration;
                transform.localPosition = startPosition;
            }
        }
    }
}
