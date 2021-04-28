using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NextLevelScript : MonoBehaviour
{
    bool won = false;
    public TMP_Text wintext;
    public KeyCode returnKey;
    void Win()
    {
        Time.timeScale = 0;
        wintext.text = "LEVEL 1 CLEARED!\nPress H to proceed";
        won = true;
    }

    private void Update()
    {
        if (won && Input.GetKeyDown(returnKey))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Win();
        }
    }
}
