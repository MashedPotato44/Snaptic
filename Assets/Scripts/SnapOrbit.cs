using Unity.VisualScripting;
using UnityEngine;

public class SnapOrbit : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float radius = 1.5f;
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

    }
}
