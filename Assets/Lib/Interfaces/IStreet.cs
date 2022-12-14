using System;

/// <summary>
/// A street can be purchased, mortgaged, and built upon.
/// </summary>
public interface IStreet : IProperty, IHousingProvider
{
    /// <summary>
    /// The liquidity required to build on a property.
    /// </summary>
    /// <value>The build liquidity cost.</value>
    public Byte BuildCost { get; }

    /// <summary>
    /// The liquidity provided by demolishing a residence.
    /// </summary>
    /// <value>The demolish liquidity value.</value>
    public Byte ResidenceValue { get; }

    /// <summary>
    /// The liquidity required when staying at a street.
    /// </summary>
    /// <value>Rent per the number of houses.</value>
    public UInt16[] RentCosts { get; }
}
