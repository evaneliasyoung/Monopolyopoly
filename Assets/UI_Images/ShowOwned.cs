using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowOwned : MonoBehaviour
{
    public Sprite propertyColor;
    public Sprite noColor;
    
    public byte space;

    //automatically set
    private GameObject target;
    private byte player;
    private ActiveTurn parent;
    private Property prop;


    private Image image;
    private Color[] colorCodes = {  new Color(0.7098039f, 0.509804f, 0.4078432f),   //0 brown
                                    new Color(0.595033f, 0.943f, 0.943f),           //1 light blue
                                    new Color(0.9333334f, 0.3960785f, 0.764706f),   //2 magenta
                                    new Color(1f, 0.7176471f, 0.2941177f),          //3 orange
                                    new Color(0.9843138f, 0.2705882f, 0.3137255f),  //4 red
                                    new Color(0.908f, 0.908f, 0.166164f),           //5 yellow
                                    new Color(0.2666667f, 0.8274511f, 0.5254902f),  //6 green
                                    new Color(0.1607843f, 0.5921569f, 0.9019608f),  //7 dark blue
                                    new Color(0.672098f, 0.787f, 0.7755098f),       //8 laundromat gray
                                    new Color(0.5901471f, 0.330504f, 0.879f),       //9 speakeasy purple
                                    new Color(0.2196079f, 0.2196079f, 0.2196079f)}; //10 unowned gray

    private Color owned;
    private Color mortgaged;
    private Color unowned;

	private float updateNSeconds = 0.25f;
	private float lastUpdateTime = 0f;

    public void OnClick()
    {
        target.GetComponent<GameControllerSys>().ClickedProperty(space);
    }


    void Start()
    {
        //get activeTurn script from Player1,2,3,4
        parent = this.GetComponentInParent<Transform>().gameObject.GetComponentInParent<ActiveTurn>();
        player = (byte)parent.Index;
        target = parent.target;

        image = GetComponent<Image>();
        if (space % 5 == 0)//speakeasy
            owned = colorCodes[9];
        else if (space == 12 || space == 28)//laundromat
            owned = colorCodes[8];
        else //street
        {
            int tempSpace = (int)space / 5;
            owned = colorCodes[tempSpace];
            //Debug.Log("color: " + tempSpace);
        }

        //Make the mortgaged property more gray
        float H, S, V;
        Color.RGBToHSV(owned, out H, out S, out V);
        S *= 0.5f;
        V *= 0.6f;
        mortgaged = Color.HSVToRGB(H, S, V);

        Color.RGBToHSV(owned, out H, out S, out V);
        S *= 0.75f;
        V *= 0.25f;
        unowned = Color.HSVToRGB(H, S, V);

        image.sprite = propertyColor;
    }

    void Update()
    {
        lastUpdateTime += Time.deltaTime;
		if (lastUpdateTime > updateNSeconds) {
			lastUpdateTime = 0;

            //if player has changed
            player = (byte)parent.Index;

            //owner = target.GetComponent<Bank>().GetPropertyOwnerByIndex(space);
            prop = target.GetComponent<Bank>().GetPropertyByIndex(space);
            if(player == prop.Owner){
                if (prop.IsMortgaged)
                {
                    //full color
                    image.color = mortgaged;
                }
                else
                {
                    //grayed out
                    image.color = owned;
                }
            }
            else {
                //unowned
                //image.color = unowned;
                image.color = colorCodes[10];
            }
        }
    }
}
