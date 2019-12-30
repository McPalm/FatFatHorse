using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform Obstracle;
    public AudioClip OpenSound;

    public int Pressure { get; set; }
    public bool Open { get; set; }

    int openedness;

    public void FixedUpdate()
    {
        if(Open)
        {
            openedness += 10;
            if(openedness > 100)
            {
                openedness = 100;
            }
        }
        else
        {
            openedness = Pressure * 2;
            if (Pressure >= 28)
            {
                Open = true;
                AudioPool.PlaySound(transform.position, OpenSound);
            }
        }
        Obstracle.localPosition = Vector2.up * openedness * 0.0135f;
    }
}
