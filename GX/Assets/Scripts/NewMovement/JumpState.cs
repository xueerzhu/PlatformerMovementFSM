using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroMovementFSM;

public class JumpState : MonoBehaviour, IHeroState
{
    // TODO: to be polished;
    public float MAX_JUMP = 2f;
    public float jumpSpeed = 100;
    
    private CollisionManager coll;
    [SerializeField] private float m_jumpTime;

    public void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        coll = collision;
        m_jumpTime = 0f;
    }
    
    public IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        if (m_jumpTime > MAX_JUMP)
        {
            return gameObject.GetComponent<FallingState>();
        }
        else
        {
            return null;
        }
        
    }
    
    public void StateUpdate()
    {
        m_jumpTime += Time.fixedDeltaTime;
        Debug.Log(m_jumpTime);
        gameObject.transform.position += jumpSpeed * new Vector3(coll.transformUp.x, coll.transformUp.y, 0) * Time.fixedDeltaTime;
    }

    public string GetStateName()
    {
        return "JumpState";
    }
}
