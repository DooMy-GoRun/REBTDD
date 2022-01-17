using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestView : MonoBehaviour
{

    private void Awake()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        ClickToBattle();
    }

    private void ClickToBattle()
    {
        if (Input.anyKeyDown)
            SceneManager.LoadScene(1);
    }
}
