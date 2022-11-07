using System;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    private readonly Byte RailMonopolyScaling = 50;
    private readonly Byte UtilityBaseScale = 4;
    private readonly Byte UtilityMonopolyScale = 10;
    private readonly Byte IncomeTax = 200;
    private readonly Byte LuxuryTax = 75;
    private readonly Byte[][] Monopolies = new Byte[8][]{
        new Byte[2]{1,3},
        new Byte[3]{6,8,9},
        new Byte[3]{11,13,14},
        new Byte[3]{16,18,19},
        new Byte[3]{21,23,24},
        new Byte[3]{26,27,29},
        new Byte[3]{31,32,34},
        new Byte[2]{37,39}
    };
    public Byte Houses { get; set; } = 32;
    public Byte Hotels { get; set; } = 12;
    public Dictionary<Byte, Street> Streets { get; } = new Dictionary<byte, Street>();
    public Dictionary<Byte, Property> Properties { get; } = new Dictionary<byte, Property>();

    public Bank()
    {
        // Streets
        Streets.Add(1, new Street(1, 60, new UInt16[6] { 2, 10, 30, 90, 160, 250 }));
        Streets.Add(3, new Street(3, 60, new UInt16[6] { 4, 20, 60, 180, 320, 450 }));
        Streets.Add(6, new Street(6, 100, new UInt16[6] { 6, 30, 90, 270, 400, 550 }));
        Streets.Add(8, new Street(8, 100, new UInt16[6] { 6, 30, 90, 270, 400, 550 }));
        Streets.Add(9, new Street(9, 120, new UInt16[6] { 8, 40, 100, 300, 450, 600 }));
        Streets.Add(11, new Street(11, 140, new UInt16[6] { 10, 50, 150, 450, 625, 750 }));
        Streets.Add(13, new Street(13, 140, new UInt16[6] { 10, 50, 150, 450, 625, 750 }));
        Streets.Add(14, new Street(14, 160, new UInt16[6] { 12, 60, 180, 500, 700, 900 }));
        Streets.Add(16, new Street(16, 180, new UInt16[6] { 14, 70, 200, 550, 750, 950 }));
        Streets.Add(18, new Street(18, 180, new UInt16[6] { 14, 70, 200, 550, 750, 950 }));
        Streets.Add(19, new Street(19, 200, new UInt16[6] { 16, 80, 220, 600, 800, 1000 }));
        Streets.Add(21, new Street(21, 220, new UInt16[6] { 18, 90, 250, 700, 875, 1050 }));
        Streets.Add(23, new Street(23, 220, new UInt16[6] { 18, 90, 250, 700, 875, 1050 }));
        Streets.Add(24, new Street(24, 240, new UInt16[6] { 20, 100, 300, 750, 925, 1100, }));
        Streets.Add(26, new Street(26, 260, new UInt16[6] { 22, 110, 330, 800, 975, 1150 }));
        Streets.Add(27, new Street(27, 260, new UInt16[6] { 22, 110, 330, 800, 975, 1150 }));
        Streets.Add(29, new Street(29, 280, new UInt16[6] { 24, 120, 360, 850, 1025, 1200 }));
        Streets.Add(31, new Street(31, 300, new UInt16[6] { 26, 130, 390, 900, 1100, 1275 }));
        Streets.Add(32, new Street(32, 300, new UInt16[6] { 26, 130, 390, 900, 1100, 1275 }));
        Streets.Add(34, new Street(34, 320, new UInt16[6] { 28, 150, 450, 1000, 1200, 1400 }));
        Streets.Add(37, new Street(37, 350, new UInt16[6] { 35, 175, 500, 1100, 1300, 1500 }));
        Streets.Add(39, new Street(39, 400, new UInt16[6] { 50, 200, 600, 1400, 1700, 2000 }));

        // Railroads
        Properties.Add(5, new Property(5, 200));
        Properties.Add(15, new Property(15, 200));
        Properties.Add(25, new Property(25, 200));
        Properties.Add(35, new Property(35, 200));

        // Utilities
        Properties.Add(12, new Property(12, 150));
        Properties.Add(28, new Property(28, 150));
    }

    public Street GetStreetByIndex(Byte Index)
    {
        return Streets[Index];
    }

    public Property GetRailOrUtilityByIndex(Byte Index)
    {
        return Properties[Index];
    }

    public Property GetPropertyByIndex(Byte Index)
    {
        if (Streets.ContainsKey(Index))
        {
            return (Property)(GetStreetByIndex(Index));
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

    public UInt16 GetPropertyCostByIndex(Byte Index)
    {
        return GetPropertyByIndex(Index).Price;
    }

    public Boolean StreetIsMonopolized(Street street)
    {
        if (!street.IsForSale)
        {
            Byte boardIndex = street.Index;
            Byte monopolyIndex = Convert.ToByte(Math.Floor(boardIndex / 5D));
            foreach (Byte testIndex in Monopolies[monopolyIndex])
            {
                if (testIndex == boardIndex) continue; // Skip the "selected" Street
                else if (GetPropertyOwnerByIndex(testIndex) != street.Owner) return false;
            }
            return true;
        }
        return false;
    }

    private Byte GetRailRent(Property Rail)
    {
        Byte rent = RailMonopolyScaling;
        for (Byte i = 5; i < 45; i += 10)
        {
            if (i == Rail.Index) continue;
            else if (GetPropertyOwnerByIndex(i) == Rail.Owner) rent += RailMonopolyScaling;
        }
        return rent;
    }

    private UInt16 GetUtilityRent(Property Utility, Byte RollValue = 7)
    {
        return (UInt16)(RollValue * (
            GetPropertyOwnerByIndex(Utility.Index == 12 ? (byte)28 : (byte)12) == Utility.Owner
                ? UtilityMonopolyScale
                : UtilityBaseScale
        ));
    }

    private UInt16 GetStreetRent(Street street)
    {
        return street.Housing > (byte)0
            ? street.RentCosts[street.Housing]
            : StreetIsMonopolized(street)
                ? (UInt16)(2 * street.RentCosts[0])
                : street.RentCosts[0];
    }

    public UInt16 GetPropertyRent(Property property, PlayerObj player)
    {
        if (property.Owner.HasValue && property.Owner != player.Index)
        {
            switch (property.PropertyType)
            {
                case PropertyTileType.Street:
                    return GetStreetRent((Street)property);
                case PropertyTileType.Rail:
                    return GetRailRent(property);
                case PropertyTileType.Utility:
                    return GetUtilityRent(property);
            }
        }
        return 0;
    }

    public UInt16 GetTileRent(TileSpace tile, PlayerObj player)
    {
        switch (tile.TileType)
        {
            case TileType.Tax:
                return tile.Index == 4 ? IncomeTax : LuxuryTax;
            case TileType.Property:
                return GetPropertyRent(GetPropertyByIndex(tile.Index), player);
            default:
                return 0;
        }
    }

    public UInt16 GetTileRentByIndex(Byte Index, PlayerObj player)
    {
        return GetTileRent(new TileSpace(Index), player);
    }

    public bool OwnerCanPurchaseProperty(IPropertyOwner Owner, IProperty Property)
    {
        return Property.IsForSale && Owner.LiquidAssets >= Property.Price;
    }

    public bool OwnerCanPurchaseProperty(IPropertyOwner Owner, Byte Index)
    {
        return OwnerCanPurchaseProperty(Owner, GetPropertyByIndex(Index));
    }

    /// <summary>
    /// Processes an asset holder pruchasing a property.
    /// </summary>
    /// <param name="Owner">The asset holder purchasing a property.</param>
    /// <param name="Property">The property being purchased.</param>
    /// <returns>True if the purchase went through, false otherwise.</returns>
    bool PurchaseProperty(ref IPropertyOwner Owner, ref Property Property)
    {
        if (OwnerCanPurchaseProperty(Owner, Property))
        {
            Owner.LiquidAssets -= Convert.ToInt16(Property.Price);
            Property.Owner = Owner.Index;
            return true;
        }
        return false;
    }

    public bool PurchaseProperty(ref IPropertyOwner Owner, Byte Index)
    {
        var Property = Streets.ContainsKey(Index) ? Streets[Index] : Properties[Index];
        return PurchaseProperty(ref Owner, ref Property);
    }

    /// <summary>
    /// Processes an asset holder mortgaging a property.
    /// </summary>
    /// <param name="Owner">The asset holder mortgaging a property.</param>
    /// <param name="Property">The property being mortgaged.</param>
    /// <returns>True if the mortgage went through, false otherwise.</returns>
    bool MortgageProperty(ref IPropertyOwner Owner, ref Property Property)
    {
        if (Property.Owner == Owner.Index && !Property.IsMortgaged && (Property.PropertyType != PropertyTileType.Street || (Property.PropertyType == PropertyTileType.Street && ((IStreet)Property).Housing == 0)))
        {
            Owner.LiquidAssets += Property.MortgageValue;
            Property.IsMortgaged = true;
            return true;
        }
        return false;
    }

    public bool MortgageProperty(ref IPropertyOwner Owner, Byte Index)
    {
        var Property = Streets.ContainsKey(Index) ? Streets[Index] : Properties[Index];
        return MortgageProperty(ref Owner, ref Property);
    }

    /// <summary>
    /// Processes an asset holder mortgaging a property.
    /// </summary>
    /// <param name="Owner">The asset holder mortgaging a property.</param>
    /// <param name="Property">The property being mortgaged.</param>
    /// <returns>True if the mortgage went through, false otherwise.</returns>
    bool UnmortgageProperty(ref IPropertyOwner Owner, ref Property Property)
    {
        if (Property.Owner == Owner.Index && Property.IsMortgaged && Owner.LiquidAssets >= Property.UnmortgageCost)
        {
            Owner.LiquidAssets -= Property.UnmortgageCost;
            Property.IsMortgaged = false;
            return true;
        }
        return false;
    }

    public bool UnmortgageProperty(ref IPropertyOwner Owner, Byte Index)
    {
        var Property = Streets.ContainsKey(Index) ? Streets[Index] : Properties[Index];
        return UnmortgageProperty(ref Owner, ref Property);
    }

    /// <summary>
    /// Processes an asset holder building on a property.
    /// </summary>
    /// <param name="Owner">The asset holder building on a property.</param>
    /// <param name="Street">The property being built upon.</param>
    /// <returns>True if the building went through, false otherwise.</returns>
    bool BuildResidence(ref IPropertyOwner Owner, ref Street Street)
    {
        if (Street.Owner == Owner.Index && !Street.IsMortgaged && Street.Housing < 5 && Owner.LiquidAssets >= Street.BuildCost)
        {
            if (Street.Houses == 4) //&& Hotels > 0
            {
                Hotels--;
            }
            else if (Street.Houses < 4) //&& Houses > 0
            {
                Houses--;
            }
            else
            {
                return false;
            }
            Owner.LiquidAssets -= Street.BuildCost;
            ++Street.Housing;
            return true;
        }
        return false;
    }

    public bool BuildResidence(ref IPropertyOwner Owner, Byte Index)
    {
        var Street = Streets[Index];
        return BuildResidence(ref Owner, ref Street);
    }

    /// <summary>
    /// Processes an asset holder demolishing a residence.
    /// </summary>
    /// <param name="Owner">The asset holder demolishing a residence.</param>
    /// <param name="Street">The property being with the residence to be demolished.</param>
    /// <returns>True if the demolishing a residence went through, false otherwise.</returns>
    bool DemolishResidence(ref IPropertyOwner Owner, ref Street Street)
    {
        if (Street.Owner == Owner.Index && !Street.IsMortgaged && Street.Housing >= 1)
        {
            Owner.LiquidAssets += Convert.ToInt16(Street.BuildCost / 2);
            if (Street.Hotels == 1)
            {
                ++Hotels;
            }
            else
            {
                ++Houses;
            }
            --Street.Housing;
            return true;
        }
        return false;
    }

    public bool DemolishResidence(ref IPropertyOwner Owner, Byte Index)
    {
        var Street = Streets[Index];
        return DemolishResidence(ref Owner, ref Street);
    }
}
