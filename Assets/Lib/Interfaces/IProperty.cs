using System;

namespace Monopolyopoly
{
    /// <summary>
    /// A property can be purchased and mortgaged.
    /// </summary>
    public interface IProperty : ITileSpace
    {
        /// <summary>
        /// The owner (or null).
        /// </summary>
        /// <value>Owner ID</value>
        public Byte? Owner { get; set; }

        /// <summary>
        /// Whether or not the property is for sale.
        /// </summary>
        /// <value>True if unowned, false otherwise.</value>
        public Boolean IsForSale { get; }

        /// <summary>
        /// The cost for a potential buyer.
        /// </summary>
        /// <value>The purchase price.</value>
        public UInt16 Price { get; set; }

        /// <summary>
        /// Whether or not the property is mortgaged.
        /// </summary>
        /// <value>True if mortgaged, false otherwise.</value>
        public Boolean IsMortgaged { get; set; }

        /// <summary>
        /// The liquidity provided by mortgaging a property.
        /// </summary>
        /// <value>The mortgage liquidity value.</value>
        public Byte MortgageValue { get; }

        /// <summary>
        /// The liquidity required to unmortgage a property.
        /// </summary>
        /// <value>The unmortgage liquidity cost.</value>
        public Byte UnmortgageCost { get; }

        /// <summary>
        /// The type of property: street, rail, or utility.
        /// </summary>
        /// <value>The type of property.</value>
        new public PropertyType PropertyType { get; }
    }
}
