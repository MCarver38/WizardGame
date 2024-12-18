using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    private InputSystem_Actions playerInput;
    CharacterController characterController;

    // Store input variables
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private const float rotationFactorPerFrame = 15f;
    
    // Jumping variables
    private bool isJumpPressed = false;
    private float initialJumpVelocity;
    private float maxJumpHeight = 0.5f;
    private float maxJumpTime = 0.5f;
    private bool isJumping = false;
    
    // Gravity variables
    private const float groundedGravity = -.05f;
    private float gravity = -9.8f;

    // Speed variables
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;

    private void Awake()
    {
        // Set reference to variables
        playerInput = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
        
        // Setup player input callbacks
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Sprint.started += OnSprintInput;
        playerInput.CharacterControls.Sprint.canceled += OnSprintInput;
        playerInput.CharacterControls.Jump.started += OnJumpInput;
        playerInput.CharacterControls.Jump.canceled += OnJumpInput;

        setupJumpVariables();
    }

    private void Update()
    {
        // Checks to see if the player is sprinting and changes the movement speed accordingly
        if (isRunPressed)
        {
            characterController.Move(currentMovement * (sprintSpeed * Time.deltaTime));
        }
        else
        {
            characterController.Move(currentMovement * (movementSpeed * Time.deltaTime));
        }
        
        HandleRotation();
        HandleGravity();
        HandleJump();
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
    
    private void OnSprintInput(InputAction.CallbackContext context)
    {
        // Changes the bool to reflect the players inputs, if they are pressing the sprint button or not
        isRunPressed = context.ReadValueAsButton();
    }
    
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        // Changes the bool to reflect the players jumping input
        isJumpPressed = context.ReadValueAsButton();
    }

    private void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        float fallMultiplier = 1.75f;
        // Keeps the player grounded and creates gravity if the player is not grounded
        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
        }
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

    private void HandleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            currentMovement.y = initialJumpVelocity * .5f;
        }
        else if (isJumping && characterController.isGrounded && !isJumpPressed)
        {
            isJumping = false;
        }
    }
    
    private void setupJumpVariables()
    {
        // Create the variables for the jump to determine the height and fall of the jump
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
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
