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
    public PlayerDodgeState dodgeState { get; private set; }

    // Store input variables
    [HideInInspector] public Vector2 currentMovementInput;
    [HideInInspector] public Vector3 currentMovement;
    [HideInInspector] public Vector3 relativeMovement;
    [HideInInspector] public bool isMovementPressed;
    [HideInInspector] public bool isRunPressed;
    [HideInInspector] public float rotationFactorPerFrame = 15f;
    
    // Jumping variables
    [HideInInspector] public bool isJumpPressed = false;
    [HideInInspector] public bool requireNewJumpPress;
    [HideInInspector] public float jumpCooldown = .25f;
    [HideInInspector] public float jumpCooldownTimer;
    [HideInInspector] public float jumpForce = 2f;
    
    // Gravity variables
    [HideInInspector] public float groundedGravity = -.05f;
    [HideInInspector] public float gravity = -9.8f;

    // Speed variables
    [HideInInspector] public float movementSpeed = 7f;
    [HideInInspector] public float sprintSpeed = 10f;
    
    // Dodge variables
    [HideInInspector] public float dodgeSpeed = 50f;
    [HideInInspector] public float dodgeDuration = 0.2f;
    [HideInInspector] public float dodgeDurationTimer;
    [HideInInspector] public float dodgeCooldown = 1f;
    [HideInInspector] public float dodgeCooldownTimer;
    [HideInInspector] public bool isDodgePressed;
    [HideInInspector] public bool requireNewDodgePress;
    [HideInInspector] public int dodgeManaAmount = 5;
    
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem dodgeParticles;
    public Mana mana;
    public GameObject characterVisuals;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        
        // Creating states for the state machine to be used
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkingState = new PlayerWalkingState(this, stateMachine, "Walking");
        sprintState = new PlayerSprintState(this, stateMachine, "Sprinting");
        jumpingState = new PlayerJumpingState(this, stateMachine, "Jumping");
        fallingState = new PlayerFallingState(this, stateMachine, "Falling");
        dodgeState = new PlayerDodgeState(this, stateMachine, "Dodge");
        
        // Set reference to variables
        playerInput = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mana = GetComponent<Mana>();
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

    public Vector3 GetCameraRelativeVector()
    {
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        
        Vector3 forwardRelativeVerticalInput = currentMovement.z * forward;
        Vector3 rightRelativeVerticalInput = currentMovement.x * right;
        
        Vector3 rotateToCameraDirection = forwardRelativeVerticalInput + rightRelativeVerticalInput;
        rotateToCameraDirection.y = currentMovement.y;

        return rotateToCameraDirection;
    }

    public bool CheckIfGrounded()
    {
        const float groundCheckDistance = 0.1f;
        return Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, groundLayer);
    }

    public void StartDodgeBlink()
    {
        dodgeParticles.Play();
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
