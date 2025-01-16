using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerNPCInteractState npcInteractState { get; private set; }
    public PlayerInventoryState inventoryState { get; private set; }

    // Store input variables
    [HideInInspector] public Vector2 currentMovementInput;
    [HideInInspector] public Vector3 currentMovement;
    [HideInInspector] public Vector3 relativeMovement;
    [HideInInspector] public bool isMovementPressed;
    [HideInInspector] public bool isRunPressed;
    [HideInInspector] public float rotationFactorPerFrame = 15f;
    [HideInInspector] public bool isInteractPressed;
    
    // Jumping variables
    [HideInInspector] public bool isJumpPressed;
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
    [SerializeField] private LayerMask interactableLayers;
    [SerializeField] private GameObject interactUI;
    [SerializeField] private TextMeshProUGUI interactUIText;

    public bool isInventoryButtonPressed;
    public bool requireNewInventoryPress;
    public bool isInventoryScreenOpen;
    
    public GameObject dialogueBox;
    public GameObject inventoryScreen;
    public Mana mana;
    public GameObject characterVisuals;
    public bool requireNewInteractPress;
    public CinemachineCamera freeLookCamera;

    private const float interactRange = 2f;
    public GameObject currentNearbyObject;
    private OutlineMaterialScript currentHighlight;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        
        // Creating states for the state machine to be used
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        walkingState = new PlayerWalkingState(this, stateMachine, "Walking");
        sprintState = new PlayerSprintState(this, stateMachine, "Walking");
        jumpingState = new PlayerJumpingState(this, stateMachine, "Jumping");
        fallingState = new PlayerFallingState(this, stateMachine, "Falling");
        dodgeState = new PlayerDodgeState(this, stateMachine, "Dodge");
        npcInteractState = new PlayerNPCInteractState(this, stateMachine, "Idle");
        inventoryState = new PlayerInventoryState(this, stateMachine, "Idle");
        
        // Set reference to variables
        playerInput = new InputSystem_Actions();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mana = GetComponent<Mana>();

        DialogueManager.OnDialogueEnd += OnDialogueEnd;
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
        CheckForNearbyInteractables();
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
    
    public void Interact()
    { 
        requireNewInteractPress = false;
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayers);
        
        Collider closestHit = null;
        var closestDistance = float.MaxValue;

        foreach (var hit in hits)
        {
            var distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hit;
            }
        }

        if (closestHit != null)
        {
            IInteractable interactable = closestHit.GetComponent<IInteractable>();
            interactable?.Interact();
        }
        else
        {
            Debug.Log("Nothing to interact with");
        }
    }

    private void CheckForNearbyInteractables()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactableLayers);
        Collider closestHit = null;
        var closestDistance = float.MaxValue;
        OutlineMaterialScript closestOutline = null;

        foreach (var hit in hits)
        {
            OutlineMaterialScript highlight = hit.GetComponent<OutlineMaterialScript>();
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hit;
                closestOutline = highlight;
            }
        }

        if (currentHighlight != null && currentHighlight != closestOutline)
        {
            currentHighlight.RemoveOutlineMaterial();
        }
        
        currentHighlight = closestOutline;

        if (currentHighlight != null)
        {
            currentHighlight.ApplyOutlineMaterial();
        }
        
        if (closestHit != null)
        {
            if (currentNearbyObject != closestHit.gameObject)
            {
                currentNearbyObject = closestHit.gameObject;
                ShowInteractUI();
            }
        }
        else
        {
            currentNearbyObject = null;
            currentHighlight = null;
            HideInteractUI();
        }
    }

    public void ShowInteractUI()
    {
        IInteractable interactable = currentNearbyObject.GetComponent<IInteractable>();
        string currentText = interactable?.GetInteractionPrompt();
        interactUIText.text = currentText;
        interactUI.SetActive(true);
    }
    
    public void HideInteractUI()
    {
        interactUI.SetActive(false);
    }

    public void StartDodgeBlink()
    {
        dodgeParticles.Play();
    }

    public void OnDialogueEnd()
    {
        stateMachine.ChangeState(idleState);
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
