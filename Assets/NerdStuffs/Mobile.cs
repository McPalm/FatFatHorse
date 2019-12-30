using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Make something a physics ish object that interact with the enviroment.
/// </summary>
public class Mobile : MonoBehaviour
{
    public bool floats;
    static RaycastHit2D hit;
    private Transform LastHit;
    private Vector3 LastHitPosition;
    public Vector2 GroundMomentum { get; private set; }
    public LayerMask Solid { get; private set; }
    public LayerMask SemiSolids { get; private set; }
    public LayerMask Stairs { get; private set; }
    public LayerMask PhysicsTriggers { get; private set;}
    public float GroundLevel { get; internal set; }
    public LayerMask Ground
    {
        get
        {
            var v = Solid;
            if (AllowSemiSolids)
                v += SemiSolids;
            if (AllowStairs)
                v += Stairs;
            return v;
        }
    }
    LayerMask SolidGround => AllowStairs ? (LayerMask)(Solid + Stairs) : Solid;

    public float radius;
    public bool TurnInAir = false;

    public bool Grounded { private set; get; }
    public void ForceGrounded(bool grounded) => Grounded = grounded;
    public bool TouchingWall { private set; get; }
    public float VMomentum { get; set; }
    public float HMomentum { get; set; }
    public float Speed { get => Mathf.Abs(HMomentum); set => HMomentum = Speed * Forward; }
    public bool LockFacing = false;
    public bool Suspend { get; set; } = false;
    public float SlopeAngle { get; private set; }
    public float RelativeSlopeAngle => -Forward * SlopeAngle;

    protected float Gravity = 18f;

    virtual public bool Flinching { get; set; }

    [Tooltip("set negative to have em bounce!")]
    public float wallStop = .8f;

    public bool AllowStairs { get; set; } = false;
    public bool AllowSemiSolids { get; set; } = true;

    public event System.Action OnLand;

    protected void Start()
    {
        if(floats)
            Solid = LayerMask.GetMask("Solid", "Water");
        else
            Solid = LayerMask.GetMask("Solid");
        SemiSolids = LayerMask.GetMask("Semisolid");
        Stairs = LayerMask.GetMask("Stairs");
        PhysicsTriggers = LayerMask.GetMask("PhysicsTriggers");
    }

    public bool FaceRight
    {
        get
        {
            return transform.localEulerAngles.y == 180f;
        }
        set
        {
            transform.localEulerAngles = new Vector3(0f, value ? 180f : 0f);
        }
    }

    public int Forward => FaceRight ? 1 : -1;

    protected void FixedUpdate()
    {
        if (Suspend)
            return;

        // Horizontal
        var hpos = transform.position.x + HMomentum * Time.fixedDeltaTime;


        

        // Vertical
        VMomentum -= Gravity * Time.fixedDeltaTime;
        var vpos = transform.position.y + VMomentum * Time.fixedDeltaTime;

        if (VMomentum > 0f)
        {
            hit = Physics2D.BoxCast(transform.position, Vector2.one * radius * .9f, 0f, Vector2.up, 10f, SolidGround);
            GroundLevel = hit.point.y;


            var ceiling = hit ? hit.point.y : vpos + 10f;
            if (ceiling < vpos + radius)
            {
                vpos = transform.position.y;
                VMomentum *= .5f;
            }
            Grounded = false;
            LastHit = null;
        }
        else
        {
            hit = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - radius + .08f),
                new Vector2(radius * .9f, .02f),
                0f, Vector2.down,
                10f, Ground);
            GroundLevel = hit ? hit.point.y : vpos - 10f;
            if (GroundLevel > vpos - radius - (Grounded ? .1f : 0f) && VMomentum < 0f)
            {
                vpos = GroundLevel + radius;
                if (!Grounded && VMomentum < 0f)
                    OnLand?.Invoke();
                VMomentum = 0f;
                Grounded = true;
                Vector2 motion = hit.transform.position - LastHitPosition;
                if (LastHit == hit.transform && motion != Vector2.zero)
                {
                    hpos += motion.x;
                    vpos += motion.y;
                }
                LastHit = hit.transform;
                LastHitPosition = LastHit.transform.position;
                float rightLevel = GroundLevel;
                float leftLevel = GroundLevel;
                var x = hit.point.x;
                hit = Physics2D.Raycast(new Vector2(hit.point.x, GroundLevel) + new Vector2(.0333333f, .1f), Vector2.down, .5f, Ground);
                if (hit)
                {
                    rightLevel = hit.point.y;
                    hit = Physics2D.Raycast(new Vector2(hit.point.x, GroundLevel) + new Vector2(-.0333333f, .1f), Vector2.down, .5f, Ground);
                    if (hit)
                    {
                        leftLevel = hit.point.y;
                        SlopeAngle = (leftLevel - rightLevel) * 30f;
                    }
                    else
                        SlopeAngle = 0f;
                }
                else
                    SlopeAngle = 0f;
            }
            else
            {
                Grounded = false;
                LastHit = null;
            }
        }
        var delta = new Vector2(hpos, vpos) - (Vector2)transform.position;

        // facing, has to be done after determining grounded status
        if (LockFacing == false && HMomentum != 0f && (Grounded || TurnInAir))
        {
            if (HMomentum > 0f)
                FaceRight = true;
            else if (HMomentum < 0f)
                FaceRight = false;
        }

        hit = Physics2D.BoxCast(transform.position, Vector2.one * radius, 0f, delta, delta.magnitude, Solid);
        if (hit)
        {
            TouchingWall = HMomentum != 0f && Mathf.Sign(HMomentum) == Mathf.Sign(hit.point.x - transform.position.x);
            if (hit.fraction > 0f)
                delta *= hit.fraction;
            else
            {
                if (delta.x != 0f)
                {
                    if (delta.x < 0f && hit.point.x < transform.position.x)
                    {
                        delta = new Vector2(0f, delta.y);
                        HMomentum *= wallStop;

                    }
                    else if (delta.x > 0f && hit.point.x > transform.position.x)
                    {
                        delta = new Vector2(0f, delta.y);
                        HMomentum *= wallStop;
                    }
                }
                else
                {

                }
            }

        }
        else
            TouchingWall = false;

        transform.position += (Vector3)delta;
    }

    public bool OnEdge
    {
        get
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x + Forward * .1f, transform.position.y - radius),
                Vector2.down, .2f, SemiSolids ^ Solid);
            return !hit;
        }
    }
}
