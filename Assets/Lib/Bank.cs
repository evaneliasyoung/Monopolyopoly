using System;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour, ILiquidityProvider, IHousingProvider
{
    public Int16 LiquidAssets { get; set; } = 20580;
    public Byte Housing { get; set; } = 0; // Unused
    public Byte Houses { get; set; } = 32;
    public Byte Hotels { get; set; } = 12;
    public Dictionary<Byte, Street> Streets { get; } = new Dictionary<byte, Street>();
    public Dictionary<Byte, Property> Properties { get; } = new Dictionary<byte, Property>();

    private void Start()
    {
        // Streets
        Streets.Add(1, new Street(1, 60));
        Streets.Add(3, new Street(3, 60));
        Streets.Add(6, new Street(6, 100));
        Streets.Add(8, new Street(8, 100));
        Streets.Add(9, new Street(9, 120));
        Streets.Add(11, new Street(11, 140));
        Streets.Add(13, new Street(13, 140));
        Streets.Add(14, new Street(14, 160));
        Streets.Add(16, new Street(16, 180));
        Streets.Add(18, new Street(18, 180));
        Streets.Add(19, new Street(19, 200));
        Streets.Add(21, new Street(21, 220));
        Streets.Add(23, new Street(23, 220));
        Streets.Add(24, new Street(24, 240));
        Streets.Add(26, new Street(26, 260));
        Streets.Add(27, new Street(27, 260));
        Streets.Add(29, new Street(29, 280));
        Streets.Add(31, new Street(31, 300));
        Streets.Add(32, new Street(32, 300));
        Streets.Add(34, new Street(34, 320));
        Streets.Add(37, new Street(37, 350));
        Streets.Add(39, new Street(39, 400));

        // Railroads
        Properties.Add(5, new Property(5, 200));
        Properties.Add(15, new Property(15, 200));
        Properties.Add(25, new Property(25, 200));
        Properties.Add(35, new Property(35, 200));

        // Utilities
        Properties.Add(12, new Property(12, 150));
        Properties.Add(28, new Property(28, 150));
    }

    public ref Street GetStreetByIndex(Byte Index)
    {
        return Streets[Index];
    }

    public ref Property GetRailOrUtilityByIndex(Byte Index)
    {
        return Properties[Index];
    }

    public ref Property GetPropertyByIndex(Byte Index)
    {
        if (Streets.ContainsKey(Index))
        {
            return GetStreetByIndex(Index);
        }
        else if (Properties.ContainsKey(Index))
        {
            return GetRailOrUtilityByIndex(Index);
        }
        else
        {
            throw new IndexOutOfRangeException("Index must be in [0, 39]");
        }
    }

    public Byte? GetPropertyOwnerByIndex(Byte Index)
    {
        return GetPropertyByIndex(Index).Owner;
    }

    public Byte GetPropertyCostByIndex(Byte Index)
    {
        return GetPropertyByIndex(Index).Price;
    }

    public bool OwnerCanPurchaseProperty(IPropertyOwner Owner, IProperty Property)
    {
        return Property.IsForSale && Owner.LiquidAssets >= Property.Price;
    }

    /// <summary>
    /// Attempts to make a transfer from the bank to an asset holder.
    /// </summary>
    /// <param name="Owner">The asset holder receiving funds.</param>
    /// <param name="Amount">The amount of funds to transfer.</param>
    /// <returns>True if the transfer succeeded, false otherwise.</returns>
    bool TransferTo(ref IPropertyOwner Owner, UInt16 Amount)
    {
        if (LiquidAssets >= Amount)
        {
            Int16 ModAmount = System.Convert.ToInt16(Amount);
            LiquidAssets -= ModAmount;
            Owner.LiquidAssets += ModAmount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Attempts to make a transfer from an asset holder to the bank.
    /// </summary>
    /// <param name="Owner">The asset holder sending funds.</param>
    /// <param name="Amount">The amount of funds to transfer.</param>
    /// <returns>True if the transfer succeeded, false otherwise.</returns>
    bool TransferFrom(ref IPropertyOwner Owner, UInt16 Amount)
    {
        if (Owner.LiquidAssets >= Amount)
        {
            Int16 ModAmount = System.Convert.ToInt16(Amount);
            LiquidAssets += ModAmount;
            Owner.LiquidAssets -= ModAmount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Processes an asset holder pruchasing a property.
    /// </summary>
    /// <param name="Owner">The asset holder purchasing a property.</param>
    /// <param name="Property">The property being purchased.</param>
    /// <returns>True if the purchase went through, false otherwise.</returns>
    bool PurchaseProperty(ref IPropertyOwner Owner, ref IProperty Property)
    {
        if (Property.IsForSale && Owner.LiquidAssets >= Property.Price)
        {
            if (TransferFrom(ref Owner, Property.Price))
            {
                Property.Owner = Owner.Index;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Processes an asset holder mortgaging a property.
    /// </summary>
    /// <param name="Owner">The asset holder mortgaging a property.</param>
    /// <param name="Property">The property being mortgaged.</param>
    /// <returns>True if the mortgage went through, false otherwise.</returns>
    bool MortgageProperty(ref IPropertyOwner Owner, ref IProperty Property)
    {
        if (Property.Owner == Owner.Index && !Property.IsMortgaged)
        {
            if (Property.PropertyType != PropertyTileType.Street || (Property.PropertyType == PropertyTileType.Street && ((IStreet)Property).Housing == 0))
            {
                if (TransferTo(ref Owner, Property.MortgageValue))
                {
                    Property.IsMortgaged = true;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Processes an asset holder mortgaging a property.
    /// </summary>
    /// <param name="Owner">The asset holder mortgaging a property.</param>
    /// <param name="Property">The property being mortgaged.</param>
    /// <returns>True if the mortgage went through, false otherwise.</returns>
    bool UnmortgageProperty(ref IPropertyOwner Owner, ref IProperty Property)
    {
        if (Property.Owner == Owner.Index && Property.IsMortgaged)
        {
            if (TransferFrom(ref Owner, Property.UnmortgageCost))
            {
                Property.IsMortgaged = false;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Processes an asset holder building on a property.
    /// </summary>
    /// <param name="Owner">The asset holder building on a property.</param>
    /// <param name="Street">The property being built upon.</param>
    /// <returns>True if the building went through, false otherwise.</returns>
    bool BuildResidence(ref IPropertyOwner Owner, ref IStreet Street)
    {
        if (Street.Owner == Owner.Index && !Street.IsMortgaged && Street.Housing < 5)
        {
            if (Street.Houses == 4 && Hotels > 0 && TransferFrom(ref Owner, Convert.ToUInt16(Street.BuildCost)))
            {
                Street.Housing += Amount;
                Hotels -= 1;
                return true;
            }
            else if (Street.Houses < 4 && Houses > 0 && TransferFrom(ref Owner, Convert.ToUInt16(Street.BuildCost)))
            {
                Street.Housing += Amount;
                Houses -= 1;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Processes an asset holder demolishing a residence.
    /// </summary>
    /// <param name="Owner">The asset holder demolishing a residence.</param>
    /// <param name="Street">The property being with the residence to be demolished.</param>
    /// <returns>True if the demolishing a residence went through, false otherwise.</returns>
    bool DemolishResidence(ref IPropertyOwner Owner, ref IStreet Street)
    {
        if (Street.Owner == Owner.Index && !Street.IsMortgaged && Street.Housing >= 1)
        {
            if (TransferTo(ref Owner, Convert.ToUInt16(Amount * Street.BuildCost / 2)))
            {
                if (Street.Hotels == 1)
                {
                    Hotels += 1;
                }
                else
                {
                    Houses += 1;
                }
                Street.Housing -= Amount;
                return true;
            }
        }
        return false;
    }
}
