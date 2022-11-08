using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBankrupt : MonoBehaviour
{
    public GameObject target;
    public Image bankruptSprite;

    /** Don't refresh at 60FPS; wasteful! */
	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;
    private bool boolAI;

    private GameControllerSys gameController;
    private ActiveTurn parent;

    void Start()
    {
        parent = this.GetComponent<ActiveTurn>();
        gameController = parent.target.GetComponent<GameControllerSys>();
        target = gameController.GetPlayer(parent.Index).gameObject;

        bankruptSprite.gameObject.SetActive(false);
    }
    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;

            target = gameController.GetPlayer(parent.Index).gameObject;

            boolAI = target.GetComponent<PlayerObj>().Bankrupt;
            if(boolAI){
                bankruptSprite.gameObject.SetActive(true);
            }
            else{
                bankruptSprite.gameObject.SetActive(false);
            }
        }
    } 
}
