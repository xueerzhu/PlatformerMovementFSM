using System.Collections;
using System.Collections.Generic;
using HeroMovementFSM;
using UnityEngine;

public abstract class OnGroundState : MonoBehaviour, IHeroState
{
    public virtual void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav) {}
    public virtual IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        if (input.jumpInput == 1) return hero.gameObject.GetComponent<JumpState>();

            if (!collision.onGround)
        {
            return hero.gameObject.GetComponent<FallingState>();
        }
        else
        {
            if (collision.onRightWall || collision.onLeftWall)
            {
                return gameObject.GetComponent<IdleState>();
            }
            
            if (input.dashInput == 1 && (input.verticalInput != 0 || input.horizontalInput != 0)) return hero.gameObject.GetComponent<DashingState>();  
        }
        
        

        return null;
    }

    public virtual void StateUpdate()
    {
        
    }

    public virtual string GetStateName()
    {
        return null;
    }
    
}
