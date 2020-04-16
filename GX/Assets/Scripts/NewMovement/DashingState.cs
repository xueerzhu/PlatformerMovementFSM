using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroMovementFSM;
public class DashingState : MonoBehaviour, IHeroState
{
    private Vector2 dashDir;  // dir differ from input based on orientation and current grav dir
    public float dashSpeed = 20;
    public float MAX_DASH = 3;
    [SerializeField]
    private float m_dashTime;

    public void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        
        NormalizeDashInput(input);
        GravitySwitchOnDash(grav);
        m_dashTime = 0;
    }

    private void NormalizeDashInput(InputManager input)
    {
        if (Mathf.Abs(input.horizontalInput) > Mathf.Abs(input.verticalInput))
            dashDir = new Vector3(input.horizontalInput, 0,0);
        else dashDir = new Vector3(0, input.verticalInput,0);
    }

    public IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        // Legacy dash design
        /*if (collision.onLeftWall)
        {
            gameObject.transform.position += new Vector3(collision.transformRight.x, collision.transformRight.y, 0) ;  // nudge avoid clipping into wall due to rotation
            //gameObject.GetComponent<FallingState>().gravityDir = new Vector3(-1, 0, 0);
            gameObject.GetComponent<FallingState>().gravityDir = -collision.transformRight;
            return gameObject.GetComponent<FallingState>();
        }
        
        if (collision.onRightWall) 
        {
            gameObject.transform.position += new Vector3(-collision.transformRight.x, -collision.transformRight.y, 0);
            //gameObject.GetComponent<FallingState>().gravityDir = new Vector3(1, 0, 0);
            gameObject.GetComponent<FallingState>().gravityDir = collision.transformRight;
            return gameObject.GetComponent<FallingState>();
        }*/
        
        if (m_dashTime > MAX_DASH)
        {
            return gameObject.GetComponent<FallingState>();
        }
        else
        {
            // New dash design: change grav on enter. 
            return null;
        }
        
    }

    void GravitySwitchOnDash(GravityManager grav)
    {
        if (dashDir.y == -1)
        {
            grav.m_GravityDirection = GravityManager.GravityDirection.Down;
        }
        else if (dashDir.y == 1)
        {
            grav.m_GravityDirection = GravityManager.GravityDirection.Up;
            
        }
        else if(dashDir.x == 1)
        {
            grav.m_GravityDirection = GravityManager.GravityDirection.Right;
        }
        else if (dashDir.x == -1)
        {
            grav.m_GravityDirection = GravityManager.GravityDirection.Left;
        }
    }
    
    public string GetStateName()
    {
        return "DashingState";
    }
    
    public void StateUpdate()
    {
        m_dashTime += Time.deltaTime;
        Vector3 v3 = new Vector3(dashDir.x, dashDir.y, 0);
        gameObject.transform.position += dashSpeed * v3 * Time.fixedDeltaTime;
    }
    
    
}