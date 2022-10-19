using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    public GameObject transforms;
    public GameObject piece;
    private List<Transform> positions;

    public float moveTime = 0.3f;
    public float jumpHeight = 1;
    public int targetSpace = 0;
    public int moveSpacesCount = 0;
    public bool directJump = false;
    public bool jumpCounter = false;
    public Vector3 offset;

    public bool jailTime = false;
    public bool noJail = false;

    //dont modify
    public int currentSpace = 0;

    private List<int> ignorePos = new List<int> { 41 };
    private int visitNum = 10;
    private int jailNum = 11;
    private Transform jail;

    private int nextSpace = 0;
    private int spaces;
    //public Transform pos;
    // Start is called before the first frame update

    private Vector3 sp1;
    private Vector3 sp2;
    private Vector2 sp1xz;
    private Vector2 sp2xz;
    private Vector2 moveVector;
    private float parabolaHeight;
    private bool moving = false;
    private bool inJail = false;


    public void goToJail()
    {
        inJail = true;
        jumpCounter = false;
        directJump = true;
        //just make it so the piece has to move
        targetSpace = -1;
        nextSpace = -1;

        sp1 = piece.transform.position;
        sp2 = jail.position + offset;
        sp1xz = new Vector2(sp1.x, sp1.z);
        sp2xz = new Vector2(sp2.x, sp2.z);
        moveVector = sp2xz - sp1xz;
        parabolaHeight = Mathf.Max(sp1.y, sp2.y) + jumpHeight;
        moving = true;
    }

    public void getOutOfJail()
    {
        directJumpTo(visitNum);
    }

    public void directJumpTo(int space)
    {
        //space = space - 1;
        if (space > spaces || space < 0)
            return;

        jumpCounter = false;
        directJump = true;
        targetSpace = space;
    }

    public void moveTo(int space)
    {
        //space = space - 1;
        if (space > spaces || space < 0)
            return;

        jumpCounter = false;
        directJump = false;
        targetSpace = space;
    }

    public void moveSpaces(int num)
    {
        jumpCounter = true;
        moveSpacesCount = num;
    }


    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    void InitializeInterpolation()
    {
        //sp1 = positions[currentSpace].position + offset;
        sp1 = piece.transform.position;
        sp2 = positions[nextSpace].position + offset;
        sp1xz = new Vector2(sp1.x, sp1.z);
        sp2xz = new Vector2(sp2.x, sp2.z);
        moveVector = sp2xz - sp1xz;
        parabolaHeight = Mathf.Max(sp1.y, sp2.y) + jumpHeight;
        moving = true;
        return;
    }

    void InterpolateMovement()
    {
        Vector3 pos = piece.transform.position;
        Vector2 xzMovement = moveVector * Time.deltaTime / moveTime;

        //piece.transform.position = new Vector3(xzMovement.x, pos.y, xzMovement.y);

        Vector2 progress = new Vector2(pos.x, pos.z) - sp1xz;

        //0 to 1 porportion between sp1 and sp2
        float progressNum = 1f - (moveVector.magnitude - progress.magnitude) / moveVector.magnitude;

        //VERTICAL MOVEMENT
        float multiplier = parabolaHeight;

        //two different parabolas that meet in the middle
        if (progressNum < 0.5f)
            multiplier -= sp1.y;
        else
            multiplier -= sp2.y;

        //modify input to shrink horizontally and make the vertex when progressNum = 0.5
        float xmodify = 2f * progressNum - 1f;
        float objectHeight = -(multiplier * xmodify * xmodify) + parabolaHeight;

        Debug.Log("progressNum " + progressNum);
        Debug.Log("multiplier " + multiplier);
        Debug.Log("xmodify " + xmodify);
        Debug.Log("objectHeight " + objectHeight);
        //END VERTICAL MOVEMENT

        piece.transform.position = new Vector3(xzMovement.x + pos.x, objectHeight, xzMovement.y + pos.z);

        //movement is complete
        if (progressNum >= 1f)
        {
            piece.transform.position = sp2;
            currentSpace = nextSpace;

            if (moveSpacesCount > 0)
                moveSpacesCount--;
            else if (moveSpacesCount < 0)
                moveSpacesCount++;

            moving = false;
        }
    }
    
    void Start()
    {
        positions = new List<Transform>(transforms.GetComponentsInChildren<Transform>());
        //remove parent
        positions.RemoveAt(0);

        //remove all spaces in ignorePos
        ignorePos.Sort();
        ignorePos.Reverse();
        foreach (int i in ignorePos)
        {
            positions.RemoveAt(i);
        }

        jail = positions[jailNum];
        positions.RemoveAt(jailNum);

        spaces = positions.Count;

        foreach (Transform i in positions)
        {
            Debug.Log(i.position);
        }

        piece.transform.position = positions[0].position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        //remove this
        if (jailTime)
        {
            goToJail();
            jailTime = false;
        }

        if (noJail)
        {
            getOutOfJail();
            noJail = false;
        }

        //check if piece needs to start moving
        if (!moving)
        {
            //relative movement
            if (jumpCounter)
            {
                if (moveSpacesCount != 0)
                {
                    if (moveSpacesCount > 0)
                        nextSpace = (currentSpace + 1) % spaces;
                    else if (moveSpacesCount < 0)
                        nextSpace = mod((currentSpace - 1), spaces);

                    if (inJail)
                    {
                        nextSpace = visitNum;
                        inJail = false;
                    }

                    InitializeInterpolation();
                }
            }
            //absolute movement
            else
            {
                if (currentSpace != targetSpace)
                {
                    //can be -1 for jail
                    if (targetSpace < -1 || targetSpace > spaces - 1)
                        targetSpace = currentSpace;

                    if (directJump)
                    {
                        nextSpace = targetSpace;
                        inJail = false;
                    }
                    else
                    {
                        if (inJail)
                        {
                            nextSpace = visitNum;
                            inJail = false;
                        }
                        else
                            nextSpace = (currentSpace + 1) % spaces;
                    }
                        
                    InitializeInterpolation();
                }
            }
        }

        if (moving)
        {
            InterpolateMovement();
        }

    }
}