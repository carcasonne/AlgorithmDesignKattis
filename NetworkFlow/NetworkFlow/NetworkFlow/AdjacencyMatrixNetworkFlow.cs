using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFlow;

public class AdjancencyMatrixNetworkFlow
{
    public int V { get; set; }
    public int Source { get; set; }
    public int Sink { get; set; }
    public int[,] Graph { get; set; }

    public AdjancencyMatrixNetworkFlow(int noVertices, int source, int sink) 
    {
        V = noVertices;
        Source = source;
        Sink = sink;
        Graph = new int[V, V]; 
    }

    public void AddEdge(int v, int w, int c)
    {
        Graph[v, w] = c;
    }

    public int FordFulkerson()
    {
        var rGraph = CreateResidualGraph();
        int maxFlow = 0;

        var (pathExists, parents) = FindAugmentingPath(rGraph);
        while(pathExists)
        {
            // find bottleneck
            var b = int.MaxValue;
            for (var vertex = Sink; vertex != Source; vertex = parents[vertex])
            {
                int parent = parents[vertex];
                b = Math.Min(b, rGraph[parent, vertex]);
            }

            // Augment the path
            for (var vertex = Sink; vertex != Source; vertex = parents[vertex])
            {
                int parent = parents[vertex];
                rGraph[vertex, parent] += b; // Normal edge
                rGraph[parent, vertex] -= b; // Reverse edge
            }

            maxFlow += b;

            // Find next augmenting path
            (pathExists, parents) = FindAugmentingPath(rGraph);
        }

        return maxFlow;
    }

    // Makes a copy of the base graph
    // Since all possible connections are already represented in an adjacency matrix, it is not necessarry to add reverse edges explicitly
    public int[,] CreateResidualGraph()
    {
        var rGraph = new int[V, V];
        for(int i = 0; i < V; i++)
        {
            for (int j = 0; j < V; j++)
            {
                rGraph[i, j] = Graph[i, j];
            }
        }
        return rGraph;
    }

    // breadth first search
    public (bool, int[]) FindAugmentingPath(int[,] rGraph)
    {
        var explored = new bool[V];
        var parents = new int[V];

        var queue = new Queue<int>();
        queue.Enqueue(Source);
        explored[Source] = true;
        parents[Source] = -1;

        while(queue.Count > 0)
        {
            int vertex = queue.Dequeue();

            // Look through all connections
            for(int i = 0; i < V; i++)
            {
                if (!explored[i] && rGraph[vertex, i] > 0)
                {
                    queue.Enqueue(i);
                    parents[i] = vertex;
                    explored[i] = true;
                }
            }
        }

        return (explored[Sink], parents);
    }
}
