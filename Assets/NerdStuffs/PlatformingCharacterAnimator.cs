using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingCharacterAnimator : MonoBehaviour, IControllable
{

    PlatformingCharacter platformingCharacter;
    Animator animator;

    public InputToken InputToken { get; set; }

    bool dying = false;

    private void Start()
    {
        platformingCharacter = GetComponent<PlatformingCharacter>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dying)
            return;
        animator.SetBool("Grounded", platformingCharacter.Grounded);
        animator.SetFloat("Speed", Mathf.Abs(platformingCharacter.HMomentum));
        // animator.SetBool("Duck", InputToken.Vertical < -.5f && InputToken.AbsHor == 0f && sugaMoni.Grounded);
    }

    public void PlayDeath() => StartCoroutine(DeathAnimation());

    IEnumerator DeathAnimation()
    {
        dying = true;
        GetComponent<PlatformingCharacterSounds>().PlayDeath();
        animator.SetBool("Grounded", false);
        animator.SetFloat("Speed", 0f);
        transform.Rotate(Vector3.forward, 90f);
        yield return new WaitForSeconds(.15f);
        transform.Rotate(Vector3.forward, 90f);
        yield return new WaitForSeconds(.15f);
        transform.Rotate(Vector3.forward, 90f);
        yield return new WaitForSeconds(.15f);
        transform.Rotate(Vector3.forward, 90f);
        yield return new WaitForSeconds(.15f);
        transform.Rotate(Vector3.forward, 90f);
        yield return new WaitForSeconds(.15f);
        transform.Rotate(Vector3.forward, 90f);

    }
}
