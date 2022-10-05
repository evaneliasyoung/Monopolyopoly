using System;

/// <summary>
/// A liquidity provider has spendable cash in hand.
/// </summary>
public interface ILiquidityProvider
{
    /// <summary>
    /// The amount of liquid assets (cash in hand).
    /// </summary>
    /// <value>The liquid assets (cash in hand).</value>
    public Int16 LiquidAssets { get; set; }
}
