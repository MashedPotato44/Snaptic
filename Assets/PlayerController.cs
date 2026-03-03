using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerInputActions playerControls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Vector2 moveDirection = Vector2.zero;
    private float moveSpeed = 5f;
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
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("We jumped");
    }
}


