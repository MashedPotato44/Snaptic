using Unity.VisualScripting;
using UnityEngine;

public class SnapOrbit : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float radius = 1.5f;

    [SerializeField] private float handOverlapRadius = 0.5f;
    [SerializeField] private LayerMask layer;
    public bool wallFlag = false;
    public Vector2 wallSide;

    private Camera cam;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
        float mouseAngle = Mathf.Atan2(mousePos.y - player.position.y, mousePos.x - player.position.x);
        transform.position = new Vector3(
            player.position.x + Mathf.Cos(mouseAngle)*radius, 
            player.position.y + Mathf.Sin(mouseAngle)*radius, 
            0f
        );

        Collider2D overlap = Physics2D.OverlapCircle(transform.position, handOverlapRadius, layer);
        if (overlap != null)
        {
            wallSide = overlap.offset/overlap.offset.magnitude;
            wallFlag = true;
        }

        else
        {
            wallFlag = false;
        }

    }
}
