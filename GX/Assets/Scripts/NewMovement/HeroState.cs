using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroMovementFSM
{
    public interface IHeroState
    {
        void Enter(Hero hero, InputManager input, CollisionManager collision, GravityManager grav); 
        IHeroState HandleInput(Hero hero, InputManager input, CollisionManager collision, GravityManager grav);
        void StateUpdate();
        string GetStateName();
    }
    
}
