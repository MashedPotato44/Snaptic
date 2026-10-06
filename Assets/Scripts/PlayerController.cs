using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector2 moveDirection = Vector2.zero;
    public float moveSpeedx = 10f;
    public float moveSpeedy = 30f;
    public float jumpVelocity = 10f;
    public float jumpUpGravity = 2.5f;
    public float jumpDownGravity = 3.5f;
    public Vector2 boxSize;
    public float castDistance;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 40f;

    public LayerMask groundLayer;
    public bool grounded; //to check grounding in the inspector
    public bool isHanging;
    public bool isRising;
    public float hangVelocityRange = 2f;
    public float hangtime = 0.05f;
    public bool hangTimerSet = false;
    private float jumpBuffer = 0.15f;
    public float jumpBufferTimer = 0f;
    public float hangTimer = 0f;

    public bool snapPressed = false;

    public bool jumpPressed = false;
    private float jumpReleaseMod = 1.5f;

    private float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    private float fallSpeedMax = -30f;

    public Vector2 gridPos = Vector2Int.zero;
    public bool cameraMoveFlag = false;
    public bool cameraMoveStart = false;

    [SerializeField] private Rigidbody2D rb;

    private InputAction move;
    private InputAction jump;
    private InputAction snap;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    public PlayerInputActions GetControls() => playerControls; //creates function for SnapForce scriptto use

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;

        snap = playerControls.Player.Snap;
        snap.Enable();
        snap.performed += Snap;


    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        snap.Disable();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        jumpPressed = jump.IsPressed();
        moveDirection = move.ReadValue<Vector2>();

        jumpBufferTimer -= Time.deltaTime;
        hangTimer -= Time.deltaTime;

        if (isGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (rb.linearVelocity.y > 0.0001f) //changes gravity for up and down
        {
            rb.gravityScale = jumpUpGravity;
            isRising = true;
        }

        else
        {
            rb.gravityScale = jumpDownGravity;
            isRising = false;
        }

        if ((Mathf.Abs(rb.linearVelocity.y) < hangVelocityRange) && !grounded) //checks if player is at jump apex
        {
            if (hangTimerSet == false)
            {
                hangTimer = hangtime;
                hangTimerSet = true;
                isHanging = true;
            }

            rb.gravityScale = 0f;

            if (hangTimer < 0f)
            {
                rb.gravityScale = jumpDownGravity;
            }
        }

        if (grounded)
        {
            hangTimerSet = false;
            isHanging = false;
        }

        if (rb.linearVelocity.y < fallSpeedMax) //sets terminal velocity
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fallSpeedMax);
        }
        

        if (!jumpPressed && rb.linearVelocity.y > 0f) //adjustable jump
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / jumpReleaseMod);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = rb.linearVelocity.x;
        float speedTarget = moveDirection.x * moveSpeedx;

        float rate = Mathf.Abs(speedTarget) > Mathf.Abs(currentSpeed) ? acceleration : deceleration; //always brings velocity to movespeedx
        float speed = Mathf.MoveTowards(currentSpeed, speedTarget, rate * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);


        if (coyoteTimeCounter > 0f && jumpBufferTimer > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity); //allows a jump if jump is pressed before buffer timer runs out
        }

        if (rb.linearVelocity.y > moveSpeedy) //clamps upward velocity
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, moveSpeedy);
        }

    }

    public bool isGrounded() //checks ground is below player
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            grounded = true;
            return true;
        }
        else
        {
            grounded = false;
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - (transform.up * castDistance), boxSize); //offsets boxcast below player
    }

    private void Jump(InputAction.CallbackContext context)
    {
        jumpBufferTimer = jumpBuffer; //resets buffer timer
    }

    private void Snap(InputAction.CallbackContext context)
    {
        snapPressed = true; //sets snap pressed for snap logic
    }

    private void OnTriggerEnter2D(Collider2D collision) //Logs players position when entering a new room
    {
        gridPos = new Vector2(collision.transform.position.x, collision.transform.position.y);
        cameraMoveFlag = true;
        cameraMoveStart = true;
    }
}


