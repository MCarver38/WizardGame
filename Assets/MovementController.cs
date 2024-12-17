using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    CharacterController characterController;

    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;
    private readonly float rotationFactorPerFrame = 15f;
    
    [SerializeField] private float movementSpeed = 10f;

    private void Awake()
    {
        // Set reference to variables
        playerInput = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
        
        // Setup player input callbacks
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
    }

    private void Update()
    {
        characterController.Move(currentMovement * (movementSpeed * Time.deltaTime));
        HandleRotation();
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        // Read the values of the players inputs and store them into a Vector 2
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        
        // Check if the player is trying to move and change bool accordingly
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        
        // Store the values of the direction the character should look toward
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        
        // The current rotation of the character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            // Creates a rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
