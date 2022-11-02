using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls piece movement and holds player data
/// </summary>
public class PlayerObj : MonoBehaviour, IPropertyOwner
{
    public byte Index;
    public int LiquidAssets;

    public GameControllerSys gameController;
    public GameObject transforms;
    public GameObject piece;
    public string playerName;
    private List<Transform> positions;

    private int targetSpace = 0;
    private int moveSpacesCount = 0;
    private bool directJump = false;
    private bool jumpCounter = false;

    private List<int> ignorePos = new List<int> { 41 };
    private int visitNum = 10;
    private int jailNum = 11;
    private Transform jail;

    private int nextSpace = 0;
    private int spaces;
    //public Transform pos;

    private Vector3 sp1;
    private Vector3 sp2;
    private Vector2 sp1xz;
    private Vector2 sp2xz;
    private Vector2 moveVector;
    private float parabolaHeight;

    //whether the piece is in the middle of translating
    private bool moving = false;

    private int currentSpace = 0;
    public int CurrentSpace
    {
        get { return currentSpace; }
    }
    private bool inJail = false;
    public bool InJail
    {
        get { return inJail; }
    }
    private bool moveComplete = true;
    public bool MoveComplete
    {
        get { return moveComplete; }
    }
    private bool bankrupt = false;
    public bool Bankrupt
    {
        get { return bankrupt; }
    }

    //Public variables to be modified and accessed
    public bool IsAi { get; set; } = false;
    public short PlayerMoney { get; set; } = 1500;
    public int JailFreeCards { get; set; } = 0;
    public int TurnsInJail { get; set; } = 0;

    //Public movement logic variables
    public float MoveTime { get; set; } = 0.3f;
    public float JumpHeight { get; set; } = 1;

    byte IWithIndex.Index { get { return playerNumber; } }
    short ILiquidityProvider.LiquidAssets { get { return PlayerMoney; } set { PlayerMoney = value; } }

    //defined in unity
    public byte playerNumber;
    public Vector3 offset;
    public Vector3 visitingOffset;

    /// <summary>
    /// Use when the player becomes bankrupt
    /// </summary>
    public void SetBankrupt()
    {
        bankrupt = true;
        piece.SetActive(false);
    }


    /// <summary>
    /// Sends piece to jail
    /// </summary>
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
        parabolaHeight = Mathf.Max(sp1.y, sp2.y) + JumpHeight;
        moving = true;
    }

    /// <summary>
    /// Send piece to just visiting
    /// </summary>
    public void getOutOfJail()
    {
        directJumpTo(visitNum);
    }

    /// <summary>
    /// Moves the player directly to "space" in one jump
    /// </summary>
    /// <param name="space">int of desired space</param>
    public void directJumpTo(int space)
    {
        //space = space - 1;
        if (space > spaces || space < 0)
            return;

        jumpCounter = false;
        directJump = true;
        targetSpace = space;
    }

    /// <summary>
    /// Moves the player to "space" and jumps on each space between
    /// </summary>
    /// <param name="space">int of desired space</param>
    public void moveTo(int space)
    {
        //space = space - 1;
        if (space > spaces || space < 0)
            return;

        jumpCounter = false;
        directJump = false;
        targetSpace = space;
    }

    /// <summary>
    /// Moves the player directly "num" spaces ahead
    /// </summary>
    /// <param name="num">int of desired spaces forward</param>
    public void directJumpSpaces(int num)
    {
        targetSpace = mod(currentSpace + num, spaces);

        jumpCounter = false;
        directJump = true;
    }

    /// <summary>
    /// Moves the player a relative amount of spaces
    /// </summary>
    /// <param name="num">int of desired moves</param>
    public void moveSpaces(int num)
    {
        jumpCounter = true;
        moveSpacesCount = num;
    }

    //Modulo function that treats negatives normally
    private int mod(int x, int m)
    {
        return (x % m + m) % m;
    }

    //Sets up variables once to be used for InterpolateMovement()
    private void InitializeInterpolation()
    {
        //current space position
        sp1 = piece.transform.position;
        //next space position

        if (nextSpace == 10)
            sp2 = positions[nextSpace].position + visitingOffset;
        else
            sp2 = positions[nextSpace].position + offset;

        //current space x,z position
        sp1xz = new Vector2(sp1.x, sp1.z);
        //next space x,z position
        sp2xz = new Vector2(sp2.x, sp2.z);

        moveVector = sp2xz - sp1xz;
        parabolaHeight = Mathf.Max(sp1.y, sp2.y) + JumpHeight;
        moving = true;
        return;
    }

    //Interpolates piece position per frame
    private void InterpolateMovement()
    {
        Vector3 pos = piece.transform.position;
        Vector2 xzMovement = moveVector * Time.deltaTime / MoveTime;

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

    //Start is called at the first frame
    void Start()
    {
        piece = this.gameObject;
        gameController = piece.GetComponentInParent<GameControllerSys>();
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

        piece.transform.position = positions[0].position + offset;
    }

    //Update is called once per frame
    void Update()
    {
        //check if piece needs to start moving
        if (!moving)
        {
            //relative movement
            if (jumpCounter && moveSpacesCount != 0)
            {
                moveComplete = false;
                if (moveSpacesCount > 0)
                {
                    nextSpace = (currentSpace + 1) % spaces;
                    if (nextSpace == 0)
                    {
                        PlayerMoney += 200;
                        //gameController.UpdateMoney();
                    }
                        
                }
                else if (moveSpacesCount < 0)
                    nextSpace = mod((currentSpace - 1), spaces);

                if (inJail)
                {
                    nextSpace = visitNum;
                    inJail = false;
                }

                InitializeInterpolation();

            }
            //absolute movement
            else if (!jumpCounter && currentSpace != targetSpace)
            {
                moveComplete = false;
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
                    {
                        nextSpace = (currentSpace + 1) % spaces;
                        if (nextSpace == 0)
                        {
                            PlayerMoney += 200;
                            //gameController.UpdateMoney();
                        }
                    }
                        
                }

                InitializeInterpolation();

            }
        }

        if (moving)
        {
            InterpolateMovement();

            //stopped moving right after
            if (!moving)
            {
                if (jumpCounter && moveSpacesCount == 0)
                {
                    moveComplete = true;
                    gameController.Stop();
                }
                else if (!jumpCounter && currentSpace == targetSpace)
                {
                    moveComplete = true;
                    gameController.Stop();
                }

            }
        }

    }
}
