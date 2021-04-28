using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    //public GameObject credits;
    public GameObject instructions;

    //bool creds = false;
    bool instruct = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void Instructions()
    {
        if (instruct)
        {
            instruct = false;
            instructions.SetActive(false);
        }
        else
        {
            instruct = true;
            instructions.SetActive(true);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    /*void Credits()
    {
        if (creds)
        {
            credits.SetActive(false);
        }
        else
        {
            credits.SetActive(true);
        }
    }*/
}
