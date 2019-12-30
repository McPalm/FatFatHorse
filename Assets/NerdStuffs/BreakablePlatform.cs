using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public Collider2D platform;
    public AudioClip BreakSound;

    FatScript fatty;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(fatty && fatty.Fat >= 4)
        {
            AudioPool.PlaySound(transform.position, BreakSound);
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fatty)
        {
            platform.enabled = fatty.Fat < 4;
        }
        else
        {
            fatty = FindObjectOfType<FatScript>();
        }
    }
}
