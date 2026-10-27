using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomShiftCast : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SnapOrbit snapMarker;
    [SerializeField] private CapsuleCollider2D capsule;
    [SerializeField] private RoomManager roomManager;

    private RaycastHit2D[] hits = new RaycastHit2D[8];
    private ContactFilter2D filter;
    public LayerMask obstacles;
    private float castDist = 32f;
    private float maxTravel = 40f;
    public bool playerLocked = false;
    private float t = 0f;
    private Vector3 endPoint;
    private float speed = 0.2f;

    void Start()
    {
        filter = new ContactFilter2D();
        filter.SetLayerMask(obstacles);
        filter.useTriggers = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.roomShiftStart)
        {
            int count = capsule.Cast(-snapMarker.wallSide, filter, hits, castDist);
            if (hits[0].distance == 0)
            {
                hits[0].distance = maxTravel;
            }
            transform.SetParent(roomManager.roomGrid[Mathf.RoundToInt(roomManager.playerPos.x / roomManager.roomSize), Mathf.RoundToInt(roomManager.playerPos.y / roomManager.roomSize)].transform);
            endPoint = transform.localPosition + new Vector3(-snapMarker.wallSide.x * hits[0].distance, -snapMarker.wallSide.y * hits[0].distance, transform.localPosition.z);
            Debug.Log(transform.localPosition + " " + endPoint);
        }

        if (player.roomShiftFlag)
        {
            LockControls(true);
            t += speed * Time.deltaTime;
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPoint, t);
        }

        else
        {
            LockControls(false);
            t = 0f;
        }
    }

    public void LockControls(bool locked)
    {
        playerLocked = locked;

        if (playerLocked)
        {
            player.rb.linearVelocity = Vector2.zero;
        }
    }
}
