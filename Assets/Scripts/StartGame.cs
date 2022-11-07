using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void BtnNewScene(int sceneID)
    {
        /*
        if(InitializeGame.Player1AI && InitializeGame.Player1Active)
        {
            InitializeGame.PlayerState[0] = "ai";
        } 
        else if (InitializeGame.Player1Active)
        {
            InitializeGame.PlayerState[0] = "player";
        }
        else
        {
            InitializeGame.PlayerState[0] = "none";
        }

        if(InitializeGame.Player2AI && InitializeGame.Player2Active)
        {
            InitializeGame.PlayerState[1] = "ai";
        } 
        else if (InitializeGame.Player2Active)
        {
            InitializeGame.PlayerState[1] = "player";
        }
        else
        {
            InitializeGame.PlayerState[1] = "none";
        }

        if(InitializeGame.Player3AI && InitializeGame.Player3Active)
        {
            InitializeGame.PlayerState[2] = "ai";
        } 
        else if (InitializeGame.Player3Active)
        {
            InitializeGame.PlayerState[2] = "player";
        }
        else
        {
            InitializeGame.PlayerState[2] = "none";
        }

        if(InitializeGame.Player4AI && InitializeGame.Player4Active)
        {
            InitializeGame.PlayerState[3] = "ai";
        } 
        else if (InitializeGame.Player4Active)
        {
            InitializeGame.PlayerState[3] = "player";
        }
        else
        {
            InitializeGame.PlayerState[3] = "none";
        }
        */



        SceneManager.LoadScene(sceneID);
    }

}
