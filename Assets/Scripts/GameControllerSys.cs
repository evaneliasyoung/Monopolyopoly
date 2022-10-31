using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the whole Monopoly game
/// </summary>
public class GameControllerSys : MonoBehaviour
{
    //public variables
    public short jailCost = 50;
    public float moveTime = 0.5f;
    public bool instantMoves = false;

    //public objects
    public Bank bank;
    public GameObject moneyIndicator;
    public GameObject gameController;
    public GameObject mainCamera;
    public CameraController cameraControl;
    public List<PlayerObj> pieces;
    public GameObject cards;
    public PlayerObj CurrentPlayer
    {
        get { return currentPlayer; }
    }

    //private objects
    private TextMeshProUGUI moneyText;
    private CardBehaviour cardScript;

    //button objects
    public GameObject passButton;
    public GameObject rollButton;
    public GameObject buyButton;
    public GameObject nextButton;
    

    //private variables
    private int currentPlayerNum = 0;
    private PlayerObj currentPlayer;
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
                    //UpdateMoney();
                }
                    
                
                break;

            case TileType.Chance:
                Debug.Log("chance");
                cameraControl.FocusCard();
                cardScript.DrawAndShowCard("chance");
                passButton.SetActive(false);
                nextButton.SetActive(true);
                return;

            case TileType.CommunityChest:
                Debug.Log("community chest");
                cameraControl.FocusCard();
                cardScript.DrawAndShowCard("community");
                passButton.SetActive(false);
                nextButton.SetActive(true);
                return;

            case TileType.Tax:
                Debug.Log("tax");
                //income tax
                if (currentSpace == 4)
                    currentPlayer.PlayerMoney -= 200;
                else if (currentSpace == 38)
                    currentPlayer.PlayerMoney -= 100;
                else
                    Debug.Log("what?!");

                //UpdateMoney();
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
    /// Called by the Buy button. Buys property
    /// </summary>
    public void Buy()
    {
        if (currentPlayer.InJail)
        {
            currentPlayer.PlayerMoney -= jailCost;
            //UpdateMoney();
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

        //UpdateMoney();
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
    /// Called by the Roll button. Rolls dice and moves piece
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
                    //UpdateMoney();

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
        {
            if (currentPlayer.CurrentSpace + die1 + die2 > 39)
                currentPlayer.PlayerMoney += 200;
            currentPlayer.directJumpSpaces(die1 + die2);
        }  
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
    /// Called by next button. Used when done looking at a card
    /// </summary>
    public void Next()
    {
        bool unmoved = false;
        cameraControl.FocusPlayer();
        nextButton.SetActive(false);

        string pulledCard = cardScript.mostRecentCardDrawnName;

        Debug.Log(pulledCard);

        switch (pulledCard)
        {
            case "chance1":
                currentPlayer.PlayerMoney += 150;
                unmoved = true;
                break;

            case "chance2": //go to "go"
                if (instantMoves)
                {
                    currentPlayer.PlayerMoney += 200;
                    currentPlayer.directJumpTo(0);
                }
                else
                    currentPlayer.moveTo(0);
                break;

            case "chance3": //go to green door brewery
                if (instantMoves)
                    currentPlayer.directJumpTo(39);
                else
                    currentPlayer.moveTo(39);
                break;

            case "chance4": //go to carmellos pizza
                if (instantMoves)
                {
                    if (currentPlayer.CurrentSpace > 1)
                        currentPlayer.PlayerMoney += 200;
                    currentPlayer.directJumpTo(1);
                }
                else
                    currentPlayer.moveTo(1);
                break;

            case "chance5": //go to nearest laundromat

                if (currentPlayer.CurrentSpace > 28)
                {
                    if (instantMoves)
                    {
                        currentPlayer.PlayerMoney += 200;
                        currentPlayer.directJumpTo(12);
                    }
                    else
                        currentPlayer.moveTo(12);
                }
                else if (currentPlayer.CurrentSpace < 12)
                {
                    if (instantMoves)
                        currentPlayer.directJumpTo(12);
                    else
                        currentPlayer.moveTo(12);
                }
                else if (currentPlayer.CurrentSpace < 28)
                {
                    if (instantMoves)
                        currentPlayer.directJumpTo(28);
                    else
                        currentPlayer.moveTo(28);
                }
                break;

            case "chance6": //go back 3 spaces
                if (instantMoves)
                    currentPlayer.directJumpSpaces(-3);
                else
                    currentPlayer.moveSpaces(-3);
                break;


            case "community1":
                currentPlayer.JailFreeCards += 1;
                unmoved = true;
                break;

            case "community2":
                currentPlayer.PlayerMoney -= 100;
                unmoved = true;
                break;

            case "community3":
                currentPlayer.PlayerMoney += 200;
                unmoved = true;
                break;

            case "community4":
                foreach (PlayerObj player in pieces)
                {
                    player.PlayerMoney -= 10;
                    currentPlayer.PlayerMoney += 10;
                }
                unmoved = true;
                break;

            case "community5":
                currentPlayer.PlayerMoney -= 50;
                unmoved = true;
                break;

            case "community6":
                currentPlayer.PlayerMoney += 50;
                unmoved = true;
                break;
        }

        cardScript.StopLookingAtCard();

        if (unmoved)
        {
            passButton.SetActive(true);
        }

        //passButton.SetActive(true);
        
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
        //set objects
        gameController = this.gameObject;
        cameraControl = mainCamera.GetComponent<CameraController>();
        pieces = new List<PlayerObj>(gameController.GetComponentsInChildren<PlayerObj>());
        moneyText = moneyIndicator.GetComponent<TextMeshProUGUI>();
        bank = gameController.GetComponent<Bank>();
        cardScript = cards.GetComponent<CardBehaviour>();

        //use objects
        pieces.Sort((x, y) => x.playerNumber.CompareTo(y.playerNumber));
        currentPlayer = pieces[currentPlayerNum];
        cameraControl.TargetPlayer(currentPlayer);
        moneyText.SetText("$" + currentPlayer.PlayerMoney);

        //set buttons
        passButton.SetActive(false);
        buyButton.SetActive(false);
        rollButton.SetActive(true);
        nextButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoney();
    }

    

}
