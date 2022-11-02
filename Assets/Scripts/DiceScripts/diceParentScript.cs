using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diceParentScript : MonoBehaviour
{
    public GameObject dice1;
    public GameObject dice2;

    public GameControllerSys gameController;

    //public GameObject nextButton;
    //public GameObject rollButton;

    public diceScript diceScript1;
    public diceScript diceScript2;

    bool diceStopped1 = false;
    bool diceStopped2 = false;

    Vector3 angle1;
    Vector3 angle2;


    private int dieVal1;
    public int DieVal1
    {
        get { return dieVal1; }
    }
    private int dieVal2;
    public int DieVal2
    {
        get { return dieVal2; }
    }

    public void DiceStopped(int num, Vector3 angle)
    {
        if (num == 1)
        {
            diceStopped1 = true;
            angle1 = angle;
        }
            
        else if (num == 2)
        {
            diceStopped2 = true;
            angle2 = angle;
        }
            
    }

    private float mod(float x, float m)
    {
        return (x % m + m) % m;
    }

    public void Roll()
    {
        diceScript1.Throw();
        diceScript2.Throw();
        //rollButton.SetActive(false);
    }

    private int getDieValue(float x, float z)
    {
        //setup z and x to be between 0 and 270
        x = mod(x, 360f);
        if (x > 315f)
            x -= 360f;

        z = mod(z, 360f);
        if (z > 315f)
            z -= 360f;

        if (x > 225f)//270, -
            return 1;

        if (x > 135f)
        {
            if (z > 225f)//180, 270
                return 5;

            if (z > 135f)//180, 180
                return 3;

            if (z > 45f)//180, 90
                return 6;

            //180, 0
            return 4;
        }
        
        if (x > 45f)//90 -
            return 2;

        if (z > 225f)//0, 270
            return 6;

        if (z > 135f)//0, 180
            return 4;

        if (z > 45f)//0, 90
            return 5;

        //0, 0
        return 3;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //nextButton.SetActive(false);
        diceScript1 = dice1.GetComponent<diceScript>();
        diceScript2 = dice2.GetComponent<diceScript>();
    }

    // Update is called once per frame
    void Update()
    {

        if (diceStopped1 && diceStopped2)
        {
            //nextButton.SetActive(true);
            diceStopped1 = false;
            diceStopped2 = false;
            dieVal1 = getDieValue(angle1.x, angle1.z);
            dieVal2 = getDieValue(angle2.x, angle2.z);
            gameController.RollDone();
        }
    }
}
