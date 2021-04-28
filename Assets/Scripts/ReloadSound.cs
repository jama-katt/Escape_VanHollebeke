using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadSound : MonoBehaviour
{
    public AudioSource sound1;
    public AudioSource sound2;

    void Sound1()
    {
        sound1.Play();
    }

    void Sound2()
    {
        sound2.Play();
    }
}
