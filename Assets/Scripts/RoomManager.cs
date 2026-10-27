using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;

public class RoomManager : MonoBehaviour

{
    public int gridSize = 3;
    public float roomSize = 32f;
    public RoomShift[,] roomGrid;
    public Vector2 playerPos;
    public bool roomFallFlag;
    public int fallRoomX;
    public int fallRoomY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        roomGrid = new RoomShift[gridSize, gridSize];

        RoomShift[] allRooms = FindObjectsByType<RoomShift>(FindObjectsSortMode.None);

        foreach (RoomShift room in allRooms)
        {
            int col = Mathf.RoundToInt(room.transform.position.x / roomSize);
            int row = Mathf.RoundToInt(room.transform.position.y / roomSize);

            roomGrid[col, row] = room;
            room.SetCoord(col, row);
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        for (int row = 1; row < gridSize; row++)
            for (int col = 0; col < gridSize; col++)
                if (roomGrid[col,row - 1] == false && roomGrid[col,row] == true)
                {
                    roomFallFlag = true;
                    fallRoomX = col;
                    fallRoomY = row;
                }

        
        

        //Debug.Log(roomGrid[0,0] + " " + roomGrid[1, 0] + " " + roomGrid[2, 0] + " " + roomGrid[0, 1] + " " + roomGrid[1, 1] + " " + roomGrid[2, 1] + " " + roomGrid[0, 2] + " " + roomGrid[1, 2] + " " + roomGrid[2, 2]);
    }

}
