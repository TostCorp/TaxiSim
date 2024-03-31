namespace TaxiSim.ServiceDefaults;

public readonly ref struct ConnectionStrings
{
    public const string Db = "sqldb";
    public const string RabbitMq = "mq";
}

public readonly ref struct DatabaseNames
{
    public const string TaxiSimDatabase = "TaxiSim";
}

public readonly ref struct ProjectNames
{
    public const string TaxiSimApiName = "taxisimapi";
}
