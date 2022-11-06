using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame
{
    public static int PlayerNum = 0;
    public static float GameSpeed = 0.2f;
    public static bool Quickplay = false;
    public static bool Player1AI = false;
    public static bool Player2AI = false;
    public static bool Player3AI = false;
    public static bool Player4AI = false;
    public static float GameVolume = 1.0f;

    public static string[] PlayerState = { "none", "ai", "player", "none" }; 
}
