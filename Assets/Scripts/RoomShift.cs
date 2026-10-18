using UnityEngine;

public class RoomShift : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SnapOrbit snapMarker;
    [SerializeField] private RoomGridSnap snapGrid;

    public int currentRoomX;
    public int currentRoomY;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float roomSpeed = 2f;
    private float roomShiftDuration = 1f;
    private float timePassed = 0f;
    public float roomShiftTimer;
    private float xPos;
    private float yPos;
    private float transformRatio;
 

    private void Start()
    {
        currentRoomX = Mathf.RoundToInt(transform.position.x / snapGrid.roomSize);
        currentRoomY = Mathf.RoundToInt(transform.position.y / snapGrid.roomSize);
    }

    void Update()
    {
        if (player.roomShiftFlag && currentRoomX == Mathf.RoundToInt(player.gridPos.x / snapGrid.roomSize) && currentRoomY == Mathf.RoundToInt(player.gridPos.y / snapGrid.roomSize))
        {
            if (player.roomShiftStart)
            {
                player.roomShiftStart = false;
                startPos = transform.position;
                xPos = transform.position.x + (snapMarker.wallSide.x * snapGrid.roomSize);
                yPos = transform.position.y + (snapMarker.wallSide.y * snapGrid.roomSize);
                
            }

            timePassed += Time.deltaTime;
            transformRatio = ExpMove(timePassed);
            targetPos = new Vector3(xPos, yPos, transform.position.z);
            transform.position = Vector3.Lerp(startPos, targetPos, transformRatio);
        }


        if (transformRatio > 1)
        {
            player.roomShiftFlag = false;
            timePassed = 0f;
            startPos = targetPos;
            currentRoomX = Mathf.RoundToInt(transform.position.x / snapGrid.roomSize);
            currentRoomY = Mathf.RoundToInt(transform.position.y / snapGrid.roomSize);
            Debug.Log(transformRatio);


        }
    }

    private float ExpMove(float x) //exponential camera
    {
        float y = (1 - Mathf.Exp((-roomSpeed * x) / roomShiftDuration)) / (1 - Mathf.Exp(-roomSpeed));
        return y;
    }
}
