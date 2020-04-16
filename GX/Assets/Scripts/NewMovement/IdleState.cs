using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroMovementFSM;
public class IdleState : OnGroundState
{
    private float walkHorizontalDir;
    public override void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav) {}
    
    public override IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {

        if (input.jumpInput == 1)
        {
            return base.HandleInput(hero, input, collision, grav);
        }
        if (input.respawnInput == 1)
        {
            return hero.gameObject.GetComponent<RespawnState>();
        }
        
        if (!collision.onGround)
        {
            return hero.gameObject.GetComponent<FallingState>();
        }
        
        // illogical idle position, self adjust , also useful for when respawning into extremely invalid position 
        if (collision.onGround && collision.onLeftWall && collision.onRightWall)
        {
            gameObject.transform.position += new Vector3(collision.transformUp.x, collision.transformUp.y, 0) * 0.1f;
        }
   
        if (collision.transformUp.x == 0) walkHorizontalDir = input.horizontalInput;
        else if (collision.transformUp.x == 1) walkHorizontalDir = - input.verticalInput;
        else if (collision.transformUp.x == -1) walkHorizontalDir = input.verticalInput;

        if (input.dashInput == 1)
        {
            if ((input.verticalInput != 0 || input.horizontalInput != 0))
            {
                // cannot dash down
                if ((collision.transformUp.y == 1 && input.verticalInput != -1)
                    || (collision.transformUp.y == -1 && input.verticalInput != 1)
                    || (collision.transformUp.x == -1 && input.horizontalInput != 1)
                    || (collision.transformUp.x == 1 && input.horizontalInput != -1))
                {
                    return hero.gameObject.GetComponent<DashingState>();
                }
                
            }  
        }
        
        if (collision.transformUp.x == 0)
        {
            // ||| on ceiling need to fix
            if ((input.horizontalInput > 0 && !collision.onRightWall) || (input.horizontalInput < 0 && !collision.onLeftWall))
            {
                return gameObject.GetComponent<WalkingState>();
            }
        }
        else
        {
            if (collision.onLeftWall)
            {
                // can only go transformRight dir
                if ((collision.transformRight.y > 0 && input.verticalInput > 0) ||
                    (collision.transformRight.y < 0 && input.verticalInput < 0))
                {
                    return gameObject.GetComponent<WalkingState>();
                }
            }
            else if (collision.onRightWall)
            {
                // can only go -transformRight dir
                if ((collision.transformRight.y > 0 && input.verticalInput < 0) ||
                    (collision.transformRight.y < 0 && input.verticalInput > 0))
                {
                    return gameObject.GetComponent<WalkingState>();
                }
            }
            else if (input.verticalInput != 0)
            {
                return gameObject.GetComponent<WalkingState>();
            }
        }

        return null;

    }

    public override string GetStateName()
    {
        return "IdleState";
    }

    public override void StateUpdate() {}
}
