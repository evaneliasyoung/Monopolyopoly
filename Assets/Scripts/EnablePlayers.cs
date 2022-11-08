using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnablePlayers : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;
    [SerializeField] private int index;

    private void Start()
    {
        _toggle.isOn = InitializeGame.ai[index];
    }

    // Start is called before the first frame update
    public void updatePlayer(int pid)
    {
        InitializeGame.ai[pid] = _toggle.isOn;

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
