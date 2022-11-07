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
        InitializeGame.active[pid] = _toggle.isOn;

        Debug.Log(pid + " " + _toggle.isOn);

        bool active = InitializeGame.active[pid];
        bool ai = InitializeGame.ai[pid];

        if (!active)
        {
            InitializeGame.PlayerState[pid] = "none";
        }
        else if (active && ai)//should be ai
        {
            InitializeGame.PlayerState[pid] = "ai";
        }
        else if (active && !ai)
        {
            InitializeGame.PlayerState[pid] = "player";
        }

    }
}
