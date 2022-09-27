using System;

namespace Monopolyopoly
{
    /// <summary>
    /// The type of a board tile: corner, tax, community chest, chance, or property.
    /// </summary>
    public enum TileType : UInt16
    {
        Corner,
        Tax,
        CommunityChest,
        Chance,
        Property,
    }

    /// <summary>
    /// The type of a property: street, rail, or utility.
    /// </summary>
    public enum PropertyType : UInt16
    {
        Street,
        Rail,
        Utility
    }
}
