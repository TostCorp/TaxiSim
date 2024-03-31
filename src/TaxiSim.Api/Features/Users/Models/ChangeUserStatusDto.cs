using Microsoft.AspNetCore.Mvc.ModelBinding;

using TaxiSim.Database.Enums;

namespace TaxiSim.Api.Features.Users.Models;

public class ChangeUserStatusDto
{
    [BindRequired]
    public required UserStatuses UserStatus { get; init; }
}
