// Made by group containing vson and alsk

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

var n = Int32.Parse(Console.ReadLine()!);
var points = new List<Coord>();

for(var i = 0; i < n; i++)
{
    var coords = Console.ReadLine()!.Split(' ');    
    var x = Double.Parse(coords[0]);
    var y = Double.Parse(coords[1]);
    points.Add(new Coord(x, y));
}

var closestPair = ClosestPair(points.ToList());


Console.WriteLine($"{closestPair.Item1.x} {closestPair.Item1.y}");
Console.WriteLine($"{closestPair.Item2.x} {closestPair.Item2.y}");

(Coord, Coord) ClosestPair(List<Coord> inputList)
{
    // List of 2: The two elements are the cloests (and only) pair
    // List of 3: Would split list in 2,1. Cannot find shortest pair with 1 element
    if(inputList.Count <= 3)
    {
        var toReturn = BruteForce(inputList);
        return toReturn;
    }

    var sortedPoints = inputList.OrderBy(p => p.x).ToList();

    var m = inputList.Count;
    var median = m / 2;
    var medianCoord = sortedPoints[median];
    var leftPoints  = sortedPoints.Take(median).ToList();
    var rightPoints = sortedPoints.Skip(median).ToList();

    var closestPairLeft = ClosestPair(leftPoints);
    var closestPairRight = ClosestPair(rightPoints);
    var distanceLeft = EuclideanDistance(   closestPairLeft.Item1.x, 
                                            closestPairLeft.Item1.y, 
                                            closestPairLeft.Item2.x, 
                                            closestPairLeft.Item2.y);
    var distanceRight = EuclideanDistance(  closestPairRight.Item1.x, 
                                            closestPairRight.Item1.y, 
                                            closestPairRight.Item2.x, 
                                            closestPairRight.Item2.y);
    var minDistance = distanceRight;
    var minPair = closestPairRight;
    if(distanceLeft < distanceRight)
    {
        minDistance = distanceLeft;
        minPair = closestPairLeft;
    }

    // These are constant, so 'cache' them.
    var minX = medianCoord.x - minDistance;
    var maxX = medianCoord.x + minDistance;

    // Prune
    var stripList = inputList.Where(p => Math.Abs(medianCoord.x - p.x) <= minDistance);
    var sortedByYCoordList = stripList.OrderBy(p => p.y).ToList();

    int yCount = sortedByYCoordList.Count;
    for (var i = 0; i < yCount; i++)
    {
        var point1 = sortedByYCoordList[i];

        for (var j = i + 1; j < yCount; j++)
        {
            var point2 = sortedByYCoordList[j];

            if (point2.y - point1.y >= minDistance)
                break;

            var dist = EuclideanDistance(point1.x, point1.y, point2.x, point2.y);
            if(dist < minDistance)
            {
                minDistance = dist;
                minPair = (point1, point2);
            }
        }
    }

    return minPair;
}

double EuclideanDistance(Double x1, Double y1, Double x2, Double y2)
{
    return Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
}

(Coord, Coord) BruteForce(List<Coord> input)
{
    (Coord, Coord) closestPair = default;
    var closestDistance = Double.PositiveInfinity;

    for (int i = 0; i < input.Count; i++)
    {
        var point1 = input[i];
        var x1 = point1.x;
        var y1 = point1.y;
        for(int j = i + 1; j < input.Count; j++)
        {
            var point2 = input[j];
            var x2 = point2.x;
            var y2 = point2.y;
            var distance = EuclideanDistance(x1, y1, x2, y2);

            if(distance < closestDistance)
            {
                closestDistance = distance;
                closestPair = (point1, point2);
            }

        }
    }
    
    return closestPair;
}

public class Coord
{
    public Double x;
    public Double y;

    public Coord(Double x, Double y)
    {
        this.x = x;
        this.y = y;
    }
}