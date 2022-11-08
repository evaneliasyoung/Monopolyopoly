using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private GameControllerSys gameController;
    [SerializeField] private Bank bank;
    [SerializeField] private List<PlayerObj> players;
    [SerializeField] private HousingModel housing;

    void BuyAllProperties(byte player)
    {
        IPropertyOwner playa = (IPropertyOwner)players[player];
        for (byte i = 0; i < 40; i++)
        {
            if (gameController.GetTileType(i) == TileType.Property)
            {
                bank.PurchaseProperty(ref playa, i);
            }
        }
    }

    void Upgrade(byte player)
    {
        IPropertyOwner playa = (IPropertyOwner)players[player];
        for (byte i = 0; i < 40; i++)
        {
            if (gameController.GetPropertyType(i) == (int)PropertyTileType.Street)
            {
                if (bank.GetStreetByIndex(i).IsMortgaged)
                {
                    bank.UnmortgageProperty(ref playa, i);
                }
                else
                {
                    bank.BuildResidence(ref playa, i);
                }
            }
        }
        housing.UpdateHousing();
    }
    void Downgrade(byte player)
    {
        IPropertyOwner playa = (IPropertyOwner)players[player];
        for (byte i = 0; i < 40; i++)
        {
            if (gameController.GetPropertyType(i) == (int)PropertyTileType.Street)
            {
                if (!bank.GetStreetByIndex(i).IsMortgaged && bank.GetStreetByIndex(i).Housing == 0)
                {
                    bank.MortgageProperty(ref playa, i);
                }
                else
                {
                    bank.DemolishResidence(ref playa, i);
                }
            }
        }
        housing.UpdateHousing();
    }

    void MoneyFunc(byte player)
    {
        players[player].PlayerMoney += 20;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad1))
        {
            MoneyFunc(0);
        }
        if (Input.GetKey(KeyCode.Keypad2))
        {
            MoneyFunc(1);
        }
        if (Input.GetKey(KeyCode.Keypad3))
        {
            MoneyFunc(2);
        }
        if (Input.GetKey(KeyCode.Keypad4))
        {
            MoneyFunc(3);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuyAllProperties(0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Upgrade(0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Downgrade(0);
        }
    }
}
