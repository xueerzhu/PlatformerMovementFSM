using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private string horizontalControl;
    private string verticalControl;
    private string dashControl;
    private string respawnControl;
    private string jumpControl;

    public float horizontalInput
    {
        get {return Input.GetAxis(horizontalControl);}
        private set{}
    }
    
    public float jumpInput
    {
        get {return Input.GetButtonDown(jumpControl)? 1f: 0f;}
        private set{}
    }
    
    public float verticalInput
    {
        get {return Input.GetAxis(verticalControl);}
        private set{}
    }
    
    public float dashInput
    {
        get {return Input.GetButtonDown(dashControl) ? 1f: 0f;}
        private set{}
    }
    
    public float respawnInput
    {
        get {return Input.GetButtonDown(respawnControl)? 1f: 0f;}  //get button down, or it would return 1 multiple times in 1 frame, not ideal for toggle
        private set{}
    }

    private void Start()
    {
        horizontalControl = gameObject.tag == "PlayerOne" ? "Horizontal_P1": "Horizontal_P2";
        verticalControl = gameObject.tag == "PlayerOne" ? "Vertical_P1": "Vertical_P2";
        dashControl = gameObject.tag == "PlayerOne" ? "Dash_P1": "Dash_P2";
        respawnControl = gameObject.tag == "PlayerOne" ? "Respawn_P1": "Respawn_P2";
        jumpControl = gameObject.tag == "PlayerOne" ? "Jump_P1": "Jump_P2";
    }

    void Update() {}
}
