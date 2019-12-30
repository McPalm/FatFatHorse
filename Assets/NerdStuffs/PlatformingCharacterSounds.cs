using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingCharacterSounds : MonoBehaviour
{
    public AudioClip Jump;
    public AudioClip Land;
    public AudioClip Death;
    public AudioClip Eat;
    public AudioClip Gym;

    FatScript fatScript;

    int Fat => fatScript.Fat;

    private void Start()
    {
        fatScript = GetComponent<FatScript>();
        GetComponent<PlatformingCharacter>().OnJump += PlatformingCharacterSounds_OnJump;
        GetComponent<PlatformingCharacter>().OnLand += PlatformingCharacterSounds_OnLand;
        fatScript.OnEat += FatScript_OnEat;
        fatScript.OnTrain += FatScript_OnTrain;
    }

    private void FatScript_OnTrain()
    {
        AudioPool.PlaySound(transform.position, Gym);
    }

    private void FatScript_OnEat()
    {
        switch(Fat)
        {
            case 2:
                AudioPool.PlaySound(transform.position, Eat, 1f, 1.8f);
                break;
            case 3:
                AudioPool.PlaySound(transform.position, Eat, 1f, 1.6f);
                break;
            case 4:
                AudioPool.PlaySound(transform.position, Eat, 1f, 1.45f);
                break;
            case 5:
                AudioPool.PlaySound(transform.position, Eat, 1f, 1.35f);
                break;
        }
    }

    public void PlayDeath()
    {
        AudioPool.PlaySound(transform.position, Death);
    }

    private void PlatformingCharacterSounds_OnLand()
    {
        if(Fat == 3)
        {
            AudioPool.PlaySound(transform.position, Land, .75f, 1f);
        }
        if(Fat == 4)
        {
            AudioPool.PlaySound(transform.position, Land, 1f, .85f);
        }
        if (Fat == 5)
        {
            AudioPool.PlaySound(transform.position, Land, 1f, .7f);
        }
    }

    private void PlatformingCharacterSounds_OnJump()
    {
        switch(Fat)
        {
            case 1:
                AudioPool.PlaySound(transform.position, Jump, 1f, 1.25f);
                break;
            case 2:
                AudioPool.PlaySound(transform.position, Jump, 1f, 1f);
                break;
            case 3:
                AudioPool.PlaySound(transform.position, Jump, 1f, .85f);
                break;
            case 4:
                AudioPool.PlaySound(transform.position, Jump, 1f, .5f);
                break;
        }
    }
}
