using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NetworkFlow;

//var graph = GraphFactory.GetSlidesGraph();
//var newNetwork = graph.FordFulkerson();

//var totalFlow = newNetwork.Source.Edges.Sum(x => x.Flow);
//Console.WriteLine($"Total flow: {totalFlow}");
//Console.WriteLine("Done");

//ElementaryMath.Solve();

//return;

var nmp = Console.ReadLine()!.Split(' ');
var n = Int32.Parse(nmp[0]); // Number of children
var m = Int32.Parse(nmp[1]); // Number of toys
var p = Int32.Parse(nmp[2]); // Number of toy categories

var nodeCount = 0;

var source = CreateNode();
var sink = CreateNode();

Graph graph = new Graph
{
    Edges = new List<Edge>(),
    Nodes = new List<Node>(),
    Source = source,
    Sink = sink,
};

graph.Nodes.Add(source);
graph.Nodes.Add(sink);

// Toy nodes
var toyNodes = new List<Node>();
for (int i = 0; i < m; i++)
{
    var node = CreateNode();
    toyNodes.Add(node);
}

// Children nodes
var childrenNodes = new List<Node>();
for (int i = 0; i < n; i++)
{
    var line = Console.ReadLine()!.Split(' ').Select(Int32.Parse).ToList();
    
    var node = CreateNode();
    var edge = new Edge
    {
        From = source,
        To = node,
        Capacity = 1,
    };
    AddEdgeToNodeAndGraph(edge, source, graph);
    childrenNodes.Add(node);

    // Make an edge from this child to given toy
    for (int j = 1; j <= line[0]; j++)
    {
        var nEdge = new Edge
        {
            From = node,
            To = toyNodes[line[j] - 1],
            Capacity = 1,
        };
        AddEdgeToNodeAndGraph(nEdge, node, graph);
    }
}

graph.Nodes.AddRange(toyNodes);
graph.Nodes.AddRange(childrenNodes);

// Toy categories
for (int i = 0; i < p; i++)
{
    var line = Console.ReadLine()!.Split(' ').Select(Int32.Parse).ToList();

    var node = CreateNode();
    graph.Nodes.Add(node);
    var nCats = line[0];
    var capacity = line[nCats + 1];
    // Connect toy node to category node
    for (int j = 1; j <= nCats; j++)
    {
        var toyNode = toyNodes[line[j] - 1];
        var edge = new Edge
        {
            From = toyNode,
            To = node,
            Capacity = 1,
        };
        AddEdgeToNodeAndGraph(edge, toyNode, graph);
    }
    // Connect category node to sink
    var sinkEdge = new Edge
    {
        From = node,
        To = sink,
        Capacity = capacity,
    };
    AddEdgeToNodeAndGraph(sinkEdge, node, graph);
}

// Undefined categories can be picked unlimited times, but is practically bounded by the number of toys in the category
var undefinedToys = toyNodes.Where(x => x.Edges.Count == 0);
foreach(var toyNode in undefinedToys)
{
    // Connect category node to sink
    var sinkEdge = new Edge
    {
        From = toyNode,
        To = sink,
        Capacity = 1,
    };
    AddEdgeToNodeAndGraph(sinkEdge, toyNode, graph);
}

var newGraph = graph.FordFulkerson();
var flow = newGraph.Source.Edges.Where(x => !x.IsReverse).Sum(x => x.Flow);


Console.WriteLine(flow);

Node CreateNode()
{
    var newNode = new Node
    {
        Id = nodeCount,
        Edges = new List<Edge>(),
    };
    nodeCount++;
    return newNode;
}

void AddEdgeToNodeAndGraph(Edge edge, Node node, Graph lgraph)
{
    node.Edges.Add(edge);
    lgraph.Edges.Add(edge);
}



