using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the whole Monopoly game
/// </summary>
public class GameControllerSys : MonoBehaviour
{
    public float moveTime = 0.5f;
    public bool instantMoves = false;
    public Bank bank;
    public GameObject passButton;
    public GameObject rollButton;
    public GameObject buyButton;
    public GameObject moneyIndicator;
    private TextMeshProUGUI moneyText;
    public GameObject gameController;
    public GameObject mainCamera;
    public CameraController cameraControl;
    public List<PlayerObj> pieces;
    public Vector3 offset;
    public Vector3 reverseOffset;

    public short jailCost = 50;

    private int currentPlayerNum = 0;
    private PlayerObj currentPlayer;
    public PlayerObj CurrentPlayer
    {
        get { return currentPlayer; }
    }

    private byte tempByte;

    private bool doubles = false;
    private int doubleCount = 0;

    /// <summary>
    /// Called by PlayerObj when it stops moving
    /// </summary>
    public void Stop()
    {
        //rolled doubles, roll again
        if (doubles)
        {
            rollButton.SetActive(true);
            doubles = false;
            return;
        }

        //player landed in jail, can only pass to next player.
        if (currentPlayer.InJail)
        {
            passButton.SetActive(true);
            return;
        }

        //tile number player landed on
        byte currentSpace = (byte)currentPlayer.CurrentSpace;

        //tile type player landed on
        TileSpace test = new TileSpace(currentSpace);

        //switch case for the type of tile landed on
        switch (test.TileType)
        {
            //landed on a property
            case TileType.Property:
                Debug.Log("property");

                //property is available. player can buy
                if (bank.OwnerCanPurchaseProperty(currentPlayer, currentSpace))
                {
                    buyButton.SetActive(true);
                }
                //property is unavaiable. player must pay rent
                else if (!bank.GetPropertyByIndex(currentSpace).IsForSale)
                {
                    currentPlayer.PlayerMoney -= (short)(bank.GetPropertyCostByIndex(currentSpace)/10);
                    pieces[(int)bank.GetPropertyOwnerByIndex(currentSpace)].PlayerMoney += (short)(bank.GetPropertyCostByIndex(currentSpace)/10);
                    UpdateMoney();
                }
                    
                
                break;
            case TileType.Chance:
                Debug.Log("chance");
                break;
            case TileType.CommunityChest:
                Debug.Log("community chest");
                break;
            case TileType.Tax:
                Debug.Log("tax");
                //income tax
                if (currentSpace == 4)
                    currentPlayer.PlayerMoney -= 200;
                else if (currentSpace == 38)
                    currentPlayer.PlayerMoney -= 100;
                else
                    Debug.Log("what?!");

                UpdateMoney();
                break;

            //corner is usually nothing
            case TileType.Corner:
                Debug.Log("corner");
                //space 30 is the go to jail space
                if (currentSpace == 30)
                {
                    currentPlayer.goToJail();
                    return;
                }
                break;
        }
        passButton.SetActive(true);
    }

    /// <summary>
    /// called by the Buy button
    /// </summary>
    public void Buy()
    {
        if (currentPlayer.InJail)
        {
            currentPlayer.PlayerMoney -= jailCost;
            UpdateMoney();
            currentPlayer.getOutOfJail();
            currentPlayer.TurnsInJail = 0;
            buyButton.SetActive(false);
            rollButton.SetActive(false);
            passButton.SetActive(true);
            return;
        }

        IPropertyOwner playa = (IPropertyOwner)currentPlayer; 
        bank.PurchaseProperty(ref playa, (byte)currentPlayer.CurrentSpace);

        Debug.Log("playa: " + playa.LiquidAssets);
        Debug.Log("player: " + currentPlayer.LiquidAssets);

        UpdateMoney();
        buyButton.SetActive(false);
    }

    /// <summary>
    /// Called when the player's money changes
    /// </summary>
    public void UpdateMoney()
    {
        moneyText.SetText("$" + currentPlayer.PlayerMoney);
    }

    /// <summary>
    /// called by the Roll button
    /// </summary>
    public void Roll()
    {
        currentPlayer.MoveTime = moveTime;
        rollButton.SetActive(false);
        int die1 = (int)(Random.value * 6f) + 1;
        int die2 = (int)(Random.value * 6f) + 1;

        Debug.Log("die1: " + die1 + " - die2: " + die2);

        if(currentPlayer.InJail == true)
        {
            buyButton.SetActive(false);
            //rolled doubles in jail
            if (die1 == die2)
            {
                //rolled doubles in jail
                currentPlayer.getOutOfJail();
                currentPlayer.TurnsInJail = 0;
                return;
            }
            else
            {
                //player did not roll doubles
                currentPlayer.TurnsInJail++;

                if (currentPlayer.TurnsInJail >= 3)
                {
                    //player spent too long in jail
                    currentPlayer.getOutOfJail();
                    currentPlayer.TurnsInJail = 0;
                    currentPlayer.PlayerMoney -= jailCost;
                    UpdateMoney();

                }
                else
                {
                    //still in jail, next turn
                    passButton.SetActive(true);
                }
            }
            return;
        }


        if(instantMoves)
            currentPlayer.directJumpSpaces(die1 + die2);
        else
            currentPlayer.moveSpaces(die1 + die2);

        //rolled doubles
        if (die1 == die2)
        {
            //speeding
            if (doubleCount >= 2)
            {
                currentPlayer.goToJail();
                doubles = false;
                doubleCount = 0;
            }
            //not yet speeding
            else
            {
                doubles = true;
                doubleCount++;
            }
        }
    }

    /// <summary>
    /// Player is done with their turn, called by Pass button
    /// </summary>
    public void Pass()
    {
        passButton.SetActive(false);
        buyButton.SetActive(false);
        rollButton.SetActive(true);

        currentPlayerNum = (currentPlayerNum + 1) % pieces.Count;
        currentPlayer = pieces[currentPlayerNum];
        doubleCount = 0;
        cameraControl.TargetPlayer(currentPlayer);
        moneyText.SetText("$" + currentPlayer.PlayerMoney);

        if (currentPlayer.InJail)
        {
            buyButton.SetActive(true);
        }
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
        currentPlayer = pieces[currentPlayerNum];
        cameraControl.TargetPlayer(currentPlayer);
        moneyText = moneyIndicator.GetComponent<TextMeshProUGUI>();
        moneyText.SetText("$" + currentPlayer.PlayerMoney);
        bank = gameController.GetComponent<Bank>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    

}
