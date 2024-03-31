using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Rewrite;

using NetTopologySuite.Geometries;

using TaxiSim.Api.SharedContext;

namespace TaxiSim.Api.Features.Vehicles.Models;

public sealed class VehicleRequestDto : PositionDto
{
    public required double RadiusInMeters { get; init; }

    [BindNever]
    internal Lazy<double> RadiusInDegrees => new(GetRadiusInDegrees);

    [BindNever]
    internal Lazy<Geometry> RequestCircle => new(CreateCircle);

    private double GetRadiusInDegrees()
    {
        return RadiusInMeters / (111.32 * 1000 * Math.Cos(Latitude * (Math.PI / 180)));
    }

    private Geometry CreateCircle()
    {
        var centre = new Point(Latitude, Longitude) { SRID = 4326 };
        var circle = centre.Buffer(RadiusInDegrees.Value);

        return circle.Reverse();
    }
}
