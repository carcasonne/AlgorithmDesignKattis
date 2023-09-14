using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Week1_ProblemA_StablePerfectMatching.Domain;

var firstLine = Console.ReadLine();
var vars = firstLine.Split(" ");

var n = Int32.Parse(vars[0]);
var m = Int32.Parse(vars[1]);
var mCount = 0;

var Vertices = new List<Vertex>();
var Edges = new HashSet<Edge>();

for(var i = 0; i < n; i++)
{
    var line = Console.ReadLine() ?? throw new ArgumentOutOfRangeException("No line could be read");
    var names = line.Split(" ");
    var vertexName = names[0];
    var preferences = names.Skip(1).ToList();

    var vertex = new Vertex(vertexName);
    Vertices.Add(vertex);
    vertex.RejectorPreferences = preferences;

    foreach (var preference in preferences)
    {
        mCount++;
        var edge = new Edge(vertexName, preference);
        Edges.Add(edge);
    }
}

while(Edges.Any())
{
    var proposer = Vertices.FirstOrDefault(v => !v.IsMatched && Edges.Any(x => x.FromName == v.Name));
    if (proposer == null)
        break;

    var edge = Edges.First(x => x.FromName == proposer.Name);
    Edges.Remove(edge);
    var rejector = Vertices.Single(v => v.Name == edge.ToName);


    // If rejector has no matches, she accepts
    if (!rejector.IsMatched)
    {
        proposer.MatchWith(rejector);
    }
    // If rejector prefers the new proposer, she accepts
    else if (rejector.PrefersToMatch(proposer))
    {
        rejector.DumpInFavorOf(proposer);
    }
    // Otherwise she kindly turns down the proposer
}

if (Vertices.Any(v => !v.IsMatched))
{
    Console.WriteLine("-");
}
else
{
    var edges = Vertices.Select(v => new Edge(v.Name, v.Match!.Name)).ToHashSet();
    foreach (var proposer in edges)
    {
        Console.WriteLine($"{proposer.FromName} {proposer.ToName}");
    }
}