using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateGameObject : MonoBehaviour
{
    public float timer = 2f;

    void Start()
    {
        
        Invoke("DeactivateAfterTimer", timer);
    }

    void DeactivateAfterTimer()
    {
        gameObject.SetActive(false);
        Destroy(gameObject, timer);
    }
}
