using System;
using UnityEngine;

/// <summary>
/// Represents a space on the game board, can be derived into subclasses.
/// </summary>
public class TileSpace : MonoBehaviour, ITileSpace
{
    public Byte Index { get; }

    public TileType TileType => Index % 10 == 0
        ? TileType.Corner
        : Index == 4 || Index == 38
        ? TileType.Tax
        : Index == 7 || Index == 22
        ? TileType.Chance
        : Index == 2 || Index == 17
        ? TileType.CommunityChest
        : TileType.Property;

    public PropertyTileType? PropertyType => TileType == TileType.Property
        ? Index % 10 == 5
            ? PropertyTileType.Rail
            : Index == 12 || Index == 28
                ? PropertyTileType.Utility
                : PropertyTileType.Street
        : null;
}
