using System.Collections;
using System.Collections.Generic;
using HeroMovementFSM;
using UnityEngine;

public class RespawnState : MonoBehaviour, IHeroState 
{
    private float flyHorizontalDir;
    private float flyVerticalDir;
    public float flySpeed = 10;
    public Hero m_Hero;

    public void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav) {}
    
    public IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        // reset gravity dir
        grav.m_GravityDirection = GravityManager.GravityDirection.Down;
        
        flyHorizontalDir = input.horizontalInput;
        flyVerticalDir = input.verticalInput;
        m_Hero = hero;

        if (input.respawnInput == 1)
        {
            return gameObject.GetComponent<IdleState>();
        }
        
        return null;
    }
    
    public void StateUpdate()
    {
        gameObject.transform.position += flySpeed * flyHorizontalDir * Vector3.right* Time.fixedDeltaTime;
        gameObject.transform.position += flySpeed * flyVerticalDir * Vector3.up* Time.fixedDeltaTime;
    }

    public string GetStateName()
    {
        return "RespawnState";
    }
}


