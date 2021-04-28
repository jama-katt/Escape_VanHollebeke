using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScript : MonoBehaviour
{
    bool won = false;
    public TMP_Text wintext;
    public KeyCode returnKey;
    void Win()
    {
        Time.timeScale = 0;
        wintext.text = "YOU WON!\nPress H to return to menu";
        won = true;
    }

    private void Update()
    {
        if (won && Input.GetKeyDown(returnKey))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
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
