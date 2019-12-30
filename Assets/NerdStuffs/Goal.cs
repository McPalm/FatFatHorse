using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioClip GetSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // FindObjectOfType<Win>().NeedCoin--;
        AudioPool.PlaySound(transform.position, GetSound);
        MySceneManager.Instance.WinStage();
        // Destroy(gameObject);
    }
}
