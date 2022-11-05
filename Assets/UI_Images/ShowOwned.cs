using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOwned : MonoBehaviour
{
    public GameObject target;
    public Sprite propertyColor;
    public Sprite noColor;
    public byte player;
    public byte space;

    private byte? owner;
    private Image image;

	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;
            
            owner = target.GetComponent<Bank>().GetPropertyOwnerByIndex(space);
            // Debug.Log("Player: " + player);
            // Debug.Log("Owner: " + owner);
            if(player == owner){
                image.sprite = propertyColor;
            }
            else {
                image.sprite = noColor;
            }
        }
    }
}
