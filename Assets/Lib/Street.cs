using System;

/// <summary>
/// Represents a property which can be built upon when an asset holder has the monopoly.
/// </summary>
public class Street : Property, IStreet
{
    public Byte BuildCost => Convert.ToByte(50 * ((Math.Floor(Index / 1e1)) + 1));
    public Byte ResidenceValue => Convert.ToByte(BuildCost / 2);
    public Byte Housing { get; set; } = 0;
    public Byte Houses => Convert.ToByte(Housing % 5);
    public Byte Hotels => Convert.ToByte(Housing == 5);
    public UInt16[] RentCosts { get; } = new UInt16[6];

    /// <summary>
    /// Constructs a new Street object.
    /// </summary>
    /// <param name="index">The position on the board, GO is 0.</param>
    /// <param name="price">The purchase price of the Street.</param>
    /// <param name="rents">The rent costs of the Street.</param>
    public Street(byte index, UInt16 price, UInt16[] rents) : base(index, price)
    {
        rents.CopyTo(RentCosts, 0);
    }
}
