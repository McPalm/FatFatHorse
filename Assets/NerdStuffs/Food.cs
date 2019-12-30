using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    public AudioClip GetSound;

    public bool dietFood = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // FindObjectOfType<Win>().NeedCoin--;
        AudioPool.PlaySound(transform.position, GetSound);
        var fatty = collision.transform.GetComponent<FatScript>();
        if (dietFood)
            fatty.DecreaseFat();
        else
            fatty.IncreaseFat();
        var platty = collision.transform.GetComponent<Mobile>();
        platty.HMomentum *= .75f;
        if (platty.VMomentum > 0f)
            platty.VMomentum *= .5f;
        ScreenFreeze.Freeze(5);
        Destroy(gameObject);
    }
}
