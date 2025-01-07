using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        
        player.playerInput.CharacterControls.Move.performed += OnMovementInput;
        player.playerInput.CharacterControls.Move.started += OnMovementInput;
        player.playerInput.CharacterControls.Sprint.started += OnSprintInput;
        player.playerInput.CharacterControls.Jump.started += OnJumpInput;
        player.playerInput.CharacterControls.Dodge.started += OnDodgeInput;
        player.playerInput.CharacterControls.Interact.started += OnInteractInput;
    }

    public override void Update()
    {
        base.Update();
        
        player.jumpCooldownTimer -= Time.deltaTime;
        player.dodgeCooldownTimer -= Time.deltaTime;
        
        HandleRotation();
        player.currentMovement.y = player.groundedGravity;
        
        if (!player.CheckIfGrounded())
        {
            stateMachine.ChangeState(player.fallingState);
        }
        
        if (player.isJumpPressed && player.jumpCooldownTimer <= 0 && player.requireNewJumpPress)
        {
            stateMachine.ChangeState(player.jumpingState);
        }

        if (player.isDodgePressed && player.dodgeCooldownTimer <= 0 && player.requireNewDodgePress && player.mana.IsEnoughManaToUse(player.dodgeManaAmount))
        {
            stateMachine.ChangeState(player.dodgeState);
        }

        if (player.isInteractPressed && player.requireNewInteractPress)
        {
            player.Interact();
            
            if (player.currentNearbyObject.CompareTag("InteractNPC"))
            {
                stateMachine.ChangeState(player.npcInteractState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.playerInput.CharacterControls.Move.canceled += OnMovementInput;
        player.playerInput.CharacterControls.Sprint.canceled += OnSprintInput;
        player.playerInput.CharacterControls.Jump.canceled += OnJumpInput;
        player.playerInput.CharacterControls.Dodge.canceled += OnDodgeInput;
        player.playerInput.CharacterControls.Interact.canceled += OnInteractInput;
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        
        // Store the values of the direction the character should look toward
        positionToLookAt.x = player.relativeMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = player.relativeMovement.z;
        
        // The current rotation of the character
        Quaternion currentRotation = player.transform.rotation;

        if (player.isMovementPressed)
        {
            // Creates a rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            player.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, player.rotationFactorPerFrame * Time.deltaTime);
        }
    }
    
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        // Read the values of the players inputs and store them into a Vector 2
        player.currentMovementInput = context.ReadValue<Vector2>();
        player.currentMovement.x = player.currentMovementInput.x;
        player.currentMovement.z = player.currentMovementInput.y;
        
        // Check if the player is trying to move and change bool accordingly
        player.isMovementPressed = player.currentMovementInput.x != 0 || player.currentMovementInput.y != 0;
    }
    
    private void OnSprintInput(InputAction.CallbackContext context)
    {
        // Changes the bool to reflect the players inputs, if they are pressing the sprint button or not
        player.isRunPressed = context.ReadValueAsButton();
    }
    
    private void OnJumpInput(InputAction.CallbackContext context)
    {
        // Changes the bool to reflect the players jumping input
        player.isJumpPressed = context.ReadValueAsButton();
        player.requireNewJumpPress = true;
    }

    private void OnDodgeInput(InputAction.CallbackContext context)
    {
        player.isDodgePressed = context.ReadValueAsButton();
        player.requireNewDodgePress = true;
    }
    
    private void OnInteractInput(InputAction.CallbackContext context)
    {
        player.isInteractPressed = context.ReadValueAsButton();
        player.requireNewInteractPress = true;
    }
}
