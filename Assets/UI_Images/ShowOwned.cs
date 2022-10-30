using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOwned : MonoBehaviour
{
    public GameObject target;
    public byte? player;

    private byte? owner;

    // Update is called once per frame
    void Update()
    {
        owner = target.GetComponent<Bank>().GetPropertyOwnerByIndex(0);
        Debug.Log(player);
    }
}
