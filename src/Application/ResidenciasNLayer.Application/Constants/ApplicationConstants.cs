namespace ResidenciasNLayer.Application.Constants;

public static class ExitStatusNames
{
    public const string Solicitado = "solicitado";
    public const string EnProceso = "en_proceso";
    public const string AutorizacionTutor = "autorizacion_tutor";
    public const string AutorizacionPreceptor = "autorizacion_preceptor";
    public const string Autorizado = "autorizado";
    public const string Rechazado = "rechazado";
    public const string Cancelado = "cancelado";
}

public static class ExitTypeNames
{
    public const string Casual = "casual";
    public const string Especial = "especial";
}

public static class ResidentTypeNames
{
    public const string Universitario = "universitario";
    public const string Colegio = "colegio";
}

public static class RoleNames
{
    public const string Preceptor = "Preceptor";
    public const string Tutor = "Tutor";
    public const string Guardia = "Guardia";
    public const string Residente = "Residente";
}

public static class ExitAuthorizationActions
{
    public const string Requested = "requested";
    public const string Approved = "approved";
    public const string Rejected = "rejected";
    public const string Canceled = "canceled";
    public const string GuardDeparture = "guard_departure";
    public const string GuardReturn = "guard_return";
}