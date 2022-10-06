using System;
using UnityEngine;

public class Bank : MonoBehaviour, ILiquidityProvider, IHousingProvider
{
    public Int16 LiquidAssets { get; set; } = 20580;
    public Byte Housing { get; set; } = 0; // Unused
    public Byte Houses { get; set; } = 32;
    public Byte Hotels { get; set; } = 12;

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
