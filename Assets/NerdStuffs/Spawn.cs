using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    bool respawning = false;

    public AudioClip Die;

    IEnumerator Start()
    {
        while(true)
        {
            var player = FindObjectOfType<PlatformingCharacter>();
            if(player != null)
            {
                player.transform.position = transform.position;
                break;
            }
            yield return null;
        }
    }
}
