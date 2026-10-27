using UnityEngine;

public class RoomFall : MonoBehaviour
{
    [SerializeField] RoomManager roomManager;
    private int currentRoomCol;
    private int currentRoomRow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentRoomCol = Mathf.RoundToInt(transform.position.x / roomManager.roomSize);
        currentRoomRow = Mathf.RoundToInt(transform.position.y / roomManager.roomSize);
    }

    // Update is called once per frame
    void Update()
    {
        if (roomManager.roomFallFlag)
        {
            roomManager.roomFallFlag = false;
            Debug.Log("Let the room " + roomManager.fallRoomX + " " + roomManager.fallRoomY + " fall");
        }
    }
}
