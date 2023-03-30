using System.ComponentModel.DataAnnotations;

namespace GrainPath.Application.Entities;

/// <summary>
/// Representation of a point embedded in two-dimensional Cartesian space.
/// </summary>
public sealed class CartesianPoint
{
    /// <summary>
    /// X-offset from the origin.
    /// </summary>
    public double x { get; }

    /// <summary>
    /// Y-offset from the origin.
    /// </summary>
    public double y { get; }

    public CartesianPoint(double x, double y)
    {
        this.x = x; this.y = y;
    }

    public override string ToString() => $"{{ X: {x}, Y: {y} }}";
}

/// <summary>
/// Representation of a point on an ellipsoidal body.
/// </summary>
public sealed class GeodeticPoint
{
    /// <summary>
    /// Longitude.
    /// </summary>
    public double lon { get; }

    /// <summary>
    /// Latitude.
    /// </summary>
    public double lat { get; }

    public GeodeticPoint(double lon, double lat)
    {
        this.lon = lon; this.lat = lat;
    }

    public override string ToString() => $"{{ Lon: {lon}, Lat: {lat} }}";
}

/// <summary>
/// Representation of a geodetic points projected onto 2D flat space.
/// </summary>
public sealed class ProjectedPoint
{
    /// <summary>
    /// Easting.
    /// </summary>
    public double eas { get; set; }

    /// <summary>
    /// Northing.
    /// </summary>
    public double nor { get; set; }

    public ProjectedPoint(double eas, double nor)
    {
        this.eas = eas; this.nor = nor;
    }

    public override string ToString() => $"{{ Eas: {eas}, Nor: {nor} }}";
}

/// <summary>
/// Representation of a point in EPSG:4326 restricted to the range of EPSG:3857.
/// See https://epsg.io/4326 and https://epsg.io/3857 for details.
/// </summary>
public sealed class WebPoint
{
    [Required]
    [Range(-180.0, 180.0)]
    public double? lon { get; set; }

    [Required]
    [Range(-85.06, 85.06)]
    public double? lat { get; set; }

    public GeodeticPoint AsGeodetic() => new(lon.Value, lat.Value);

    public CartesianPoint AsCartesian() => new(lon.Value, lat.Value);
}
