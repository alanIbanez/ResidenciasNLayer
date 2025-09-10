namespace ResidenciasNLayer.Entities;

/// <summary>
/// Device entity with lowercase properties for PostgreSQL compatibility
/// </summary>
public class Device
{
    /// <summary>
    /// Device identifier - PRIMARY KEY
    /// </summary>
    public string deviceid { get; set; } = string.Empty;

    /// <summary>
    /// Firebase Cloud Messaging token
    /// </summary>
    public string tokenfcm { get; set; } = string.Empty;

    /// <summary>
    /// Device status - default true
    /// </summary>
    public bool estado { get; set; } = true;

    /// <summary>
    /// Navigation property for the user that owns this device
    /// </summary>
    public User? User { get; set; }
}
