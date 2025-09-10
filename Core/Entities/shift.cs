namespace ResidenciasNLayer.Core.Entities;

public class shift
{
    public int id { get; set; }
    public string name { get; set; } = string.Empty;
    public TimeOnly starttime { get; set; }
    public TimeOnly endtime { get; set; }
}