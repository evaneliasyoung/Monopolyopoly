using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncludePlayer : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    // Start is called before the first frame update
    public void updatePlayer(int pid)
    {
        switch(pid)
        {
            case 1:
                InitializeGame.Player1AI = _toggle;
                break;
            case 2:
                InitializeGame.Player2AI = _toggle;
                break;
            case 3:
                InitializeGame.Player3AI = _toggle;
                break;
            case 4:
                InitializeGame.Player4AI = _toggle;
                break;
            default:
                break;
        }
    }
}
