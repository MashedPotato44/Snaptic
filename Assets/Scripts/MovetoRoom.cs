using System.Data;
using UnityEngine;

public class MovetoRoom : MonoBehaviour
{

    [SerializeField] private PlayerController player;

    private float cameraOffset;
    private Vector3 targetPos; //Camera Destination
    public float cameraMoveDuration = 0.5f; //Time spent moving
    public float cameraSpeed = 3f;
    private Vector3 startPos; //Camera start
    private float timePassed;
    public float cameraMoveTimer;
    private void Start()
    {
        startPos = transform.position;
        cameraOffset = transform.position.x;
    }
    void Update()
    {
        if (player.cameraMoveFlag)
        {
            if (player.cameraMoveStart)
            {
                player.cameraMoveStart = false; 
                cameraMoveTimer = cameraMoveDuration;


                if(cameraMoveTimer > 0)
                {
                    startPos = transform.position;
                }

            }

            cameraMoveTimer -= Time.deltaTime;
            timePassed += Time.deltaTime;
            //float percentageTime = timePassed / cameraMoveDuration; //generating timebased ratio
            //float radPos = (Mathf.PI / 2) * percentageTime; //timebased ratio converted to radians

            //float transformRatio = Mathf.Sin(radPos); //ratio transformed using an exponential

            float transformRatio = ExpMove(timePassed);
            Debug.Log(transformRatio);
            targetPos = new Vector3(player.gridPos.x + cameraOffset, player.gridPos.y, transform.position.z); //target position calculated with camera offset
            transform.position = Vector3.Lerp(startPos, targetPos, transformRatio); //camera position interpolated
            

            
            if (timePassed > cameraMoveDuration) //resets flags once time is up - NEED TO FIND A FIX IF PLAYER MOVES BACK BEFORE TIMER IS FINISHED
            {
                player.cameraMoveFlag = false;
                timePassed = 0;
                startPos = targetPos;
            }
        }
    
    }
    private float ExpMove(float x)
        {
        float y = (1 - Mathf.Exp((-cameraSpeed * x) / cameraMoveDuration)) / (1 - Mathf.Exp(-cameraSpeed));
        return y;
        }
}