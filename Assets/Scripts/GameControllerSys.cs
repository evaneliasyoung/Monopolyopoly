using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the whole Monopoly game
/// </summary>
public class GameControllerSys : MonoBehaviour
{
    public Bank bank;
    public GameObject passButton;
    public GameObject rollButton;
    public GameObject buyButton;
    public GameObject gameController;
    public GameObject mainCamera;
    public CameraController cameraControl;
    public List<PlayerObj> pieces;
    public Vector3 offset;
    public Vector3 reverseOffset;
    private int currentPlayer = 0;

    private bool doubles = false;
    private int doubleCount = 0;
    
    //Return which player is currently active
    public int PlayerTurn()
    {
        return currentPlayer;
    }

    /// <summary>
    /// Call when piece is done moving
    /// </summary>
    public void Stop()
    {
        if(doubles)
        {
            rollButton.SetActive(true);
            doubles = false;
        }
        else
        {
            passButton.SetActive(true);
            buyButton.SetActive(true);
        }
    }

    //Roll button triggers this function
    public void Roll()
    {
        rollButton.SetActive(false);
        int die1 = (int)(Random.value * 6f) + 1;
        int die2 = (int)(Random.value * 6f) + 1;

        Debug.Log("die1: " + die1 + " - die2: " + die2);


        //currentPlayer = (currentPlayer + 1) % pieces.Count;
        //cameraControl.TargetPlayer(currentPlayer);
        //rolled = true;

        if(pieces[currentPlayer].InJail == true)
        {
            //rolled doubles in jail
            if (die1 == die2)
            {
                //rolled doubles in jail
                pieces[currentPlayer].getOutOfJail();
                pieces[currentPlayer].TurnsInJail = 0;
            }
            else
            {
                //player did not roll doubles
                pieces[currentPlayer].TurnsInJail++;

                if (pieces[currentPlayer].TurnsInJail >= 3)
                {
                    //player spent too long in jail
                    pieces[currentPlayer].getOutOfJail();
                    pieces[currentPlayer].TurnsInJail = 0;
                }
                else
                {
                    //still in jail, next turn
                    passButton.SetActive(true);
                }
            }
            return;
        }
        

        pieces[currentPlayer].moveSpaces(die1 + die2);

        if(die1 == die2)
        {
            if (doubleCount >= 2)
            {
                pieces[currentPlayer].goToJail();
                doubles = false;
                doubleCount = 0;
            }
            else
            {
                doubles = true;
                doubleCount++;
            }
        }
    }

    //Pass button triggers this function
    public void Pass()
    {
        passButton.SetActive(false);
        buyButton.SetActive(false);
        rollButton.SetActive(true);
        currentPlayer = (currentPlayer + 1) % pieces.Count;
        doubleCount = 0;
        cameraControl.TargetPlayer(currentPlayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        passButton.SetActive(false);
        buyButton.SetActive(false);
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
