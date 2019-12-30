using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingCharacter : Mobile, IControllable
{
    public InputToken InputToken { get; set; }

    public float airControl = .2f;
    public float groundControl = .5f;
    public float speed = 5f;
    public float jumpForce = 9f;
    public int airJumps = 0;

    public bool Disable { get; set; } = false;

    public event System.Action OnJump;
    

    int airJump = 0;

    int cyoteTime = 5;

    new void Start()
    {
        base.Start();
        var spawn = FindObjectOfType<Spawn>();
        if (spawn)
            transform.position = spawn.transform.position;
    }

    // Update is called once per frame
    new void FixedUpdate()
    {
        if(Disable) // dirty hack, dont do it like this
        {
            HMomentum = 0f;
            base.FixedUpdate();
            return;
        }

        cyoteTime--;
        if (Grounded)
        {
            if(Mathf.Abs(HMomentum) < 1f && InputToken.AbsHor == 0f)
            {
                HMomentum = 0f;
            }
            else
            {
                HMomentum = HMomentum * (1f - groundControl) + InputToken.Horizontal * speed * groundControl;
            }
            airJump = airJumps;
            UpdateFacing();
            cyoteTime = 5;
        }
        else
            HMomentum = HMomentum * (1f - airControl) + InputToken.Horizontal * speed * airControl;
        if (cyoteTime >= 0 && InputToken.Jump)
        {
            VMomentum = jumpForce;
            InputToken.ClearJump();
            OnJump?.Invoke();
        }
        else if(airJump > 0 && InputToken.Jump && (VMomentum <= 0f || VMomentum > 1f))
        {
            // the snippet works together with the input buffer to help you nail the air jump at the top of your arc
            VMomentum = jumpForce * .8f;
            InputToken.ClearJump();
            OnJump?.Invoke();
            HMomentum = InputToken.Horizontal * speed;
            airJump--;
            UpdateFacing();
        }
        base.FixedUpdate();

        Gravity = InputToken.HoldJump && VMomentum > 0f ? 16f : 30f;
        if (Grounded == false && VMomentum > 0f & !InputToken.HoldJump)
        {
            VMomentum *= .8f;
        }
    }

    void UpdateFacing()
    {
        if(speed > 0f && InputToken.AbsHor > 0f)
        {
            FaceRight = InputToken.Horizontal > 0f;
        }
    }
}
