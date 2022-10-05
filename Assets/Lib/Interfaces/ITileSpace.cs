/// <summary>
/// A tile space is a space with an index, and type.
/// </summary>
public interface ITileSpace : IWithIndex
{
    /// <summary>
    /// The type of tile: corner, tax, community chest, chance, or property.
    /// </summary>
    /// <value>The type of tile.</value>
    public TileType TileType { get; }

    /// <summary>
    /// The type of property: street, rail, or utility.
    /// </summary>
    /// <value>The type of property.</value>
    public PropertyTileType? PropertyType { get; }
}
