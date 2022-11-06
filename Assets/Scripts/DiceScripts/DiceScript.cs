using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceScript : MonoBehaviour
{
    public GameObject dice;
    public Rigidbody dicePhys;
    public MeshRenderer diceRender;
    public DiceParentScript diceParent;
    public bool started = false;
    public Vector3 origin;
    public int diceNum;

    /// <summary>
    /// Throw dice
    /// </summary>
    public void Throw()
    {
        dicePhys.position = origin + diceParent.transform.position;
        dicePhys.rotation = Random.rotation;

        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(1f, 3f);
        float z = Random.Range(-0.5f, 0.5f);
        dicePhys.velocity = new Vector3(x, y, z);

        x = Random.Range(-5f, 5f);
        y = Random.Range(-5f, 5f);
        z = Random.Range(-5f, 5f);
        dicePhys.angularVelocity = new Vector3(x, y, z);

        started = true;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (dicePhys.velocity.magnitude < 0.001f)
            {
                //Debug.Log(dice.transform.eulerAngles);
                diceParent.DiceStopped(diceNum, dice.transform.eulerAngles);
                started = false;
            }
        }
    }
}
