using System.Collections;
using System.Collections.Generic;
using HeroMovementFSM;
using UnityEngine;


public class FallingState : MonoBehaviour, IHeroState 
{
    private float fallHorizontalDir; // horizontal movement control while falling
    public float fallHorizontalSpeed = 5;
    private Vector3 fallHorizontalRight;

    public void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav) {}
    
    public float fallSpeed = 10;
    private Vector3 fallDir;
    private Vector2 airControlDir;

    /*[SerializeField]
    private Vector3 m_gravityDir = new Vector3(0, -1, 0);
    public Vector3 gravityDir
    {
        get { return m_gravityDir; }
        set { m_gravityDir = value; }
    }*/
    
    

    private Vector2 gravityDir;
    public IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        fallDir = grav.GetGravityVector2();
        airControlDir = grav.GravCorrectWorldToLocalInput(input.horizontalInput, input.verticalInput);
        fallHorizontalRight = collision.transformRight;
        
        
        if (collision.onGround)
        {
            return gameObject.GetComponent<IdleState>();
        }
        
        if (collision.transformUp.x == 0) fallHorizontalDir = input.horizontalInput;
        else if (collision.transformUp.x == 1) fallHorizontalDir = - input.verticalInput;
        else if (collision.transformUp.x == -1) fallHorizontalDir = input.verticalInput;

        if ((collision.onRightWall && fallHorizontalDir > 0) || collision.onLeftWall && fallHorizontalDir < 0)
        {
            fallHorizontalDir = 0;
        }
        
        return null;
    }
    
    public void StateUpdate()
    {
        Fall();
        AirControl();
    }
    
    private void Fall()
    {
        gameObject.transform.position += fallSpeed * fallDir * Time.fixedDeltaTime;
    }

    void AirControl()
    {
        Vector3 v3 = new Vector3(airControlDir.x, airControlDir.y, 0);
        gameObject.transform.position += fallSpeed * v3 * Time.fixedDeltaTime;
    }



    public string GetStateName()
    {
        return "FallingState";
    }
}
