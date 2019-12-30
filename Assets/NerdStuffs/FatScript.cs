using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatScript : MonoBehaviour
{
    int fat = 1;
    public int Fat => fat;

    public event System.Action OnEat;
    public event System.Action OnTrain;

    public void IncreaseFat()
    {
        if(fat < 5)
            fat++;
        OnEat?.Invoke();
        SetFatProperties(fat);
    }

    public void DecreaseFat()
    {
        if(fat > 1)
        fat--;
        OnTrain?.Invoke();
        SetFatProperties(fat);
    }

    void SetFatProperties(int fat)
    {
        var lemon = GetComponent<PlatformingCharacter>();
        var animated = GetComponent<Animator>();
        switch(fat)
        {
            case 1:
                lemon.speed = 8;
                lemon.jumpForce = 11;
                lemon.airControl = 0.1f;
                lemon.groundControl = 0.3f;
                animated.SetFloat("Fat", 0f);
                break;
            case 2:
                lemon.speed = 6;
                lemon.jumpForce = 9.5f;
                lemon.airControl = 0.015f;
                lemon.groundControl = 0.15f;
                animated.SetFloat("Fat", .33f);
                break;
            case 3:
                lemon.speed = 3.5f;
                lemon.jumpForce = 7f;
                lemon.airControl = 0.01f;
                lemon.groundControl = 0.1f;
                animated.SetFloat("Fat", .66f);
                break;
            case 4:
                lemon.speed = 2;
                lemon.jumpForce = 4;
                lemon.airControl = 0.01f;
                lemon.groundControl = 0.1f;
                animated.SetFloat("Fat", 1f);
                break;
            case 5:
                lemon.speed = 0;
                lemon.jumpForce = 0;
                animated.SetFloat("Fat", 1.33f);
                break;
        }
    }

}
