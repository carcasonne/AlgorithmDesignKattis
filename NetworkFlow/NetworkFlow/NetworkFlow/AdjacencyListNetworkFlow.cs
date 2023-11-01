using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFlow;
public class Graph
{
    public List<Node> Nodes { get; set; } = new List<Node>();
    public List<Edge> Edges { get; set; } = new List<Edge>();
    public int V => Nodes.Count;
    public int E => Edges.Count;

    public void AddNode(Node node) => Nodes.Add(node);
    public Node CreateNewNode()
    {
        var node = new Node(V);
        AddNode(node);
        return node;
    }
    public void AddEdge(int v, int w, int c, char? sign = null, bool isReverse = false)
    {
        var edge = new Edge {
            Id = Edges.Count,
            From = v,
            To = w,
            Capacity = c,
            Flow = 0,
            Sign = sign,
            IsReverse = isReverse,
        };
        var fromNode = Nodes[v];
        fromNode.Edges.Add(edge);
        Edges.Add(edge);
    }
}
 
public class AdjancencyListNetworkFlow
{
    internal Graph Graph { get; set; }
    public Node Source { get; set; }
    public Node Sink { get; set; }

    public AdjancencyListNetworkFlow(Node source, Node sink) 
    {
        Source = source;
        Sink = sink;

        Graph = new Graph();
        Graph.AddNode(Source);
        Graph.AddNode(Sink);
    }

    public (int, Graph) FordFulkerson()
    {
        var rGraph = CreateResidualGraph();
        int maxFlow = 0;

        var (pathExists, parents) = FindAugmentingPath(rGraph);
        while(pathExists)
        {
            // find bottleneck
            var b = int.MaxValue;
            for (var vertex = Sink.Id; vertex != Source.Id; vertex = parents[vertex])
            {
                var parent = rGraph.Nodes[parents[vertex]];
                var edge = parent.GetEdgeTo(vertex);
                b = Math.Min(b, edge.ResidualCapacity);
            }

            // Augment the path
            for (var vertex = Sink.Id; vertex != Source.Id; vertex = parents[vertex])
            {
                var parent = rGraph.Nodes[parents[vertex]];
                var node = rGraph.Nodes[vertex];
                parent.GetEdgeTo(node.Id).Flow += b; // Normal edge
                node.GetEdgeTo(parent.Id).Flow -= b; // Reverse edge
            }

            maxFlow += b;

            // Find next augmenting path
            (pathExists, parents) = FindAugmentingPath(rGraph);
        }

        return (maxFlow, rGraph);
    }

    // Makes a copy of the base graph
    // Since all possible connections are already represented in an adjacency matrix, it is not necessarry to add reverse edges explicitly
    internal Graph CreateResidualGraph()
    {
        var V = Graph.V;
        var rGraph = new Graph();
        rGraph.Nodes = Graph.Nodes.Select(x => new Node(x.Id)).ToList();

        foreach(var node in Graph.Nodes)
        {
            int c;
            foreach(var edge in node.Edges)
            {
                c = edge.Capacity;
                rGraph.AddEdge(node.Id, edge.To, c); // Normal
                rGraph.AddEdge(edge.To, node.Id, 0, null, true); // Reverse
            }
        }

        return rGraph;
    }

    internal (bool, int[]) FindAugmentingPath(Graph rGraph)
    {
        var V = rGraph.V;
        var explored = new bool[V];
        var parents = new int[V];
        explored[Source.Id] = true;
        for(int i = 0; i < V; i++)
            parents[i] = -1;

        // breadth first search
        var queue = new Queue<int>();
        queue.Enqueue(Source.Id);
        while (queue.Count > 0)
        {
            var vertex = rGraph.Nodes[queue.Dequeue()];

            // Look through all connections
            foreach (var edge in vertex.Edges)
            {
                if (!explored[edge.To] && edge.ResidualCapacity > 0)
                {
                    queue.Enqueue(edge.To);
                    parents[edge.To] = vertex.Id;
                    explored[edge.To] = true;

                    //if (edge.To == Sink.Id)
                    //    return(true, parents);
                }
            }
        }

        return (explored[Sink.Id], parents);
    }
}

public class Node
{
    public int Id { get; set; }
    public List<Edge> Edges { get; set; } = new List<Edge>();
    public Tuple<int, int>? ValuePair { get; set; }
    public long? Result { get; set; }

    public Node(int id)
    {
        this.Id = id;
    }
    public Edge GetEdgeTo(int to) => this.Edges.First(x => x.To == to);
}

public class Edge
{
    public int Id { get; set; }
    public int From { get; set; }
    public int To { get; set; }
    public int Capacity { get; set; }
    public int Flow { get; set; } = 0;
    public char? Sign { get; set; }
    public bool IsReverse { get; set; } = false;
    public int ResidualCapacity => Capacity - Flow;
}