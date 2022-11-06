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
    public DescisionScript descision;
    public GameObject moneyIndicator;
    public GameObject gameController;
    public GameObject mainCamera;
    public CameraController cameraControl;
    public List<PlayerObj> pieces;
    public List<GameObject> playerPortraits;
    public GameObject cards;
    public DiceParentScript diceScript;
    public PlayerObj CurrentPlayer
    {
        get { return currentPlayer; }
    }

    //private objects
    private TextMeshProUGUI moneyText;
    private CardBehaviour cardScript;
    
    //private variables
    private int currentPlayerNum = 0;
    private PlayerObj currentPlayer;
    private bool doubles = false;
    private int doubleCount = 0;

    private string state = "start";

    
    //public List<string> disQueue = new List<string>();

    /// <summary>
    /// Get PlayerObj by index
    /// </summary>
    /// <param name="playerNum">Index of player</param>
    /// <returns></returns>
    public PlayerObj GetPlayer(int playerNum)
    {
        foreach (PlayerObj player in pieces)
        {
            if (player.playerNumber == playerNum)
            {
                return player;
            }
        }
        return null;
    }


    /// <summary>
    /// Called when PlayerObj lands on a space
    /// </summary>
    public void Stop()
    {
        descision.clearButtons();

        //player landed in jail, can only pass to next player.
        if (currentPlayer.InJail)
        {
            //passButton.SetActive(true);
            descision.QueueDescision("pass");
            return;
        }

        //rolled doubles, roll again
        if (doubles)
        {
            //rollButton.SetActive(true);
            descision.QueueDescision("new turn");
            doubles = false;
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
                //Debug.Log("property");

                //property is available. player can buy
                if (bank.OwnerCanPurchaseProperty(currentPlayer, currentSpace))
                {
                    //player has the money
                    if (currentPlayer.PlayerMoney >= bank.GetPropertyCostByIndex(currentSpace))
                    {
                        descision.QueueDescision("buy");
                        return;
                    }
                }
                //player does not own the property
                else if (bank.GetPropertyOwnerByIndex(currentSpace) != currentPlayerNum)
                {
                    Debug.Log("other prop");
                    short cost = (short)bank.GetTileRentByIndex(currentSpace, currentPlayer);

                    currentPlayer.PlayerMoney -= cost;
                    int owner = (int)bank.GetPropertyOwnerByIndex(currentSpace);
                    currentPlayer.lastPayed = owner;
                    pieces[owner].PlayerMoney += cost;
                }
                break;

            case TileType.Chance:
                cameraControl.FocusCard();
                cardScript.DrawAndShowCard("chance");

                string pulledCardChance = cardScript.mostRecentCardDrawnName;
                SoundManager.Instance.CommunitySound(pulledCardChance);

                descision.QueueDescision("card");
                state = "card";
                return;

            case TileType.CommunityChest:
                cameraControl.FocusCard();
                cardScript.DrawAndShowCard("community");

                string pulledCardCommunity = cardScript.mostRecentCardDrawnName;
                SoundManager.Instance.CommunitySound(pulledCardCommunity);

                descision.QueueDescision("card");
                state = "card";
                return;

            case TileType.Tax:
                currentPlayer.PlayerMoney -= (short)bank.GetTileRentByIndex(currentSpace, currentPlayer);
                currentPlayer.lastPayed = -1;
                break;

            //corner is usually nothing
            case TileType.Corner:
                //space 30 is the go to jail space
                if (currentSpace == 30)
                {
                    SoundManager.Instance.MiscSound("jail");
                    currentPlayer.goToJail();
                    return;
                }
                break;
        }
        descision.QueueDescision("pass");
        //passButton.SetActive(true);
    }

    /// <summary>
    /// Called by the Buy button. Buys property
    /// </summary>
    public void Buy()
    {
        SoundManager.Instance.MiscSound("buy");

        descision.clearButtons();
        if (currentPlayer.InJail)
        {
            currentPlayer.PlayerMoney -= jailCost;
            currentPlayer.lastPayed = -1;
            currentPlayer.getOutOfJail();
            currentPlayer.TurnsInJail = 0;
            descision.QueueDescision("pass");
            return;
        }

        byte currentSpace = (byte)currentPlayer.CurrentSpace;

        IPropertyOwner playa = (IPropertyOwner)currentPlayer;

        //player owns space, buy residence
        if (bank.GetPropertyOwnerByIndex(currentSpace) == currentPlayerNum)
        {
            bank.BuildResidence(ref playa, currentSpace);
            return;
        }

        bank.PurchaseProperty(ref playa, currentSpace);

        descision.QueueDescision("pass");
    }

    /// <summary>
    /// Called when the player's money changes
    /// </summary>
    public void UpdateMoney()
    {
        moneyText.SetText("$" + currentPlayer.PlayerMoney);
    }

    /// <summary>
    /// called by roll button
    /// </summary>
    public void Roll()
    {
        SoundManager.Instance.MiscSound("roll");
        descision.clearButtons();

        if (instantMoves)
        {
            int die1 = (int)(Random.value * 6f) + 1;
            int die2 = (int)(Random.value * 6f) + 1;
            MovePiece(die1, die2);
            return;
        }

        state = "dice";
        cameraControl.FocusDice();
        diceScript.Roll();
    }

    /// <summary>
    /// Player uses a Get Out of Jail Free card
    /// </summary>
    public void GetOutFree()
    {
        descision.clearButtons();

        currentPlayer.JailFreeCards -= 1;
        currentPlayer.getOutOfJail();
        currentPlayer.TurnsInJail = 0;
        descision.QueueDescision("pass");

    }

    /// <summary>
    /// When property button is clicked
    /// </summary>
    /// <param name="player">Player Num</param>
    /// <param name="property">Property Num</param>
    public void ClickedProperty(byte player, byte property)
    {
        Debug.Log("clicked player " + player + " property " + property);
    }

    /// <summary>
    /// Called when roll is complete. Rolls dice and moves piece
    /// </summary>
    public void MovePiece(int die1, int die2)
    {
        cameraControl.FocusPlayer();
        currentPlayer.MoveTime = moveTime;

        //Debug.Log("die1: " + die1 + " - die2: " + die2);

        if(currentPlayer.InJail == true)
        {
            //get out of jail is a movement and will trigger Stop()
            descision.clearButtons();
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
                    currentPlayer.lastPayed = -1;
                }
                else
                {
                    //still in jail, next turn
                    descision.QueueDescision("pass");
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
                SoundManager.Instance.MiscSound("jail");
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
    /// called by diceParentScript when dice are done moving
    /// </summary>
    /// <param name="die1">value of 1st die</param>
    /// <param name="die2">value of 2nd die</param>
    public void RollDone()
    {
        state = "dice";
        descision.QueueDescision("dice");
    }

    /// <summary>
    /// Called by next button
    /// </summary>
    public void Next()
    {
        SoundManager.Instance.MiscSound("next");
        //nextButton.SetActive(false);
        descision.clearButtons();
        switch (state)
        {
            case "card":
                CardEffect();
                break;
            case "dice":
                MovePiece(diceScript.DieVal1, diceScript.DieVal2);
                break;
            case "start":
                descision.QueueDescision("new turn");
                break;
        }
    }

    /// <summary>
    /// Called by next after card is found. Used when done looking at a card
    /// </summary>
    public void CardEffect()
    {
        bool unmoved = false;
        cameraControl.FocusPlayer();

        string pulledCard = cardScript.mostRecentCardDrawnName;

        //Debug.Log(pulledCard);

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
                currentPlayer.lastPayed = -1;
                unmoved = true;
                break;

            case "community3":
                currentPlayer.PlayerMoney += 200;
                unmoved = true;
                break;

            case "community4":
                foreach (PlayerObj player in pieces)
                {
                    if (!player.Bankrupt)
                    {
                        player.PlayerMoney -= 10;
                        player.lastPayed = currentPlayer.playerNumber;
                        currentPlayer.PlayerMoney += 10;
                    }
                }
                unmoved = true;
                break;

            case "community5":
                currentPlayer.PlayerMoney -= 50;
                currentPlayer.lastPayed = -1;
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
            descision.QueueDescision("pass");
        }
        
    }

    private void Bankrupt(PlayerObj currentPlayer)
    {
        byte? owner;
        for (byte i = 0; i < 40; i++)
        {
            owner = bank.GetPropertyOwnerByIndex(i);
            if (owner == currentPlayer.playerNumber)
            {
                if (currentPlayer.lastPayed == -1)
                {
                    bank.Properties[i].Owner = null;
                }
                else
                {
                    bank.Properties[i].Owner = (byte)currentPlayer.lastPayed;
                }
            }
        }
        currentPlayer.SetBankrupt();
    }

    /// <summary>
    /// Player is done with their turn, called by Pass button
    /// </summary>
    public void Pass()
    {
        SoundManager.Instance.MiscSound("pass");
        descision.clearButtons();

        if (currentPlayer.PlayerMoney < 0)
            Bankrupt(currentPlayer);

        int activePlayers = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            if (!pieces[i].Bankrupt)
                activePlayers++;
        }

        if (activePlayers <= 1)
        {
            descision.clearButtons();
            Debug.Log("GAME OVER");
            return;
        }

        for (int i = 0; i < pieces.Count; i++)
        {
            currentPlayerNum = (currentPlayerNum + 1) % pieces.Count;
            currentPlayer = pieces[currentPlayerNum];
            if(!currentPlayer.Bankrupt)
            {
                if(currentPlayer.PlayerMoney < 0)
                {
                    Bankrupt(currentPlayer);
                }
                else
                {
                    break;
                }
            }
        }

        doubleCount = 0;
        cameraControl.TargetPlayer(currentPlayer);

        descision.CurrentPlayer = currentPlayer;

        if (currentPlayer.InJail)
        {
            descision.QueueDescision("in jail");
            return;
        }

        descision.QueueDescision("new turn");
    }

    public int activeTurn()
    {
        return currentPlayerNum;
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
        int uiCount = 0;
        currentPlayer = null;
        for (int i = 0; i < pieces.Count; i++)
        {
            string playerState = InitializeGame.PlayerState[i];
            if (playerState == "none")
            {
                pieces[i].SetBankrupt();
            }
            else if (playerState == "player")
            {
                if (currentPlayer == null)
                    currentPlayer = pieces[i];
                pieces[i].IsAi = false;
                playerPortraits[uiCount].GetComponent<ActiveTurn>().playerNum = i;
                uiCount++;
            }
            else if (playerState == "ai")
            {
                if (currentPlayer == null)
                    currentPlayer = pieces[i];
                pieces[i].IsAi = true;
                playerPortraits[uiCount].GetComponent<ActiveTurn>().playerNum = i;
                uiCount++;
            }
        }

        for (int i = uiCount; i < pieces.Count; i++)
        {
            playerPortraits[i].SetActive(false);
        }

        currentPlayerNum = currentPlayer.playerNumber;
        cameraControl.TargetPlayer(currentPlayer);
        moneyText.SetText("$" + currentPlayer.PlayerMoney);

<<<<<<< HEAD
=======


        pieces[0].IsAi = InitializeGame.Player1AI;
        pieces[1].IsAi = InitializeGame.Player2AI;
        pieces[2].IsAi = InitializeGame.Player3AI;
        pieces[3].IsAi = InitializeGame.Player4AI;

>>>>>>> 46108f04ca67597c7397d8a09f0240379cf8f99d
        moveTime = InitializeGame.GameSpeed;
        instantMoves = InitializeGame.Quickplay;

        descision.CurrentPlayer = currentPlayer;
        //set buttons
        descision.clearButtons();
        descision.QueueDescision("new turn");
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateMoney();
    }

    

}
