using UnityEngine;

public class Player : MonoBehaviour
{
    public InputSystem_Actions playerInput;
    public CharacterController characterController;
    public Animator animator;
    private PlayerStateMachine stateMachine { get; set; }
    
    // Creating references to states the player has
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkingState walkingState { get; private set; }
    public PlayerSprintState sprintState { get; private set; }
    public PlayerJumpingState jumpingState { get; private set; }
    public PlayerFallingState fallingState { get; private set; }

    // Store input variables
    [HideInInspector] public Vector2 currentMovementInput;
    [HideInInspector] public Vector3 currentMovement;
    [HideInInspector] public bool isMovementPressed;
    [HideInInspector] public bool isRunPressed;
    [HideInInspector] public float rotationFactorPerFrame = 15f;
    
    // Jumping variables
    [HideInInspector] public bool isJumpPressed = false;
    [HideInInspector] public bool requireNewJumpPress;
    [HideInInspector] public float jumpCooldown = .25f;
    [HideInInspector] public float jumpCooldownTimer;
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
        animator = GetComponentInChildren<Animator>();
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
    }

    public bool CheckIfGrounded()
    {
        const float groundCheckDistance = 0.1f;
        return Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public float clampedDeltaTime()
    {
        return Mathf.Min(Time.deltaTime, 0.0333f);
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
