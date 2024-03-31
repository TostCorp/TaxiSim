using TaxiSim.Database.Primitives;
using TaxiSim.ServiceDefaults.Attributes;

namespace TaxiSim.Api.SharedContext;

[Lifetime(ServiceLifetime.Scoped)]
public class ScopedContext : IScopedContext
{
    public UserId UserId { get; private set; } = UserId.From(Guid.Empty);
    public bool IsLoggedIn => UserId != Guid.Empty;

    public void SetUserId(UserId userId)
    {
        if (userId == UserId.From(Guid.Empty))
        {
            throw new ArgumentNullException(nameof(userId));
        }

        if (UserId.Value == Guid.Empty)
        {
            UserId = userId;
            return;
        }

        throw new InvalidOperationException("You're not allowed to change the UserId after it's already set.");
    }
}

public interface IScopedContext
{
    public UserId UserId { get; }
    public bool IsLoggedIn { get; }

    public void SetUserId(UserId userId);
}