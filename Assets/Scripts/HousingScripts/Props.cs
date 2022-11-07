using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Props : MonoBehaviour
{
    [SerializeField] private GameObject Hotel;
    [SerializeField] private GameObject House1;
    [SerializeField] private GameObject House2;
    [SerializeField] private GameObject House3;
    [SerializeField] private GameObject House4;
    private byte housingLevel = 0;
    public byte HousingLevel { get { return housingLevel; } }
    public byte index;
    public void SetHousing(byte level)
    {
        housingLevel = level;
        switch (level)
        {
            case 0:
                Hotel.SetActive(false);
                House1.SetActive(false);
                House2.SetActive(false);
                House3.SetActive(false);
                House4.SetActive(false);
                break;
            case 1:
                Hotel.SetActive(false);
                House1.SetActive(false);
                House2.SetActive(true);
                House3.SetActive(false);
                House4.SetActive(false);
                break;
            case 2:
                Hotel.SetActive(false);
                House1.SetActive(false);
                House2.SetActive(true);
                House3.SetActive(true);
                House4.SetActive(false);
                break;
            case 3:
                Hotel.SetActive(false);
                House1.SetActive(true);
                House2.SetActive(true);
                House3.SetActive(true);
                House4.SetActive(false);
                break;
            case 4:
                Hotel.SetActive(false);
                House1.SetActive(true);
                House2.SetActive(true);
                House3.SetActive(true);
                House4.SetActive(true);
                break;
            case 5:
                Hotel.SetActive(true);
                House1.SetActive(false);
                House2.SetActive(false);
                House3.SetActive(false);
                House4.SetActive(false);
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
