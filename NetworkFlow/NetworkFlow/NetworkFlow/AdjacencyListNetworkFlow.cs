using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFlow;
public class Graph
{
    public List<Node> Nodes { get; set; } = new List<Node>();
    public Dictionary<Tuple<int, int>, int> Flow { get; set; } = new Dictionary<Tuple<int, int>, int>();
    public Dictionary<Tuple<int, int>, char> Sign { get; set; } = new Dictionary<Tuple<int, int>, char>();
    public int V => Nodes.Count;

    public void AddNode(Node node) => Nodes.Add(node);
    public Node CreateNewNode()
    {
        var node = new Node(V);
        AddNode(node);
        return node;
    }
    public void AddEdge(int v, int w, int c, char? sign = null)
    {
        var edge = new Tuple<int, int>(v, w);
        Flow[edge] = c;
        Nodes[v].Neighbors.Add(w);
        if (sign != null)
            Sign[edge] = sign.Value;
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
                int parent = parents[vertex];
                b = Math.Min(b, rGraph.Flow[new Tuple<int, int>(parent, vertex)]);
            }

            // Augment the path
            for (var vertex = Sink.Id; vertex != Source.Id; vertex = parents[vertex])
            {
                int parent = parents[vertex];
                rGraph.Flow[new Tuple<int, int>(vertex, parent)] += b; // Normal edge
                rGraph.Flow[new Tuple<int, int>(parent, vertex)] -= b; // Reverse edge
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
            foreach(var neighbor in node.Neighbors)
            {
                var edge = new Tuple<int, int>(node.Id, neighbor);
                c = Graph.Flow[edge];

                rGraph.AddEdge(node.Id, neighbor, c);
                rGraph.AddEdge(neighbor, node.Id, 0);
            }
        }

        return rGraph;
    }

    internal (bool, int[]) FindAugmentingPath(Graph rGraph)
    {
        var V = Graph.V;
        var explored = new bool[V];
        var parents = new int[V];
        explored[Source.Id] = true;
        parents[Source.Id] = -1;


        // breadth first search
        var queue = new Queue<int>();
        queue.Enqueue(Source.Id);
        while (queue.Count > 0)
        {
            var vertex = Graph.Nodes[queue.Dequeue()];

            // Look through all connections
            foreach (var neighbor in vertex.Neighbors)
            {
                var flow = rGraph.Flow[new Tuple<int, int>(vertex.Id, neighbor)];
                if (!explored[neighbor] && flow != 0)
                {
                    queue.Enqueue(neighbor);
                    parents[neighbor] = vertex.Id;
                    explored[neighbor] = true;

                    if (neighbor == Sink.Id)
                        return(true, parents);
                }
            }
        }

        //depth first search
        //var stack = new Stack<int>();
        //stack.Push(Source.Id);

        //while(stack.Count > 0)
        //{
        //    var vertex = Graph.Nodes[stack.Pop()];

        //    if (!explored[vertex.Id])
        //    {
        //        explored[]
        //    }
        //}

        return (explored[Sink.Id], parents);
    }
}

public class Node
{
    public int Id { get; set; }
    public List<int> Neighbors { get; set; } = new List<int>();
    public Tuple<int, int>? ValuePair { get; set; }
    public long? Result { get; set; }

    public Node(int id)
    {
        this.Id = id;
    }
}