using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTurn : MonoBehaviour
{
    public GameObject target;
	public string variableName;
    public int playerNum;

	/** Don't refresh at 60FPS; wasteful! */
	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;
    private int playerTurn;
    private bool active = false;

    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;

            playerTurn = target.GetComponent<GameControllerSys>().activeTurn();

            if (playerNum == playerTurn && active == false){
                transform.position += new Vector3(0, 100, 0);
                active = true;
            }

            if (playerTurn != playerNum && active == true){
                transform.position += new Vector3(0, -100, 0);
                active = false;
            }

        }
        
    }
}
