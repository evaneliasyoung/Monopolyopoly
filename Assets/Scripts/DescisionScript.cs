using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescisionScript : MonoBehaviour
{
    [SerializeField] private GameObject passButton;
    [SerializeField] private GameObject rollButton;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject jailFreeButton;
    [SerializeField] private GameObject increaseButton;
    [SerializeField] private GameObject decreaseButton;
    [SerializeField] private GameControllerSys gameController;
    public int jailCost = 50;
    public float aiTimer = 0f;
    public PlayerObj CurrentPlayer { get; set; }
    public List<string> disQueue = new List<string>();
    Bank bank;

    public void PropertyButtonClicked()
    {
        increaseButton.SetActive(false);
        decreaseButton.SetActive(false);
    }

    /// <summary>
    /// When property button is clicked
    /// </summary>
    /// <param name="player">Player Num</param>
    /// <param name="property">Property Num</param>
    public void PropertyClicked(byte property)
    {
        PropertyButtonClicked();

        TextMeshProUGUI decreaseText = decreaseButton.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI increaseText = increaseButton.GetComponentInChildren<TextMeshProUGUI>();
        increaseButton.GetComponent<PropertyButtons>().property = property;
        decreaseButton.GetComponent<PropertyButtons>().property = property;


        Property tempProp = bank.GetPropertyByIndex(property);
        //owner of the property
        if (CurrentPlayer.playerNumber == tempProp.Owner)
        {
            //is street
            if (tempProp.PropertyType == PropertyTileType.Street)
            {
                Street tempStreet = bank.GetStreetByIndex(property);

                //hotel, can only downgrade
                if (tempStreet.Hotels > 0)
                {
                    decreaseText.SetText("Sell Hotel");
                    decreaseButton.SetActive(true);
                }
                //mortgaged, can unmortgage
                else if (tempStreet.IsMortgaged)
                {
                    //needs to have the money
                    if (CurrentPlayer.PlayerMoney >= tempStreet.UnmortgageCost)
                    {
                        increaseText.SetText("Unmortgage");
                        increaseButton.SetActive(true);
                    }
                }
                //default property
                else if (tempStreet.Houses == 0)
                {
                    //needs to have the money
                    if (CurrentPlayer.PlayerMoney >= tempStreet.BuildCost)
                    {
                        //needs to be monopolized
                        if (bank.StreetIsMonopolized(tempStreet))
                        {
                            increaseText.SetText("Buy House");
                            increaseButton.SetActive(true);
                        }
                    }
                    decreaseText.SetText("Mortgage");
                    decreaseButton.SetActive(true);
                }
                //one to four houses
                else if (tempStreet.Houses > 0)
                {
                    decreaseText.SetText("Sell House");
                    decreaseButton.SetActive(true);

                    //needs to have the money
                    if (CurrentPlayer.PlayerMoney >= tempStreet.BuildCost)
                    {
                        if (tempStreet.Houses > 3)
                        {
                            increaseText.SetText("Buy Hotel");
                        }
                        else
                        {
                            increaseText.SetText("Buy House");
                        }
                        increaseButton.SetActive(true);
                    }
                }
            }
            //is not street
            else
            {
                if(tempProp.IsMortgaged)
                {
                    //need to have the money
                    if (CurrentPlayer.PlayerMoney >= tempProp.UnmortgageCost)
                    {
                        increaseText.SetText("Unmortgage");
                        increaseButton.SetActive(true);
                    }
                }
                else
                {
                    decreaseText.SetText("Mortgage");
                    decreaseButton.SetActive(true);
                }
            }
        }
        
    }

    public void QueueDescision(string descision)
    {
        //Debug.Log(descision);

        if (CurrentPlayer.IsAi)
        {
            disQueue.Add(descision);

            switch (descision)
            {
                case "card":
                    aiTimer = 2f;
                    break;
                default:
                    aiTimer = 0.3f;
                    break;
            }
        }
            
        else
            GetDescision(descision);
    }

    public void clearButtons()
    {
        buyButton.SetActive(false);
        nextButton.SetActive(false);
        passButton.SetActive(false);
        rollButton.SetActive(false);
        jailFreeButton.SetActive(false);
        increaseButton.SetActive(false);
        decreaseButton.SetActive(false);
}

    public void GetDescision(string descision)
    {
        if (CurrentPlayer.IsAi)
            Debug.Log("ai: " + descision);

        switch (descision)
        {


            case "new turn":
                if (CurrentPlayer.IsAi)
                    gameController.Roll();
                else
                    rollButton.SetActive(true);
                break;

            case "dice":
                if (CurrentPlayer.IsAi)
                    gameController.Next();
                else
                    nextButton.SetActive(true);
                break;

            case "card":
                if (CurrentPlayer.IsAi)
                    gameController.Next();
                else
                    nextButton.SetActive(true);
                break;

            case "pass":
                if (CurrentPlayer.IsAi)
                    gameController.Pass();
                else
                    passButton.SetActive(true);
                break;

            case "buy":
                if (CurrentPlayer.IsAi)
                    gameController.Buy();
                else
                {
                    buyButton.SetActive(true);
                    passButton.SetActive(true);
                }
                break;

            case "in jail":
                if (CurrentPlayer.IsAi)
                {
                    gameController.Roll();
                }
                else
                {
                    if (CurrentPlayer.PlayerMoney >= jailCost)
                        buyButton.SetActive(true);
                    if (CurrentPlayer.JailFreeCards >= 1)
                        jailFreeButton.SetActive(true);
                    rollButton.SetActive(true);
                }
                break;

        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //nextButton.SetActive(true);
        bank = gameController.bank;
    }

    // Update is called once per frame
    void Update()
    {
        //wait for aiTimer
        if (aiTimer > 0f)
        {
            aiTimer -= Time.deltaTime;
        }
        else
        {
            if (disQueue.Count > 0)
            {
                Debug.Log("update: " + disQueue[0]);
                GetDescision(disQueue[0]);
                disQueue.RemoveAt(0);
            }
        }

    }
}
