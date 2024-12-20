using UnityEngine;

public class Player : MonoBehaviour
{
    public InputSystem_Actions playerInput;
    public CharacterController characterController;
    public PlayerStateMachine stateMachine { get; private set; }
    
    // Creating references to states the player has
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkingState walkingState { get; private set; }
    public PlayerSprintState sprintState { get; private set; }
    public PlayerJumpingState jumpingState { get; private set; }
    public PlayerFallingState fallingState { get; private set; }

    // Store input variables
    public Vector2 currentMovementInput;
    public Vector3 currentMovement;
    public bool isMovementPressed;
    public bool isRunPressed;
    public float rotationFactorPerFrame = 15f;
    
    // Jumping variables
    public bool isJumpPressed = false;
    public float jumpForce = 2f;
    
    // Gravity variables
    public float groundedGravity = -.05f;
    public float gravity = -9.8f;

    // Speed variables
    public float movementSpeed = 7f;
    public float sprintSpeed = 10f;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        
        // Creating states for the state machine to be used
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkingState = new PlayerWalkingState(this, stateMachine, "Walking");
        sprintState = new PlayerSprintState(this, stateMachine, "Sprinting");
        jumpingState = new PlayerJumpingState(this, stateMachine, "Jumping");
        fallingState = new PlayerFallingState(this, stateMachine, "Falling");
        
        // Set reference to variables
        playerInput = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Start the state machine in the idle state
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        // Allows the state machine to update, as it is not a monobehavior script
        stateMachine.currentState.Update();
        
        Debug.Log(CheckIfGrounded());
    }

    public bool CheckIfGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.4f, groundLayer);
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
