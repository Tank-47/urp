using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// new Input System
/// </summary>
public class CharacterInputSystem : MonoBehaviour
{
    private Controls _controls;
    // Start is called before the first frame update
    private void Awake()
    {
        if (_controls == null)
        {
            _controls = new Controls();
        }
    }

    public Vector2 PlayerMovement
    {
        get => _controls.Player.Move.ReadValue<Vector2>();
    }

    public bool PlayerJump
    {
        get => _controls.Player.Move.triggered;
    }
    
    public Vector2 cameraLook
    {
        get => _controls.Player.CameraLook.ReadValue<Vector2>();
    }
    //内部函数
    

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    
}
