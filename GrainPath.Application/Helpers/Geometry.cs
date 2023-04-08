using System;
using System.Collections.Generic;
using System.Linq;
using GrainPath.Application.Entities;
using NetTopologySuite.Geometries;
using NetTopologySuite.Geometries.Utilities;
using NetTopologySuite.Utilities;

namespace GrainPath.Application.Helpers.Geometry;

internal static class Spherical
{
    /// <summary>
    /// Arithmetic mean radius.
    /// <list>
    ///   <item><see href="https://en.wikipedia.org/wiki/Earth_radius#Arithmetic_mean_radius"/></item>
    ///   <item><see href="https://en.wikipedia.org/wiki/Earth_radius#Published_values"/></item>
    /// </list>
    /// </summary>
    private static double EarthMeanRadius => 6_371_008.8;

    private static readonly double _deg2rad = Math.PI / 180.0;
    private static readonly double _rad2deg = 180.0 / Math.PI;

    /// <summary>
    /// Converts degrees to radians.
    /// </summary>
    private static double DegToRad(double deg) => deg * _deg2rad;

    /// <summary>
    /// Converts radians to degrees.
    /// </summary>
    private static double RadToDeg(double rad) => rad * _rad2deg;

    /// <summary>
    /// Weight of one longitudinal radian at a certain latitude.
    /// </summary>
    /// <param name="lat">Latitude in radians.</param>
    private static double LonRadCost(double lat) => Math.Cos(lat);

    /// <summary>
    /// Length of one radian along longitude at a given latitude.
    /// </summary>
    /// <param name="lat">Latitude in radians.</param>
    /// <returns>Length in meters.</returns>
    private static double LonRadLen(double lat) => EarthMeanRadius * LonRadCost(lat);

    /// <summary>
    /// Absolute longitudinal difference.
    /// </summary>
    /// <returns>Difference in radians.</returns>
    private static double LonRadDif(WgsPoint p1, WgsPoint p2) => DegToRad(Math.Abs(p1.lon - p2.lon));

    /// <summary>
    /// Weight of one latitudinal radian at a certain latitude.
    /// </summary>
    /// <param name="_">Latitude in radians.</param>
    private static double LatRadCost(double _) => 1.0;

    /// <summary>
    /// Length of one latitudinal radian at a certai latitude.
    /// </summary>
    /// <param name="_">Latitude in radians.</param>
    /// <returns>Length in meters.</returns>
    private static double LatRadLen(double _) => EarthMeanRadius * LatRadCost(_);

    /// <summary>
    /// Calculate absolute latitude difference in radians.
    /// </summary>
    /// <returns>Difference in radians.</returns>
    private static double LatRadDif(WgsPoint p1, WgsPoint p2) => DegToRad(Math.Abs(p1.lat - p2.lat));

    /// <summary>
    /// Calculate midpoint (use <b>ONLY</b> for small distances).
    /// </summary>
    public static WgsPoint Midpoint(WgsPoint p1, WgsPoint p2)
        => new((p1.lon + p2.lon) / 2.0, (p1.lat + p2.lat) / 2.0);

    /// <summary>
    /// Calculate angle in counter-clockwise direction.
    /// </summary>
    /// <returns>Angle in radians.</returns>
    private static double RotAngle(WgsPoint p1, WgsPoint p2)
    {
        var lat = DegToRad(Midpoint(p1, p2).lat);
        return Math.Atan((DegToRad(p2.lat - p1.lat) * LatRadCost(lat)) / (DegToRad(p2.lon - p1.lon) * LonRadCost(lat)));
    }

    /// <summary>
    /// Calculate great-circle distance (use <b>ONLY</b> for small distances).
    /// </summary>
    /// <returns>Distance in meters.</returns>
    private static double GreatCircleDistance(WgsPoint p1, WgsPoint p2)
    {
        var m_lat = DegToRad(Midpoint(p1, p2).lat);
        var d_lon = LonRadDif(p1, p2) * LonRadLen(m_lat);
        var d_lat = LatRadDif(p1, p2) * LatRadLen(m_lat);

        return Math.Sqrt(d_lon * d_lon + d_lat * d_lat);
    }

    /// <summary>
    /// Construct bounding ellipse given foci and maximum distance. Foci are
    /// guaranteed to be within an ellipse even though the distance is not enough.
    /// </summary>
    /// <param name="f1">Focus 1</param>
    /// <param name="f2">Focus 2</param>
    /// <param name="distance">Maximum walking distance (in meters).</param>
    internal static List<WgsPoint> BoundingEllipse(WgsPoint f1, WgsPoint f2, double distance)
    {
        var m = Spherical.Midpoint(f1, f2);
        var c = Spherical.GreatCircleDistance(f1, f2) / 2.0;

        // construct bounding ellipse with center at the origin

        var a = ((distance > (2.0 * c)) ? distance : (2.0 * c + 250.0)) / 2.0;
        var b = Math.Sqrt(a * a - c * c);

        var factory = new GeometricShapeFactory();
        factory.Envelope = new(-a, +a, -b, +b);
        var e1 = factory.CreateEllipse();

        // rotated, transformed and translated ellipse

        var e2 = new AffineTransformation()
            .Rotate(Spherical.RotAngle(f1, f2))
            .Transform(e1);

        var lr = DegToRad(m.lat);
        var cs = new Coordinate[e2.Coordinates.Length];

        for (var i = 0; i < cs.Length; ++i)
        {
            var pt = e2.Coordinates[i];
            cs[i] = new Coordinate(
                Spherical.RadToDeg(pt.X / Spherical.LonRadLen(lr)),
                Spherical.RadToDeg(pt.Y / Spherical.LatRadLen(lr)));
        }

        var e3 = new AffineTransformation()
            .Translate(m.lon, m.lat)
            .Transform(new Polygon(new LinearRing(cs)));

        return e3.Coordinates.Select(c => new WgsPoint(c.X, c.Y)).ToList();
    }
}
