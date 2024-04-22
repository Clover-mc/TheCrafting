using System;

namespace Minecraft;

public struct Location
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public float Yaw { get; set; }
    public float Pitch { get; set; }
    
    public World? World { get; set; }

    public readonly double DistanceTo(Location other)
    {
        double x = Math.Pow(other.X - X, 2);
        double y = Math.Pow(other.Y - Y, 2);
        double z = Math.Pow(other.Z - Z, 2);

        return Math.Sqrt(x + y + z);
    }
}
