using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Reset_Quit();
    }

    private void Reset_Quit()
    {
        if(Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick1Button9))
            SceneManager.LoadScene(1);

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button8))
            Application.Quit();
    }
}
