using System;

/// <summary>
/// A housing provider has an amount of housing and hotelling available.
/// </summary>
public interface IHousingProvider
{
    /// <summary>
    /// The amount of total housing available.
    /// </summary>
    /// <value>The total housing available.</value>
    public Byte Housing { get; set; }

    /// <summary>
    /// The amount of houses built or possessed.
    /// </summary>
    /// <value>The houses built or possessed.</value>
    public Byte Houses { get; }

    /// <summary>
    /// The amount of hotels built or possessed.
    /// </summary>
    /// <value>The hotels built or possessed.</value>
    public Byte Hotels { get; }
}
