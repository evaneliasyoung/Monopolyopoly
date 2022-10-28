using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerSys : MonoBehaviour
{
    public GameObject passButton;
    public GameObject rollButton;
    public GameObject gameController;
    public GameObject mainCamera;
    public CameraController cameraControl;
    public List<PlayerObj> pieces;
    public Vector3 offset;
    public Vector3 reverseOffset;
    private int currentPlayer = 0;
    private bool rolled;
    
    //Return which player is currently active
    public int PlayerTurn()
    {
        return currentPlayer;
    }

    //When piece stops moving
    public void Stop()
    {
        passButton.SetActive(true);
        rolled = false;
    }

    //Roll button triggers this function
    public void Roll()
    {
        if (pieces[currentPlayer].isStopped() == true)
        {
            rollButton.SetActive(false);
            rolled = true;
            int die1 = (int)(Random.value * 6f) + 1;
            int die2 = (int)(Random.value * 6f) + 1;
            //Debug.Log("die1: " + die1 + " - die2: " + die2);


            //currentPlayer = (currentPlayer + 1) % pieces.Count;
            //cameraControl.TargetPlayer(currentPlayer);
            //rolled = true;
            pieces[currentPlayer].moveSpaces(die1 + die2);
        }
    }

    //Pass button triggers this function
    public void Pass()
    {
        if (pieces[currentPlayer].isStopped() == true)
        {
            passButton.SetActive(false);
            rollButton.SetActive(true);
            rolled = false;
            currentPlayer = (currentPlayer + 1) % pieces.Count;
            cameraControl.TargetPlayer(currentPlayer);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        passButton.SetActive(false);
        rollButton.SetActive(true);
        gameController = this.gameObject;
        cameraControl = mainCamera.GetComponent<CameraController>();
        pieces = new List<PlayerObj>(gameController.GetComponentsInChildren<PlayerObj>());
        pieces.Sort((x, y) => x.playerNumber.CompareTo(y.playerNumber));
        //mainCamera.TargetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    

}
