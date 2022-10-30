using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOwned : MonoBehaviour
{
    public GameObject target;
    public Image panel;
    public int player;

    private byte? owner;

	/** Don't refresh at 60FPS; wasteful! */
	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;

    void Start()
    {
        panel = GetComponent<Image>();
        panel.color = Color.white;
    }

    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;
            owner = target.GetComponent<Bank>().GetPropertyOwnerByIndex(6);
            if(owner.ToString() == player.ToString()){
                panel.color = Color.black;
            }

            Debug.Log(player);

        }
    }
}
