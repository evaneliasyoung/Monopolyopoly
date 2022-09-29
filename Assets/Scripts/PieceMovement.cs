using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    public GameObject transforms;
    public GameObject piece;
    private List<Transform> positions;

    public float moveTime = 0.4f;
    public float jumpHeight = 3;
    public int targetSpace = 0;
    public int moveSpacesCount = 0;
    public bool directJump = false;
    public bool jumpCounter = false;

    //dont modify
    public int currentSpace = 0;
    
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
    

    public void directJumpTo(int space)
    {
        space = space - 1;
        if (space > spaces)
            return;

        jumpCounter = false;
        directJump = true;
        targetSpace = space;
    }

    public void moveTo(int space)
    {
        space = space - 1;
        if (space > spaces)
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

        spaces = positions.Count;

        foreach (Transform i in positions)
        {
            Debug.Log(i.position);
        }

        piece.transform.position = positions[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (jumpCounter)
        {
            if (moveSpacesCount != 0)
            {
                if (!moving)
                {
                    if (moveSpacesCount > 0)
                        nextSpace = (currentSpace + 1) % spaces;
                    else if (moveSpacesCount < 0)
                        nextSpace = mod((currentSpace - 1), spaces);

                    sp1 = positions[currentSpace].position;
                    sp2 = positions[nextSpace].position;
                    sp1xz = new Vector2(sp1.x, sp1.z);
                    sp2xz = new Vector2(sp2.x, sp2.z);
                    moveVector = sp2xz - sp1xz;
                    parabolaHeight = Mathf.Max(sp1.y, sp2.y) + jumpHeight;
                    moving = true;
                }
                InterpolateMovement();
            }
        }
        else
        {
            if (currentSpace != targetSpace)
            {
                if (targetSpace < 0 || targetSpace > spaces - 1)
                    targetSpace = currentSpace;
                else
                {
                    if (!moving)
                    {
                        if (directJump)
                            nextSpace = targetSpace;
                        else
                            nextSpace = (nextSpace + 1) % spaces;

                        sp1 = positions[currentSpace].position;
                        sp2 = positions[nextSpace].position;
                        sp1xz = new Vector2(sp1.x, sp1.z);
                        sp2xz = new Vector2(sp2.x, sp2.z);
                        moveVector = sp2xz - sp1xz;
                        parabolaHeight = Mathf.Max(sp1.y, sp2.y) + jumpHeight;
                        moving = true;
                    }
                    InterpolateMovement();
                }
            }
        }
    }
}
