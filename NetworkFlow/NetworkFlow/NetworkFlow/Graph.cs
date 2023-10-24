using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkFlow;

public class Graph : INetworkFlowable
{
    public required Node Source { get; set; }
    public required Node Sink { get; set; }
    public required List<Node> Nodes { get; set; }
    public required List<Edge> Edges { get; set; }

    public Graph CreateResidualNetwork()
    {
        var nodes = new List<Node>();
        var edges = new List<Edge>();

        foreach (var node in Nodes)
        {
            var newNode = new Node
            {
                Id = node.Id,
                Edges = new List<Edge>(),
            };
            nodes.Add(newNode);
        }

        foreach (var edge in Edges)
        {
            var newTo = nodes.Single(x => x.Id == edge.To.Id);
            var newFrom = nodes.Single(x => x.Id == edge.From.Id);

            var newEdge = new Edge
            {
                To = newTo,
                From = newFrom,
                Capacity = edge.Capacity,
                Content = edge.Content,
            };
            newFrom.Edges.Add(newEdge);

            var reverseEdge = new Edge
            {
                To = newFrom,
                From = newTo,
                Capacity = 0,
                IsReverse = true,
                Content = edge.Content,
            };
            newTo.Edges.Add(reverseEdge);

            edges.Add(newEdge);
            edges.Add(reverseEdge);
        }

        return new Graph
        { 
            Source = nodes.Single(x => x.Id == Source.Id),
            Sink = nodes.Single(x => x.Id == Sink.Id),
            Edges = edges,
            Nodes = nodes,
        };
    }

    public IEnumerable<Edge>? FindAugmentingPath(Graph graph)
    {
        // simple bfs
        var queue = new Queue<Node>();
        var explored = new bool[graph.Nodes.Count()];
        var parents  = new Node[graph.Nodes.Count()];
        explored[graph.Source.Id] = true;
        queue.Enqueue(graph.Source);

        while(queue.Count > 0)
        {
            var node = queue.Dequeue();
            if (node == graph.Sink)
                break;
            foreach (var edge in node.Edges)
            {
                if (edge.ResidualCapacity == 0)
                    continue;

                var neighbor = edge.To;
                if (!explored[neighbor.Id])
                {
                    explored[neighbor.Id] = true;
                    parents[neighbor.Id] = edge.From;
                    queue.Enqueue(neighbor);
                }
            }
        }

        // If no path was found
        if (parents[graph.Sink.Id] == null)
        {
            return null;
        }

        // Convert path to edges
        Node to = graph.Sink;
        Node from = null;
        var path = new List<Edge>();
        while(from != graph.Source)
        {
            from = parents[to.Id];
            var edge = from.Edges.Single(x => x.To.Id == to.Id); // If this fails, something is very wrong
            path.Add(edge);
            to = from;
        }

        return path;
    }

    // TODO: If algorithm is slow, look at linq statements in here
    public void Augment(IEnumerable<Edge> path)
    {
        var b = path.MinBy(x => x.ResidualCapacity).ResidualCapacity; // List will never be empty, so this is fine
        foreach (var edge in path)
        {
            if (!edge.IsReverse)
            {
                edge.Flow += b;
            }
            else
            {
                edge.Flow -= b;
            }
        }
    }

    public Graph FordFulkerson()
    {
        var residual = CreateResidualNetwork();
        var path = residual.FindAugmentingPath(residual);
        while(path != null)
        {
            Augment(path);
            // Find next augmenting path
            path = residual.FindAugmentingPath(residual);
        }

        return residual;
    }
}
