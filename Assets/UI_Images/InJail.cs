using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InJail : MonoBehaviour
{
    public GameObject target;
    public Image jailSprite;

    /** Don't refresh at 60FPS; wasteful! */
	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;
    private int jailFreeCards;

    private GameControllerSys gameController;
    private ActiveTurn parent;

    void Start()
    {
        parent = this.GetComponent<ActiveTurn>();
        gameController = parent.target.GetComponent<GameControllerSys>();
        target = gameController.GetPlayer(parent.playerNum).gameObject;

        jailSprite.gameObject.SetActive(false);
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;

            target = gameController.GetPlayer(parent.playerNum).gameObject;

            jailFreeCards = target.GetComponent<PlayerObj>().JailFreeCards;
            if(jailFreeCards != 0){
                jailSprite.gameObject.SetActive(true);
            }
            else{
                jailSprite.gameObject.SetActive(false);
            }
        }
    }
}
