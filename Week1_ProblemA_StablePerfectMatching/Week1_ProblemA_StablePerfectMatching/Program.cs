using System;
using Week1_ProblemA_StablePerfectMatching.Domain;

var n = Console.ReadKey().KeyChar;
Console.ReadKey(); // Discard space
var m = Console.ReadKey().KeyChar;

var vertices = new List<Vertex>();
var halfN = n / 2; // cache

// Read n lines of vertices + its preferences
for(int i = 0; i < n; i++)
{
    var line = Console.ReadLine() ?? throw new ArgumentOutOfRangeException("No line could be read");
    var names = line.Split(" ");
    var vertexName = names[0];
    var preferences = names.Skip(1).ToList();
    var isProposer = i + 1 < halfN;

    var vertex = new Vertex(i, vertexName, preferences, isProposer);
    vertices.Add(vertex);
}

