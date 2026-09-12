using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector2 moveDirection = Vector2.zero;
    public float moveSpeed = 8f;
    public float jumpVelocity = 10f;
    public float jumpUpGravity = 2.5f;
    public float jumpDownGravity = 3.5f;
    public Vector2 boxSize;
    public float castDistance;
    
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

    public bool jumpPressed = false;
    private float jumpReleaseMod = 1.5f;

    private float coyoteTime = 0.2f;
    public float coyoteTimeCounter;

    private float fallSpeedMax = -30f;

    [SerializeField] private Rigidbody2D rb;

    private InputAction move;
    private InputAction jump;
    
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        jump = playerControls.Player.Jump;
        jump.Enable();
        jump.performed += Jump;
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
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
       
        if ((Mathf.Abs(rb.linearVelocity.y) < hangVelocityRange) && !grounded)
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

        if (!jumpPressed && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / jumpReleaseMod);
        }

        if (rb.linearVelocity.y < fallSpeedMax)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fallSpeedMax);
        }

        Debug.Log(rb.linearVelocity.y);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        
        
        if (coyoteTimeCounter > 0f && jumpBufferTimer > 0f) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity); //allows a jump if jump is pressed before buffer timer runs out
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
}


