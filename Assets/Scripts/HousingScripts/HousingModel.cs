using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousingModel : MonoBehaviour
{
    [SerializeField] private GameControllerSys gameController;
    [SerializeField] private Bank bank;
    //[SerializeField] private GameObject House;
    //[SerializeField] private GameObject Hotel;
    [SerializeField] private GameObject transforms;
    [SerializeField] private GameObject housingObject;
    private List<Props> properties = new List<Props>();
    //private Dictionary<byte, List<GameObject>> housing = new Dictionary<byte, List<GameObject>>();
    //private Dictionary<byte, byte> housingLevel = new Dictionary<byte, byte>();
    public void UpdateHousing()
    {
        foreach (Props prop in properties)
        {
            
            byte index = prop.index;
            byte housingLevel = bank.GetStreetByIndex(index).Housing;
            Debug.Log(index + " " + housingLevel);
            if (housingLevel != prop.HousingLevel)
            {
                prop.SetHousing(housingLevel);
            }
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        properties = new List<Props>(housingObject.GetComponentsInChildren<Props>());

        foreach (Props prop in properties)
        {
            prop.SetHousing(0);
        }

        /*
        positions = new List<Transform>(transforms.GetComponentsInChildren<Transform>());
        positions.RemoveAt(0);
        positions.RemoveAt(11);
        positions.RemoveAt(40);

        for (byte i = 0; i < 40; i++)
        {
            if (gameController.GetTileType(i) == TileType.Property)
            {
                TileSpace tempTile = new TileSpace(i);
                if (tempTile.PropertyType == PropertyTileType.Street)
                {
                    housingLevel[i] = 0;
                    housing[i] = null;
                }
            }
        }
        */


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
