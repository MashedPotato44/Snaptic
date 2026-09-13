using UnityEngine;
using UnityEngine.InputSystem;

public class SnapForce : MonoBehaviour
{
    private PlayerInputActions playerControls;
    private InputAction snap;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerControls = GetComponent<PlayerController>().GetControls(); //picks up player controlls from playercontroller
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
