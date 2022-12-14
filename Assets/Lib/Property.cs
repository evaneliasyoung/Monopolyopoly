using System;

/// <summary>
/// Represents a tile space which can be purchased by an asset holder.
/// </summary>
public class Property : TileSpace, IProperty
{
    public Byte? Owner { get; set; } = null;
    public Boolean IsForSale => !Owner.HasValue;
    public UInt16 Price { get; set; }
    public Boolean IsMortgaged { get; set; } = false;
    public Byte MortgageValue => Convert.ToByte(Price / 2);
    public Byte UnmortgageCost => Convert.ToByte(MortgageValue * 1.1);
    new public PropertyTileType PropertyType => (PropertyTileType)base.PropertyType;

    /// <summary>
    /// Constructs a new Property object.
    /// </summary>
    /// <param name="index">The position on the board, GO is 0.</param>
    /// <param name="price">The purchase price of the Property.</param>
    public Property(byte index, UInt16 price) : base(index)
    {
        Price = price;
    }
}
