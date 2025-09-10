namespace ResidenciasNLayer.Entities;

/// <summary>
/// User entity with device reference using lowercase properties for PostgreSQL compatibility
/// </summary>
public class User
{
    /// <summary>
    /// User identifier - PRIMARY KEY
    /// </summary>
    public int userid { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string username { get; set; } = string.Empty;

    /// <summary>
    /// User email
    /// </summary>
    public string email { get; set; } = string.Empty;

    /// <summary>
    /// Device identifier - Foreign Key to Device
    /// </summary>
    public string? deviceid { get; set; }

    /// <summary>
    /// Navigation property to the associated device
    /// </summary>
    public Device? Device { get; set; }
}