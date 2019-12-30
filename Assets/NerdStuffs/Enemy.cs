using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mobile
{
    public AudioClip BounceSound;
    public AudioClip DeathSound;

    public bool stationary = false;
    public bool flying = false;
    bool dead = false;
    int stun = 0;

    


    public GameObject WallTest;
    public GameObject GroundTest;

    LayerMask mask;

    private void Awake()
    {
        mask = LayerMask.GetMask("Solid", "Semisolid");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dead)
            return;
        var fat = collision.GetComponent<FatScript>();
        if(fat.Fat == 4)
        {
            Kill();
            return;
        }
        var deltay = collision.transform.position.y - transform.position.y;
        var player = collision.GetComponent<PlatformingCharacter>();
        bool onTop = deltay > .1f && player.VMomentum < 0f;
        if(onTop)
        {
            ScreenFreeze.Freeze(6);
            if(fat.Fat < 3)
                player.VMomentum = player.jumpForce;
            if(fat.Fat == 3)
                player.VMomentum = player.jumpForce * .5f;
            if (fat.Fat > 1)
                Kill();
            else
                Flinch();
        }
        else
        {
            MySceneManager.Kill();
            // Debug.Log("Kill!");
        }
        
    }

    new private void FixedUpdate()
    {
        if (dead)
            return;
        stun--;
        if(flying)
        {
            Gravity = 0f;
            GetComponent<Animator>().SetBool("Fly", true);
            return;
        }
        if(stun > 0 || stationary)
        {
            HMomentum = 0f;
        }
        else if(HMomentum == 0f)
        {
            HMomentum = 1.5f * Forward;
        }

        GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(HMomentum));

        if (MustTurn())
            HMomentum *= -1f;

        base.FixedUpdate();

        
    }

    static RaycastHit2D[] dummy = new RaycastHit2D[1];

    bool MustTurn()
    {
        var hits = Physics2D.BoxCastNonAlloc(WallTest.transform.position, new Vector2(.1f, .1f), 0f, Vector2.zero, dummy, 0f, mask);
        if (hits > 0)            
            return true;
        hits = Physics2D.BoxCastNonAlloc(GroundTest.transform.position, new Vector2(.1f, .1f), 0f, Vector2.zero, dummy, 0f, mask);
        return hits == 0;
    }

    public void Kill()
    {
        dead = true;
        GetComponent<Animator>().SetTrigger("Kill");
        Destroy(gameObject, .5f);
        AudioPool.PlaySound(transform.position, DeathSound);
    }

    public void Flinch()
    {
        stun = 30;
        GetComponent<Animator>().SetTrigger("Flinch");
        AudioPool.PlaySound(transform.position, BounceSound);
    }
}
