
using System;
using FlaxEngine;

public static class Randomize
{
    static Random random = new Random();



    /// <summary>
    /// Return a random integer value between or equals to the Min and Max values
    /// </summary>
    public static int Range(int min, int max)
         => random.Next(min, max);


    /// <summary>
    /// Return a random float value between or equals to the Min and Max values
    /// </summary>
    public static float Range(float min, float max)
    {
        float rand = (float)random.NextDouble();
        return (float)(random.NextDouble() * (max - min)) + min;
    }


    /// <summary>
    /// Generates a random value inside a unit circle
    /// </summary>
    public static Vector2 RandomInsideUnitCircle()
    {
        // Generate random angle
        float angle = (float)(2 * Math.PI * random.NextDouble());

        // Calculate the random point inside the unit circle
        float x = (float)(Math.Cos(angle));
        float y = (float)(Math.Sin(angle));

        return new Vector2(x, y);
    }



    /// <summary>
    /// Generates a random point on the surface of a unit circle
    /// </summary>
    public static Vector2 RandomOutsideUnitCircle()
    {
        // Generate random angle
        float angle = (float)(2 * Math.PI * random.NextDouble());

        // Calculate the point on the unit circle
        float x = (float)(Math.Cos(angle));
        float y = (float)(Math.Sin(angle));

        return new Vector2(x, y);
    }



    /// <summary>
    /// Generates a random Vector3 point inside a unit sphere
    /// </summary>
    public static Vector3 RandomInsideUnitSphere()
    {
        // Generate random spherical coordinates
        float phi = (float)(2 * Math.PI * random.NextDouble());
        float theta = (float)(Math.Acos(2 * random.NextDouble() - 1));

        // Convert spherical coordinates to Cartesian coordinates
        float x = (float)(Math.Sin(theta) * Math.Cos(phi));
        float y = (float)(Math.Sin(theta) * Math.Sin(phi));
        float z = (float)(Math.Cos(theta));

        // Scale the point to be inside the unit sphere
        float radius = (float)random.NextDouble();
        Vector3 randomPoint = new Vector3(x, y, z) * radius;

        return randomPoint;
    }


    /// <summary>
    /// Generates a random point on the surface of a unit sphere
    /// </summary>
    public static Vector3 RandomOutsideUnitSphere()
    {
        // Generate random spherical coordinates
        float phi = (float)(2 * Math.PI * random.NextDouble());
        float theta = (float)(Math.Acos(2 * random.NextDouble() - 1));

        // Convert spherical coordinates to Cartesian coordinates
        float x = (float)(Math.Sin(theta) * Math.Cos(phi));
        float y = (float)(Math.Sin(theta) * Math.Sin(phi));
        float z = (float)(Math.Cos(theta));

        return new Vector3(x, y, z);
    }
}
