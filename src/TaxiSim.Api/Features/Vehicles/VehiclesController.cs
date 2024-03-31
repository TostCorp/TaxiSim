using Microsoft.AspNetCore.Mvc;

using TaxiSim.Api.Features.Vehicles.Models;
using TaxiSim.Api.Features.Vehicles.Services;
using TaxiSim.Api.SharedContext;
using TaxiSim.Database.Primitives;

using TostCorp.ObjectResults;

namespace TaxiSim.Api.Features.Vehicles;

[ApiController]
[Route("[controller]")]
public class VehiclesController : ApiControllerBase
{
    [HttpGet("all-vehicles")]
    [ResponseCache(Duration = 5)]
    public async Task<IActionResult> GetAllVehicles([FromServices] IGetVehicleService getVehicleService, [FromQuery] VehicleRequestDto request, CancellationToken token)
    {
        var result = await getVehicleService.Fetch(request, token);
        return result.HandleResult();
    }

    [HttpGet("get-vehicle-location")]
    [ResponseCache(Duration = 5)]
    public async Task<IActionResult> GetVehicleLocation([FromServices] IGetVehiclePositionService requestService, [FromQuery] GetVehicleLocationDto request, CancellationToken token)
    {
        var result = await requestService.Fetch(request, token);
        return result.HandleResult();
    }

    [HttpPost("create-vehicle")]
    public async Task<IActionResult> CreateVehicle([FromServices] INewVehicleService newVehicleService, [FromForm] NewVehicleDto request, CancellationToken token)
    {
        var result = await newVehicleService.Create(request, token);
        return result.HandleResult();
    }

    [HttpPost("update-vehicle-position")]
    public async Task<IActionResult> UpdateVehiclePosition([FromServices] IUpdateVehiclePositionService requestService, [FromForm] AvailableVehicleBroadcastDto request, CancellationToken token)
    {
        var result = await requestService.UpdatePosition(request, token);
        return result.HandleResult();
    }

    [HttpPost("change-vehicle-status")]
    public async Task<IActionResult> ChangeVehicleStatus([FromServices] IChangeVehicleStatusService requestService, [FromForm] ChangeVehicleStatusDto request, CancellationToken token)
    {
        var result = await requestService.ChangeStatus(request, token);
        return result.HandleResult();
    }

    [HttpDelete("delete-vehicle")]
    public async Task<IActionResult> DeleteVehicle([FromServices] IDeleteVehicleService deleteService, [FromForm] VehicleId id, CancellationToken token)
    {
        var result = await deleteService.Delete(id, token);
        return result.HandleResult();
    }
}
