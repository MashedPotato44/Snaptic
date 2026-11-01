using System.Runtime.CompilerServices;
using UnityEngine;

public class RoomShift : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private SnapOrbit snapMarker;
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private MovetoRoom cam;
    [SerializeField] private Collider2D wallUp;
    [SerializeField] private Collider2D wallRight;
    [SerializeField] private Collider2D wallLeft;
    [SerializeField] private Collider2D wallDown;
    [SerializeField] private Collider2D roomCol;

    public int currentRoomCol;
    public int currentRoomRow;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float roomSpeed = 2f;
    private float roomShiftDuration = 1f;
    private float timePassed = 0f;
    public float roomShiftTimer;
    private float xPos;
    private float yPos;
    private float transformRatio;

    public bool roomFallFlag = false;
    public bool roomFallStart = false;
    public int fallDist = 0;
    public int fallOldCol;
    public int fallOldRow;

    private void Start()
    {
        //currentRoomCol = Mathf.RoundToInt(transform.position.x / roomManager.roomSize);
        //currentRoomRow = Mathf.RoundToInt(transform.position.y / roomManager.roomSize);
        //roomManager.roomGrid[currentRoomCol, currentRoomRow] = true;
    }

    void Update()
    {
        if (!player.roomShiftFlag)
            if ((currentRoomCol != Mathf.RoundToInt(roomManager.playerPos.x / roomManager.roomSize)) || (currentRoomRow != Mathf.RoundToInt(roomManager.playerPos.y / roomManager.roomSize)))
            {
                //Debug.Log(currentRoomCol + " " + currentRoomRow);
                wallUp.enabled = false;
                wallLeft.enabled = false;
                wallRight.enabled = false;
                wallDown.enabled = false;
                roomCol.enabled = true;
            }
            else
            {
                wallUp.enabled = true;
                wallLeft.enabled = true;
                wallRight.enabled = true;
                wallDown.enabled = true;
                roomCol.enabled = false;
            }
        else
        {
            wallUp.enabled = false;
            wallLeft.enabled = false;
            wallRight.enabled = false;
            wallDown.enabled = false;
            roomCol.enabled = false;
        }



        if (player.roomShiftFlag && currentRoomCol == Mathf.RoundToInt(roomManager.playerPos.x / roomManager.roomSize) && currentRoomRow == Mathf.RoundToInt(roomManager.playerPos.y / roomManager.roomSize))
        {
            if (player.roomShiftStart)
            {
                player.cameraMoveFlag = true;
                player.cameraMoveStart = true;
                player.roomShiftStart = false;
                startPos = transform.position;
                xPos = transform.position.x + (snapMarker.wallSide.x * roomManager.roomSize);
                yPos = transform.position.y + (snapMarker.wallSide.y * roomManager.roomSize);
                cam.targetX = xPos;
                cam.targetY = yPos;
            }

            timePassed += Time.deltaTime;
            transformRatio = ExpMove(timePassed);
            targetPos = new Vector3(xPos, yPos, transform.position.z);
            transform.position = Vector3.Lerp(startPos, targetPos, transformRatio);



            if (transformRatio > 1)
            {
                player.roomShiftFlag = false;
                timePassed = 0f;
                startPos = targetPos;
                roomManager.roomGrid[currentRoomCol, currentRoomRow] = null;
                currentRoomCol = Mathf.RoundToInt(transform.position.x / roomManager.roomSize);
                currentRoomRow = Mathf.RoundToInt(transform.position.y / roomManager.roomSize);
                roomManager.playerPos.x = xPos;
                roomManager.playerPos.y = yPos;
                roomManager.roomGrid[currentRoomCol, currentRoomRow] = this;
            }
        }
    }

    private float ExpMove(float x) //exponential camera
    {
        float y = (1 - Mathf.Exp((-roomSpeed * x) / roomShiftDuration)) / (1 - Mathf.Exp(-roomSpeed));
        return y;
    }

    public void SetCoord(int col, int row)
    {
        currentRoomCol = col;
        currentRoomRow = row;
    }
}
