using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroMovementFSM;

public class GravityManager : MonoBehaviour
{
    public enum GravityDirection
    {
        Down,
        Right,
        Left,
        Up
    };
    public GravityDirection m_GravityDirection;
    private void Awake()
    {
        m_GravityDirection = GravityDirection.Down;
    }
    
    void Update()
    {
        UpdatePlayerGFX();
    }
    
    void UpdatePlayerGFX()
    {
        switch(m_GravityDirection)
        {
            case GravityDirection.Down:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case GravityDirection.Left:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
            case GravityDirection.Up:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                break;
            case GravityDirection.Right:
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                break;   
            default:
                Debug.LogWarning("Gravity should not be null");
                break;
        }
    }

    public Vector2 GetGravityVector2()
    {
        switch(m_GravityDirection)
        {
            case GravityDirection.Down:
                return new Vector2(0, -1);
                break;
            case GravityDirection.Left:
                return new Vector2(-1, 0);
                break;
            case GravityDirection.Up:
                return new Vector2(0, 1);
                break;
            case GravityDirection.Right:
                return new Vector2(1, 0);
                break;   
            default:
                Debug.LogWarning("Gravity should not be null");
                return new Vector2(0, -1);
                break;
        }
    }

    // Using control input relative to world instead of player
    public Vector2 GravCorrectWorldToLocalInput(float horizontal, float vertical)
    {
        switch(m_GravityDirection)
        {
            case GravityDirection.Down:
                return new Vector2(horizontal, 0);
                break;
            case GravityDirection.Left:
                return new Vector2(0, vertical);
                break;
            case GravityDirection.Up:
                return new Vector2(horizontal, 0);
                break;
            case GravityDirection.Right:
                return new Vector2(0, vertical);
                break;   
            default:
                Debug.LogWarning("Gravity should not be null");
                return new Vector2(horizontal, vertical);
                break;
        }
    }
}
