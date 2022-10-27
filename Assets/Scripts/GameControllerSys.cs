using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerSys : MonoBehaviour
{
    public GameObject moveSys;
    public GameObject mainCamera;
    public List<PlayerObj> pieces;
    public Vector3 offset;
    private int currentPlayer = 0;


    public void Roll()
    {
        if (pieces[currentPlayer].isStopped() == true)
        {
            int die1 = (int)(Random.value * 6f) + 1;
            int die2 = (int)(Random.value * 6f) + 1;
            //Debug.Log("die1: " + die1 + " - die2: " + die2);


            currentPlayer = (currentPlayer + 1) % pieces.Count;
            pieces[currentPlayer].moveSpaces(die1 + die2);
        }
    }

    public int activeTurn()
    {
        return currentPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSys = this.gameObject;
        pieces = new List<PlayerObj>(moveSys.GetComponentsInChildren<PlayerObj>());
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 piecePos = pieces[currentPlayer].transform.position;
        Vector3 pieceXZ = new Vector3(piecePos.x, 0, piecePos.z);
        mainCamera.transform.position = pieceXZ + offset;
    }

    

}
