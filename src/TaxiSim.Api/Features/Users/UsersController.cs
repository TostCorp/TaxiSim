using Microsoft.AspNetCore.Mvc;

using TaxiSim.Api.Features.Users.Models;
using TaxiSim.Api.Features.Users.Services;
using TaxiSim.Api.SharedContext;

using TostCorp.ObjectResults;

namespace TaxiSim.Api.Features.Users;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ApiControllerBase
{
    [HttpGet("get-user-location")]
    [ResponseCache(Duration = 5)]
    public async Task<IActionResult> GetUserLocation([FromServices] IGetUserPositionService requestService, [FromQuery] GetUserLocationDto request, CancellationToken token)
    {
        var result = await requestService.Fetch(request, token);
        return result.HandleResult();
    }

    [HttpPost("update-user-position")]
    public async Task<IActionResult> UpdateUserPosition([FromServices] IUpdateUserPositionService requestService, [FromForm] PositionDto request, CancellationToken token)
    {
        var result = await requestService.UpdatePosition(request, token);
        return result.HandleResult();
    }

    [HttpPost("change-user-status")]
    public async Task<IActionResult> ChangeUserStatus([FromServices] IChangeUserStatusService requestService, [FromForm] ChangeUserStatusDto request, CancellationToken token)
    {
        var result = await requestService.ChangeStatus(request, token);
        return result.HandleResult();
    }

    [HttpPost("broadcast-request-vehicle")]
    public IActionResult BroadcastRequestVehicle([FromServices] IRequestForVehicleService requestService, [FromForm] BroadcastVehicleRequestDto request)
    {
        var result = requestService.BroadcastRequestForVehicle(request);
        return result.HandleResult();
    }
}
