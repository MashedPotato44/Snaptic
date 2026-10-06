using UnityEngine;


[ExecuteAlways]
public class RoomGridSnap : MonoBehaviour
{

    public float roomSize = 28f;
    public int gridSize = 5;
    public int roomX;
    public int roomY;

    void Update()
    {
        if (Application.isPlaying)
        {
            return;
        }

        roomX = Mathf.Clamp(Mathf.RoundToInt(transform.localPosition.x / roomSize), 0, gridSize - 1);
        roomY = Mathf.Clamp(Mathf.RoundToInt(transform.localPosition.y / roomSize), 0, gridSize - 1);
        transform.localPosition = new Vector3(roomX*roomSize, roomY*roomSize, 0f);

    }
}
