using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSys : MonoBehaviour
{
    public GameObject moveSys;
    public List<PieceMovement> pieces;
    private int currentPlayer = 0;

    public void Roll()
    {
        if (pieces[currentPlayer].isStopped() == true)
        {
            currentPlayer = (currentPlayer + 1) % pieces.Count;
            pieces[currentPlayer].moveSpaces(40);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        moveSys = this.gameObject;
        pieces = new List<PieceMovement>(moveSys.GetComponentsInChildren<PieceMovement>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
