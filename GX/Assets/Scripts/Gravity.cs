using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public enum GravityDirection
    {
        Down,
        Right,
        Left,
        Up
    };

    [Header("References")]
    public Rigidbody2D playerRB;
    [Space]
    [Header("Gravity")]
    public GravityDirection m_GravityDirection; 
    //[HideInInspector]
    public float gravityScale;
    public float defaultGravityScale = 9.8f;

    [Space]
    [Header("Input")]
    public string horrizontalCtrl = "Horizontal_P1";
    public string verticalCtrl =  "Vertical_P1";
    public string gravSwitchButton =  "GravSwitch_P1";

    private Collision coll;
    private float xRaw;
    private float yRaw;

    private Vector2 m_bottomOffset = new Vector2(0f, -0.16f);
    private Vector2 m_rightOffset = new Vector2(0.25f, 0f);
    private Vector2 m_leftOffset = new Vector2(-0.25f, 0f);
    private Vector2 m_topOffset = new Vector2(0f, 0.32f);

    private bool hasGravity = true;

    private void Start() {
        Physics2D.gravity = Vector2.zero;
        gravityScale = defaultGravityScale;
        m_GravityDirection = GravityDirection.Down;
        coll = GetComponent<Collision>();

        Vector2 m_bottomOffset = coll.bottomOffset;
        Vector2 m_rightOffset = coll.rightOffset;
        Vector2 m_leftOffset = coll.leftOffset;
    }
    private void Update() {
        xRaw = Input.GetAxisRaw(horrizontalCtrl);
        yRaw = Input.GetAxisRaw(verticalCtrl);

        if (Input.GetAxis(gravSwitchButton) > 0)
        {
            //SwitchGravityDirection();  // TODO taking grav swtich out in first playable
        }
       
        UpdatePlayerOrientation();
        ApplyGravityForce();

        //Debug.Log(coll.wrapCorner);
        if(coll.wrapCorner)
        {
            StartCoroutine(ExampleCoroutine());
            //rb.velocity = Vector2.zero; // TODO
            m_GravityDirection = GravityDirection.Right;
            coll.wrapCorner = false;
        }
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1f);
    }

    /*private void SwitchGravityDirection()
    {
        if (xRaw == -1)
        {
            m_GravityDirection = GravityDirection.Left;
        }
        else if (xRaw == 1)
        {
            m_GravityDirection = GravityDirection.Right;
        }
        else if (yRaw == -1)
        {
            m_GravityDirection = GravityDirection.Down;
        }
        else if (yRaw == 1)
        {
            m_GravityDirection = GravityDirection.Up;
        }
            
    }*/

    public void OnDashGravityChange()
    {
        if (coll.onWall)
        {
            if(coll.wallSide == 1)
            {
                switch(m_GravityDirection)
                {
                    case GravityDirection.Down:
                        m_GravityDirection = GravityDirection.Right;
                        break;
                    case GravityDirection.Left:
                        m_GravityDirection = GravityDirection.Down;
                        break;
                    case GravityDirection.Up:
                        
                        m_GravityDirection = GravityDirection.Left;
                        break;
                    case GravityDirection.Right:
                        m_GravityDirection = GravityDirection.Up;
                        break;
                }
            }
            else if (coll.wallSide == -1)
            {
                switch(m_GravityDirection)
                {
                    case GravityDirection.Down:
                        m_GravityDirection = GravityDirection.Left;
                        break;
                    case GravityDirection.Left:
                        m_GravityDirection = GravityDirection.Up;
                        break;
                    case GravityDirection.Up:
                        
                        m_GravityDirection = GravityDirection.Right;
                        break;
                    case GravityDirection.Right:
                        m_GravityDirection = GravityDirection.Down;
                        break;
                }
            }
            else if (coll.wallSide == 2)
            {
                switch(m_GravityDirection)
                {
                    case GravityDirection.Down:
                        m_GravityDirection = GravityDirection.Up;
                        break;
                    case GravityDirection.Left:
                        m_GravityDirection = GravityDirection.Right;
                        break;
                    case GravityDirection.Up:
                        m_GravityDirection = GravityDirection.Down;
                        break;
                    case GravityDirection.Right:
                        m_GravityDirection = GravityDirection.Left;
                        break;
                }
            }
            
        }
    }

    private void UpdatePlayerOrientation()
    {
        switch(m_GravityDirection)
        {
            case Gravity.GravityDirection.Down:
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                coll.bottomOffset = m_bottomOffset;
                coll.collisionBoxX = 0.42f;
                coll.collisionBoxY = 0.04f;
                coll.rightOffset = m_rightOffset;
                coll.leftOffset = m_leftOffset;
                coll.topOffset = m_topOffset;
                break;
            case Gravity.GravityDirection.Left:
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                coll.bottomOffset = new Vector2(m_bottomOffset.y, m_bottomOffset.x) ;
                coll.collisionBoxX = 0.04f;
                coll.collisionBoxY = 0.42f;
                coll.rightOffset = new Vector2(m_rightOffset.y, - m_rightOffset.x) ;
                coll.leftOffset = new Vector2(m_leftOffset.y, - m_leftOffset.x) ;
                coll.topOffset = new Vector2(m_topOffset.y, m_topOffset.x);
                break;
            case Gravity.GravityDirection.Up:
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                coll.bottomOffset = new Vector2(m_bottomOffset.x, - m_bottomOffset.y) ;
                coll.collisionBoxX = 0.42f;
                coll.collisionBoxY = 0.04f;
                coll.rightOffset = new Vector2(- m_rightOffset.x, m_rightOffset.y) ;
                coll.leftOffset = new Vector2(-m_leftOffset.x, m_leftOffset.y) ;
                coll.topOffset = new Vector2(-m_topOffset.x, - m_topOffset.y);
                break;
            case Gravity.GravityDirection.Right:
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                coll.bottomOffset = new Vector2(- m_bottomOffset.y, m_bottomOffset.x) ;
                coll.collisionBoxX = 0.04f;
                coll.collisionBoxY = 0.42f;
                coll.rightOffset = new Vector2(- m_rightOffset.y, m_rightOffset.x) ;
                coll.leftOffset = new Vector2(m_leftOffset.y, m_leftOffset.x) ;
                coll.topOffset = new Vector2(- m_topOffset.y, m_topOffset.x);
                break;
        }
    }

    private void ApplyGravityForce()
    {
        if (hasGravity)
        {
            switch(m_GravityDirection)
            {
                case GravityDirection.Down:
                    playerRB.AddForce(new Vector2(0, -1 * gravityScale) * gravityScale);
                    break;
                case GravityDirection.Left:
                    playerRB.AddForce(new Vector2(-1 * gravityScale, 0) * gravityScale);
                    break;
                case GravityDirection.Up:
                    playerRB.AddForce(new Vector2(0, 1 * gravityScale) * gravityScale);
                    break;
                case GravityDirection.Right:
                    playerRB.AddForce(new Vector2(1 * gravityScale, 0) * gravityScale);
                    break;   
                default:
                Debug.LogWarning("Gravity should not be null");
                break;
            }
        }
    }

    public void DisableGravity()
    {
        hasGravity = false;
    }

    public void EnableGravity()
    {
        hasGravity = true;
    }

}


