using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
