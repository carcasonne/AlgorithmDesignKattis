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

var Proposers = new List<ProposerVertex>();
var Rejectors = new List<RejectorVertex>();
var Edges = new HashSet<Edge>();

// Read n lines of vertices + its preferences

// todo plan:
// Det her er fucking scuffed pt
// Løsningen (måske): Lav edges indtil Edges.Count == m, men når m er overskredet ved vi, at der ikke er flere edges
// Alle linjer, som kommer efter m er overskredet, derfor må resten af linjerne være Rejectors

for(var i = 0; i < n; i++)
{
    var line = Console.ReadLine() ?? throw new ArgumentOutOfRangeException("No line could be read");
    var names = line.Split(" ");
    var vertexName = names[0];
    var preferences = names.Skip(1).ToList();

    foreach (var preference in preferences)
    {
        var edge = new Edge(vertexName, preference);

        if (Edges.Contains(edge))
        {
            var rejector = new RejectorVertex(vertexName, preferences);
            if(!Rejectors.Any(x => x.Name == vertexName))
                Rejectors.Add(rejector);
        }

        Edges.Add(edge);
    }
}

var proposerNameToVertex = new Dictionary<string, ProposerVertex>();

var reverseEdges = Edges.Reverse();

foreach (var edge in reverseEdges) 
{
    ProposerVertex proposer;

    var proposerName = edge.FromName;
    if (proposerNameToVertex.ContainsKey(proposerName))
    {
        proposer = proposerNameToVertex[proposerName];
        proposer.Preferences.Push(edge.ToName);
        continue;
    }

    proposer = new ProposerVertex(proposerName, new List<string>(new [] {edge.ToName}));
    Proposers.Add(proposer);
    proposerNameToVertex[proposerName] = proposer;
}

while (true)
{
    var proposer = Proposers.FirstOrDefault(x => !x.IsMatched && !x.IsExhausted);
    if (proposer == null)
        break;

    var rejectorName = proposer.Preferences.Pop();
    var rejector = Rejectors.FirstOrDefault(x => x.Name == rejectorName);
    if (rejector == null)
        break;

    // If rejector has no matches, she acceps
    if (!rejector.IsMatched)
    {
        proposer.MatchWithVertex(rejector);
    }
    // If rejector prefers the new proposer, she accepts
    else if (rejector.PrefersToMatch(proposer))
    {
        rejector.DumpInFavorOf(proposer);
    }
    // Otherwise she kindly turns down the proposer
}

if (Proposers.Any(p => !p.IsMatched))
{
    Console.WriteLine("-");
}
else
{
    foreach (var proposer in Proposers)
    {
        Console.WriteLine($"{proposer.Name} {proposer.Match!.Name}");
    }
}
