using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescisionScript : MonoBehaviour
{
    public GameObject passButton;
    public GameObject rollButton;
    public GameObject buyButton;
    public GameObject nextButton;
    public GameObject jailFreeButton;
    public GameControllerSys gameController;
    public int jailCost = 50;
    public float aiTimer = 0f;
    public PlayerObj CurrentPlayer { get; set; }
    public List<string> disQueue = new List<string>();

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
        nextButton.SetActive(true);
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
