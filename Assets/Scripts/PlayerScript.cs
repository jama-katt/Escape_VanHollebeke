using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerScript : MonoBehaviour
{
    public float health = 100f;
    public RectTransform hpBar;
    public Vector3 hpBarPos;
    public float hpBarWidth;
    public float hpBarHeight;
    bool dead = false;

    public TMP_Text deathText;

    public float speed = 5f;

    public KeyCode restartKey;
    public KeyCode jumpKey;

    public KeyCode sprintKey;
    float sprint = 1f;
    public float sprintfactor = 1.5f;

    public Vector3 velocity;
    float gravity = -9.8f;

    public Transform feet;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool onGround = false;

    public CharacterController controller;

    private void Update()
    {
        if (dead)
        {
            if (Input.GetKeyDown(restartKey))
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        int timeNum = SlowTime.timeNumber;
        onGround = Physics.CheckSphere(feet.position, groundDistance, groundMask);

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (Input.GetKey(sprintKey))
        {
            sprint = sprintfactor;
        }
        else if (sprint == sprintfactor)
        {
            sprint = 1f;
        } 

        Vector3 direction = Vector3.Normalize(gameObject.transform.forward * inputY + gameObject.transform.right * inputX);
        controller.Move(direction * speed * sprint * Time.deltaTime);

        if (!onGround)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (Input.GetKeyDown(jumpKey))
        {
            velocity = new Vector3(0, 5, 0);
        }

        controller.Move(velocity * Time.deltaTime);
    }

    public void Damage(float num)
    {
        health -= num;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        AdjustHpBar();
    }

    private void Die()
    {
        Time.timeScale = 0;
        deathText.text = "YOU DIED!\nPress H to restart";
        dead = true;
    }

    private void AdjustHpBar()
    {
        hpBar.sizeDelta = new Vector2 (hpBarWidth * ((200 - health) / 200), hpBarHeight);
        hpBar.localPosition = new Vector3(hpBarPos.x + (0.5f * (hpBarWidth - hpBar.sizeDelta.x)) - 100, hpBarPos.y, hpBarPos.z);
    }

}
