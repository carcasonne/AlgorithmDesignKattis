using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Week1_ProblemA_StablePerfectMatching.Domain;

var firstLine = Console.ReadLine();
var vars = firstLine.ToCharArray();

var n = vars[0] - '0';
var m = vars[2] - '0';

var Proposers = new List<ProposerVertex>();
var Rejectors = new List<RejectorVertex>();

var halfN = n / 2; // cache

// Read n lines of vertices + its preferences
for(var i = 0; i < n; i++)
{
    var line = Console.ReadLine() ?? throw new ArgumentOutOfRangeException("No line could be read");
    var names = line.Split(" ");
    var vertexName = names[0];
    var preferences = names.Skip(1).ToList();
    var isProposer = i < halfN;

    if (isProposer)
    {
        var vertex = new ProposerVertex(i, vertexName, preferences);
        Proposers.Add(vertex);
    }
    else
    {
        var vertex = new RejectorVertex(i, vertexName, preferences);
        Rejectors.Add(vertex);
    }
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
