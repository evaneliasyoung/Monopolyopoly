using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame
{
    public static int PlayerNum = 0;
    public static float GameSpeed = 0.2f;
    public static bool Quickplay = false;
    public static bool Player1AI = false;
    public static bool Player1Active = true;
    public static bool Player2AI = false;
    public static bool Player2Active = true;
    public static bool Player3AI = false;
    public static bool Player3Active = true;
    public static bool Player4AI = false;
    public static bool Player4Active = true;
    public static float GameVolume = 1.0f;

    public static string[] PlayerState = { "none", "none", "none", "none" };
/*
    public void update()
    {
        if(Player1AI && Player1Active)
        {
            PlayerState[0] = "ai";
        } 
        else if (Player1Active)
        {
            PlayerState[0] = "player";
        }
        else
        {
            PlayerState[0] = "none";
        }

        if(Player2AI && Player2Active)
        {
            PlayerState[1] = "ai";
        } 
        else if (Player2Active)
        {
            PlayerState[1] = "player";
        }
        else
        {
            PlayerState[1] = "none";
        }

        if(Player3AI && Player3Active)
        {
            PlayerState[2] = "ai";
        } 
        else if (Player3Active)
        {
            PlayerState[2] = "player";
        }
        else
        {
            PlayerState[2] = "none";
        }

        if(Player4AI && Player4Active)
        {
            PlayerState[3] = "ai";
        } 
        else if (Player4Active)
        {
            PlayerState[3] = "player";
        }
        else
        {
            PlayerState[3] = "none";
        }
    }
    */
}
