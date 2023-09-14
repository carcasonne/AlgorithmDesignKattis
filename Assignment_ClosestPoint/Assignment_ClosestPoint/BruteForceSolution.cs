// Made by group containing vson and alsk

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace Assignment_ClosestPoint.BruteForce;

public static class BruteForce
{
    public static void SolveBruteForce()
    {
        var n = Int32.Parse(Console.ReadLine()!);
        var points = new List<(Double, Double)>();

        for(var i = 0; i < n; i++)
        {
            var coords = Console.ReadLine()!.Split(' ');    
            var x = Double.Parse(coords[0]);
            var y = Double.Parse(coords[1]);
            points.Add((x, y));
        }

        ((Double, Double),(Double, Double)) closestPair = default;
        var closestDistance = Double.PositiveInfinity;


        for (int i = 0; i < points.Count; i++)
        {
            var point1 = points[i];
            var x1 = point1.Item1;
            var y1 = point1.Item2;
            for(int j = 0; j < points.Count; j++)
            {
                if(j == i)
                    continue;

                var point2 = points[j];
                var x2 = point2.Item1;
                var y2 = point2.Item2;
                var distance = Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));

                if(distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPair = ((x1, y1),(x2,y2));
                }

            }
        }

        Console.WriteLine($"{closestPair.Item1.Item1} {closestPair.Item1.Item2}");
        Console.WriteLine($"{closestPair.Item2.Item1} {closestPair.Item2.Item2}");
    }
}