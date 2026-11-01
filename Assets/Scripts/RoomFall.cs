using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class RoomFall : MonoBehaviour
{
    [SerializeField] RoomManager roomManager;
    [SerializeField] RoomShift roomInfo;

    private Vector3 landingPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (roomInfo.roomFallFlag)
            if (roomInfo.roomFallStart)
            {
                roomInfo.roomFallStart = false;
                roomManager.roomGrid[roomInfo.currentRoomCol, roomInfo.currentRoomRow] = roomInfo;
                roomManager.roomGrid[roomInfo.fallOldCol, roomInfo.fallOldRow] = null;
            }

        if (roomInfo.roomFallFlag)
        {
            roomInfo.roomFallFlag = false;
            Debug.Log("Room landing in " + roomInfo.currentRoomCol + " " + roomInfo.currentRoomRow + " by falling " + roomInfo.fallDist);
        }

    }


}
