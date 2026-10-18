using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SnapForce : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private float radius = 3f;
    [SerializeField] private float objectForce = 5f;
    [SerializeField] private float playerForce = 7f;
    [SerializeField] private LayerMask pushable;
    [SerializeField] private float velocityYmult = 0.2f;


    void FixedUpdate()
    {
        if (player.snapPressed)
        {
            player.snapPressed = false;
            PerformSnap();
        }
    }

    private void PerformSnap()
    {
        Vector2 origin = transform.position;

        Collider2D[] victims = Physics2D.OverlapCircleAll(origin, radius, pushable);

        foreach (Collider2D victim in victims)
        {
            Rigidbody2D rb = victim.attachedRigidbody;
            if (rb == null) continue;

            Vector2 rb_Pos = rb.transform.position;
            Vector2 snapDirection = rb_Pos - origin;

            float force;

            if (rb.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                force = playerForce;
            }
            else
            {
                force = objectForce;
            }

            Vector2 snapImpulse = force * snapDirection;

            Vector2 currentVel = rb.linearVelocity;

            rb.linearVelocity = new Vector2(currentVel.x, currentVel.y * velocityYmult); //reducing velocity before snap to prevent massive vertical movement

            rb.AddForce(snapImpulse, ForceMode2D.Impulse);
       
        }
    }
}
