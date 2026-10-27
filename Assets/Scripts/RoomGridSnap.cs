using UnityEngine;


[ExecuteAlways]
public class RoomGridSnap : MonoBehaviour
{
    [SerializeField] private RoomManager roomManager;

    public int roomX;
    public int roomY;


    void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        roomX = Mathf.Clamp(Mathf.RoundToInt(transform.localPosition.x / roomManager.roomSize), 0, roomManager.gridSize - 1);
        roomY = Mathf.Clamp(Mathf.RoundToInt(transform.localPosition.y / roomManager.roomSize), 0, roomManager.gridSize - 1);
        transform.localPosition = new Vector3(roomX*roomManager.roomSize, roomY*roomManager.roomSize, 0f);

    }
}
