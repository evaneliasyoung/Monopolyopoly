using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public List<PlayerObj> pieces;
    public Vector3 offset;
    public Vector3 reverseOffset;
    public Vector3 angle;
    public Vector3 reverseAngle;

    private int currentPlayer = 0;
    public GameObject mainCamera;
    public GameObject gameController;

    /*
    public float timeframe;
    public float height;
    public float radius;
    public float yAngle;
    public Vector3 origin;

    public float targetAngle;
    public bool rotating;
    public float timeElapsed;
    


    public void rotateTo(float angle, float time)
    {
        timeframe = time;
        timeElapsed = 0f;
        targetAngle = angle;
        rotating = true;

        if (Mathf.Abs(yAngle - targetAngle) > 180)
        {
            if (targetAngle > 180f)
                yAngle += 360f;
            else
                yAngle -= 360f;
        }

    }
    */

    public void TargetPlayer(int player)
    {
        currentPlayer = player;
    }

    public void SetPlayers(List<PlayerObj> players)
    {
        pieces = players;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = this.gameObject;
        reverseOffset = new Vector3(-offset.x, offset.y, -offset.z);
        reverseAngle = new Vector3(angle.x, angle.y + 180f, angle.z);

        pieces = new List<PlayerObj>(gameController.GetComponentsInChildren<PlayerObj>());
        pieces.Sort((x, y) => x.playerNumber.CompareTo(y.playerNumber));
    }

    

    // Update is called once per frame
    void Update()
    {
        /*
        if(yAngle != targetAngle)
        {
            timeElapsed += Time.deltaTime;
            float progress = timeElapsed / timeframe;

            if (progress >= 1)
            {
                yAngle = targetAngle;
                
            }

            float smoothProgress = -2 * Mathf.Pow(progress, 3) + 3 * Mathf.Pow(progress, 2);
            

        }
        */

        

        Vector3 piecePos = pieces[currentPlayer].transform.position;
        Vector3 pieceXZ = new Vector3(piecePos.x, 0, piecePos.z);
        if (pieces[currentPlayer].CurrentSpace >= 11 && pieces[currentPlayer].CurrentSpace <= 30)
        {
            
            mainCamera.transform.position = pieceXZ + reverseOffset;
            mainCamera.transform.eulerAngles = reverseAngle;
        }
        else
        {
            mainCamera.transform.position = pieceXZ + offset;
            mainCamera.transform.eulerAngles = angle;
        }
    }
}
