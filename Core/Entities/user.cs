namespace ResidenciasNLayer.Core.Entities;

public class user
{
    public int id { get; set; }
    public string username { get; set; } = string.Empty;
    public string passwordhash { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty; // "preceptor"|"tutor"|"guardia"|"residente"
}