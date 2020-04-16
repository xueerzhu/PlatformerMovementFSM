using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroMovementFSM;

public class Hero : MonoBehaviour
{
    private InputManager m_Input;

    private IHeroState m_State;

    private CollisionManager m_Collision;
    private GravityManager m_Grav;

    public string m_CurrentState;
    private void Start()
    {
        m_Input = GetComponent<InputManager>();
        m_State = gameObject.GetComponent<FallingState>();
        m_Collision = gameObject.GetComponent<CollisionManager>();
        m_CurrentState = m_State.GetStateName();
        m_Grav = gameObject.GetComponent<GravityManager>();
    }

    public void HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav)
    {
        IHeroState state = m_State.HandleInput(hero, m_Input, m_Collision, m_Grav);
        if (state != null)
        {
            //Destroy(m_State as Component);
            m_State = state;
            m_State.Enter(hero, m_Input, m_Collision, m_Grav);
            m_CurrentState = m_State.GetStateName();
        }
    }

    public void FixedUpdate()
    {
        HandleInput(this, m_Input, m_Collision, m_Grav);
        m_State.StateUpdate();
       
    }
}