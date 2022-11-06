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

    private Vector3 pos1;
    private Vector3 pos2;
    private float parabolaHeight;
    private float timePassed;

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

        timePassed = 0f;
        pos1 = piece.transform.position;
        pos2 = jail.position + offset;
        parabolaHeight = Mathf.Max(pos1.y, pos2.y) + JumpHeight;
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
        pos1 = piece.transform.position;
        //next space position

        if (nextSpace == 10)
            pos2 = positions[nextSpace].position + visitingOffset;
        else
            pos2 = positions[nextSpace].position + offset;

        parabolaHeight = Mathf.Max(pos1.y, pos2.y) + JumpHeight;
        moving = true;
        return;
    }

    //Interpolates piece position per frame
    private void InterpolateMovement()
    {

        timePassed += Time.deltaTime;
        float progressNum = timePassed / MoveTime;

        if (progressNum >= 1f)
        {
            timePassed = 0f;
            piece.transform.position = pos2;
            currentSpace = nextSpace;

            if (moveSpacesCount > 0)
                moveSpacesCount--;
            else if (moveSpacesCount < 0)
                moveSpacesCount++;

            moving = false;
            return;
        }

        //VERTICAL MOVEMENT
        float multiplier = parabolaHeight;

        //two different parabolas that meet in the middle
        if (progressNum < 0.5f)
            multiplier -= pos1.y;
        else
            multiplier -= pos2.y;

        //modify input to shrink horizontally and make the vertex when progressNum = 0.5
        float xmodify = 2f * progressNum - 1f;
        float objectHeight = -(multiplier * xmodify * xmodify) + parabolaHeight;

        float xPos = pos2.x * progressNum + pos1.x * (1 - progressNum);
        float zPos = pos2.z * progressNum + pos1.z * (1 - progressNum);
        piece.transform.position = new Vector3(xPos, objectHeight, zPos);
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
                int n = (int)(Random.value * 3f);
                //Piece Specific Movement Sounds
                switch (playerNumber)
                {
                    
                    case 0: //MoneyToken
                        if (n == 0) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.MoneyBag1, 0.3f);
                        }
                        if (n == 1) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.MoneyBag2, 0.3f);
                        } 
                        if (n == 2){
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.MoneyBag3, 0.3f);
                        }
                        break;

                    case 1: //CrowbarToken
                        if (n == 0) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Crowbar1, 0.3f);
                        }
                        if (n == 1) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Crowbar2, 0.3f);
                        } 
                        if (n == 2) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Crowbar3, 0.3f);
                        }
                        break;
                    case 2: //TommygunToken
                        if (n == 0) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Tommygun1, 0.2f);
                        }
                        if (n == 1) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Tommygun2, 0.2f);
                        } 
                        if (n == 2) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Tommygun3, 0.2f);
                        }
                        break;

                    case 3: //BowToken
                        if (n == 0) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Bow1, 0.4f);
                        }
                        if (n == 1) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Bow2, 0.4f);
                        } 
                        if (n == 2) {
                            SoundManager.PlaySoundSpecificVolume(SoundManager.Sound.Bow3, 0.4f);
                        }
                        break;
                }
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
