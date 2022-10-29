using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Controls the whole Monopoly game
/// </summary>
public class GameControllerSys : MonoBehaviour
{
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

    private int currentPlayerNum = 0;
    private PlayerObj currentPlayer;
    public PlayerObj CurrentPlayer
    {
        get { return currentPlayer; }
    }

    private byte tempByte;

    private bool doubles = false;
    private int doubleCount = 0;
    
    public TileType getTileType(int position)
    {

        return 0;
    }

    public void Stop()
    {
        if (doubles)
        {
            rollButton.SetActive(true);
            doubles = false;
            return;
        }

        if (currentPlayer.InJail)
        {
            passButton.SetActive(true);
            return;
        }

        byte currentSpace = (byte)currentPlayer.CurrentSpace;

        //final tile player has landed on
        TileSpace test = new TileSpace(currentSpace);


        switch (test.TileType)
        {
            case TileType.Property:
                Debug.Log("property");
                Debug.Log(bank.GetPropertyByIndex(currentSpace).GetType());

                if (bank.OwnerCanPurchaseProperty(currentPlayer, currentSpace))
                {
                    buyButton.SetActive(true);
                }
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
                break;
            case TileType.Corner:
                Debug.Log("corner");
                if (currentSpace == 30)
                {
                    CurrentPlayer.goToJail();
                    return;
                }
                break;
        }
        passButton.SetActive(true);
    }

    public void Buy()
    {
        IPropertyOwner playa = (IPropertyOwner)currentPlayer; 
        bank.PurchaseProperty(ref playa, (byte)currentPlayer.CurrentSpace);

        Debug.Log("playa: " + playa.LiquidAssets);
        Debug.Log("player: " + currentPlayer.LiquidAssets);

        UpdateMoney();
        buyButton.SetActive(false);
    }

    public void UpdateMoney()
    {
        moneyText.SetText("$" + currentPlayer.PlayerMoney);
    }    

    //Roll button triggers this function
    public void Roll()
    {
        rollButton.SetActive(false);
        int die1 = (int)(Random.value * 6f) + 1;
        int die2 = (int)(Random.value * 6f) + 1;

        Debug.Log("die1: " + die1 + " - die2: " + die2);

        if(currentPlayer.InJail == true)
        {
            //rolled doubles in jail
            if (die1 == die2)
            {
                //rolled doubles in jail
                currentPlayer.getOutOfJail();
                currentPlayer.TurnsInJail = 0;
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
                    currentPlayer.PlayerMoney -= 50;
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
        

        currentPlayer.moveSpaces(die1 + die2);

        if(die1 == die2)
        {
            if (doubleCount >= 2)
            {
                currentPlayer.goToJail();
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

    /// <summary>
    /// Player is done with their turn
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
