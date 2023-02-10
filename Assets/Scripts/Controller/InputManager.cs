using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    private CharacterMovement _characterMovement;


    InputAction movement;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _characterMovement = GetComponent<CharacterMovement>();
        
        movement = _playerInput.actions.FindAction("Move");  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _characterMovement.Move(movement.ReadValue<Vector2>());
    }
}
