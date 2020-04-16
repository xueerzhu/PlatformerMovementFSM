using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroMovementFSM;
using UnityEditor.UIElements;

public class WalkingState : OnGroundState
{
    private float walkHorizontalDir;
    private Vector2 walkDir;
    public float walkSpeed = 5;
    private Vector3 walkRight;
    private SpriteRenderer sprite;

    public override void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    
    public override IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        walkDir = grav.GravCorrectWorldToLocalInput(input.horizontalInput, input.verticalInput);
        
        FlipSprite(grav);
        
        if (input.jumpInput == 1) return base.HandleInput(hero, input, collision, grav);
        walkRight = collision.transformRight;
        if (collision.onGround)
        {
            // make input global instead of relative to player local
            if (collision.transformUp.y == 1) walkHorizontalDir = input.horizontalInput;
            else if (collision.transformUp.y == -1) walkHorizontalDir = - input.horizontalInput;
            else if (collision.transformUp.x == 1) walkHorizontalDir = - input.verticalInput;
            else if (collision.transformUp.x == -1) walkHorizontalDir = input.verticalInput;
            
            if (collision.onRightWall && collision.onGround && walkHorizontalDir > 0)
            {
                return base.HandleInput(hero, input, collision, grav);
            }
            
            if (collision.onLeftWall && collision.onGround && walkHorizontalDir < 0)
            {
                return base.HandleInput(hero, input, collision, grav);
            }
            
            if (walkHorizontalDir == 0)
            {
                return gameObject.GetComponent<IdleState>();
            }

            return base.HandleInput(hero, input, collision, grav);
        }
        else
        {
            return base.HandleInput(hero, input, collision, grav);
        }
        
        
        
        return null;
    }
    
    public override string GetStateName()
    {
        return "WalkingState";
    }
    
    public override void StateUpdate()
    {
        /*gameObject.transform.position += walkSpeed * walkHorizontalDir * walkRight * Time.fixedDeltaTime;

        if (walkRight.y > 0) sprite.flipX = walkHorizontalDir * walkRight.x < 0 || walkHorizontalDir * walkRight.y < 0;
        else sprite.flipX = walkHorizontalDir * walkRight.x < 0 || walkHorizontalDir * walkRight.y > 0;*/

        
        
        Vector3 v3 = new Vector3(walkDir.x, walkDir.y, 0);
        gameObject.transform.position += walkSpeed * v3 * Time.fixedDeltaTime;
    }

    void FlipSprite(GravityManager grav)
    {
        switch (grav.m_GravityDirection)
        {
            case GravityManager.GravityDirection.Down:
                if (walkDir.x < 0) sprite.flipX = true;
                else if (walkDir.x > 0) sprite.flipX = false;
                break;
            case GravityManager.GravityDirection.Up:
                if (walkDir.x > 0) sprite.flipX = true;
                else if (walkDir.x < 0) sprite.flipX = false;
                break;
            case GravityManager.GravityDirection.Right:
                if (walkDir.y < 0) sprite.flipX = true;
                else if (walkDir.y > 0) sprite.flipX = false;
                break;
            case GravityManager.GravityDirection.Left:
                if (walkDir.y > 0) sprite.flipX = true;
                else if (walkDir.y < 0) sprite.flipX = false;
                break;
            default:
                break;
        }
    }
    
    
}
