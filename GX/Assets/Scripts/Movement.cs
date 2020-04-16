using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Collision coll;
    private Gravity grav;
    private InputBuffer listener;
    private SpriteRenderer sprite;

    [HideInInspector]
    public Rigidbody2D rb;

    [Space]
    [Header("Stats")]
    public float walkSpeed = 10;
    public float dashSpeed = 30;
    public float dashDur = 0.01f;
    //public float dashCoolDown = 4f;
    public float flySpeed = 15;
    public float dashFallGravityScale = 25;

    [Space]
    [Header("Booleans")]
    public bool canWalk;
    public bool isDashing = false;
    //public bool canDash;

    // there is a frame delay between isdashing turns false while still on wall
    // when the player perfectly dashinto wall while colliding with it at the
    // exact last frame of the dash
    // canDashGravChange repairs the visual delay to amend for player perceiving
    // that their dash grav change wasn't going through
    public bool canDashGravChange = true;

    public bool canFly;  // a toggle in editor for now, player form switch logic is not implemented

    [Space]
    [Header("Input")]
    public string horrizontalCtrl = "Horizontal_P1";
    public string verticalCtrl =  "Vertical_P1";
    public string dashButton = "Dash_P1";
    private float lastTimeStamp;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collision>();
        grav = GetComponent<Gravity>();
        listener = GetComponent<InputBuffer>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        //lastTimeStamp = Time.time;
        //canDash = true;
    }

    private void Update() {
        var x = Input.GetAxis(horrizontalCtrl);
        var xRaw = Input.GetAxisRaw(horrizontalCtrl);

        var y = Input.GetAxis(verticalCtrl);
        var yRaw = Input.GetAxisRaw(verticalCtrl);

       if (canFly)
       {
           grav.DisableGravity();
           Fly(x, y);
           return;
       }
       else
       {
           grav.EnableGravity();
       }

       if(coll.onGround && !isDashing)
        {
            Walk(x, y);
        }



        //canDash =  Time.time - lastTimeStamp > dashCoolDown;

        //if(Input.GetButtonDown(dashButton) && !isDashing && canDash)
        if (Input.GetButtonDown(dashButton) && !isDashing && coll.onGround)
        {
            if (Mathf.Abs(x) >= 0.1 || Mathf.Abs(y) >= 0.1)
            {
                Normalize(x, y, out float xNor, out float yNor);
                //lastTimeStamp = Time.time;
                //Debug.Log("x is " + xNor + "y is" + yNor);
                //listener.Listen();
                Dash(xNor, yNor);

            }
        }

        // if (isDashing && coll.onWall)
        // {
        //     rb.velocity = Vector2.zero;
        //     //canDash = false;
        //     //lastTimeStamp = Time.time;
        //     canWalk = false;

        //     //added enumerator terminator---
        //     StopCoroutine(DashWait());

        //     //grav.gravityScale = 3;
        //     isDashing = false;
        //     canDashGravChange = true;
        //     canWalk = true;
        //     //lastTimeStamp = 0;
        //     grav.enableGravity();

        //     // string nextKey = listener.Request();

        //     // if (nextKey == dashButton)
        //     // {
        //     //     Debug.Log("Followup");
        //     //     if (Mathf.Abs(x) >= 0.1 || Mathf.Abs(y) >= 0.1)
        //     //     {
        //     //         Normalize(x, y, out float xNor, out float yNor);
        //     //         lastTimeStamp = Time.time;
        //     //         listener.Listen();
        //     //         Dash(xNor, yNor);

        //     //     }
        //     // }
        //     // else
        //     // {
        //     //     Debug.Log("No Followup: " + nextKey);
        //     // }
        //     //------------------------------
        // }
        // else
        // {
        //     canWalk = true;
        //     canDashGravChange = false;
        // }

    }

    private void Walk(float x, float y)
    {
        var dir = Vector2.zero;

        if (grav.m_GravityDirection == Gravity.GravityDirection.Down || grav.m_GravityDirection == Gravity.GravityDirection.Up )
        {
            if (x == 1)
            {
                dir = new Vector2(1, 0);
                sprite.flipX = grav.m_GravityDirection == Gravity.GravityDirection.Up;
            }
            else if (x == -1)
            {
                dir = new Vector2(-1, 0);
                sprite.flipX = grav.m_GravityDirection == Gravity.GravityDirection.Down;
            }
        }
        else
        {
            if (y == 1)
            {
                dir = new Vector2(0, 1);
                sprite.flipX = grav.m_GravityDirection == Gravity.GravityDirection.Left;
            }
            else if (y == -1)
            {
                dir = new Vector2(0, -1);
                sprite.flipX = grav.m_GravityDirection == Gravity.GravityDirection.Right;
            }
        }
        rb.velocity = dir * walkSpeed;
    }

    private void Dash(float x, float y)
    {
        isDashing = true;
        var dir = Vector2.zero;
        dir = new Vector2(x,y);
        rb.velocity = dir.normalized * dashSpeed;  // physics dashing, not in 1 frame
        //rb.AddForce(dir.normalized * dashSpeed, ForceMode2D.Impulse);
        StartCoroutine(DashWait());
    }

    private IEnumerator DashWait()
    {
        grav.gravityScale = 0;
        yield return new WaitForSeconds(dashDur);

        //discards listener data if the dash went full duration
        //listener.Discard();

        grav.gravityScale = dashFallGravityScale;
        isDashing = false;
    }

    private void Fly(float x, float y)
    {
        if (!canFly)
        {
            return;
        }
        else
        {

            var dir = Vector2.zero;

            dir = new Vector2(x, y);

            rb.velocity = dir * flySpeed;

            Debug.Log("flying");
        }
    }

    private void Normalize(float x, float y, out float xNor, out float yNor)
    {
        float angle = Mathf.Rad2Deg * Mathf.Atan2(y, x);

        if (angle >= 165 || angle < -165)
        {
            xNor = -1;
            yNor = 0;
        }
        else if (angle < 165 && angle >= 105)
        {
            xNor = -1;
            yNor = 1;
        }
        else if (angle < 105 && angle >= 75)
        {
            xNor = 0;
            yNor = 1;
        }
        else if (angle < 75 && angle >= 15)
        {
            xNor = 1;
            yNor = 1;
        }
        else if (angle < 15 && angle >= -15)
        {
            xNor = 1;
            yNor = 0;
        }
        else if (angle < -15 && angle >= -75)
        {
            xNor = 1;
            yNor = -1;
        }
        else if (angle < -75 && angle >= -105)
        {
            xNor = 0;
            yNor = -1;
        }
        else if (angle < -105 && angle >= -165)
        {
            xNor = -1;
            yNor = -1;
        }
        else
        {
            xNor = 0;
            yNor = 0;
        }
    }
}
