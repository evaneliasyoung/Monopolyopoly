using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkAI : MonoBehaviour
{
    public GameObject target;
    public Image aiSprite;

    /** Don't refresh at 60FPS; wasteful! */
	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;
    private bool boolAI;

    private GameControllerSys gameController;
    private ActiveTurn parent;

    void Start()
    {
        aiSprite.gameObject.SetActive(false);

        parent = this.GetComponent<ActiveTurn>();
        gameController = parent.target.GetComponent<GameControllerSys>();
        target = gameController.GetPlayer(parent.playerNum).gameObject;

    }
    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;

            target = gameController.GetPlayer(parent.playerNum).gameObject;

            boolAI = target.GetComponent<PlayerObj>().IsAi;
            if(boolAI){
                aiSprite.gameObject.SetActive(true);
            }
            else{
                aiSprite.gameObject.SetActive(false);
            }
        }
    } 
}
