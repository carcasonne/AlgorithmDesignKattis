using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NetworkFlow;

public static class ElementaryMath
{
    public static void Solve()
    {
        //var watch = new Stopwatch();
        //watch.Start();
        var n = Int32.Parse(Console.ReadLine()!); // Number of pairs

        var source = new Node(0);
        var sink = new Node(1);

        var pairBase = 2;
        var resultBase = pairBase + n;

        var networkFlowSolver = new AdjancencyListNetworkFlow(source, sink);
        var resultToNode = new Dictionary<long, int>();

        for(int i = 0; i < n; i++)
        {
            var pair = Console.ReadLine()!.Split(' ').Select(Int32.Parse).ToList();
            var a = pair[0];
            var b = pair[1];

            // Add the input pair to the graph
            var node = networkFlowSolver.Graph.CreateNewNode();
            node.ValuePair = new Tuple<int, int>(a, b);

            // Get all nodes this input pair can lead to
            var plusNode  = GetOrCreateNode(resultToNode, a + b, networkFlowSolver);
            var minusNode = GetOrCreateNode(resultToNode, a - b, networkFlowSolver);
            var mulNode   = GetOrCreateNode(resultToNode, a * ((Int64) b), networkFlowSolver);

            // Connect source to node
            networkFlowSolver.Graph.AddEdge(source.Id, node.Id, 1);

            // Connect node to result nodes
            networkFlowSolver.Graph.AddEdge(node.Id, plusNode, 1, '+');
            networkFlowSolver.Graph.AddEdge(node.Id, mulNode, 1, '*');
            networkFlowSolver.Graph.AddEdge(node.Id, minusNode, 1, '-');
        }

        var (flow, rGraph) = networkFlowSolver.FordFulkerson();
        if(flow != n)
        {
            Console.WriteLine("impossible");
        }
        else
        {
            // Due to above if statement, every neighbor must have a flow
            foreach (var edges in source.Edges)
            {
                // Find the edge from pair to result with flow
                var valueNode = networkFlowSolver.Graph.Nodes[edges.To];
                var withFlow = rGraph.Nodes[valueNode.Id].Edges.Single(x => x.Flow > 0);
                var resultNode = networkFlowSolver.Graph.Nodes[withFlow.To];
                var valuePair = networkFlowSolver.Graph.Nodes[valueNode.Id].ValuePair;
                var result    = resultNode.Result;

                var hej = networkFlowSolver.Graph.Edges.First(x => x.From == withFlow.From && x.To == withFlow.To);

                var sign = hej.Sign;
                Console.WriteLine($"{valuePair.Item1} {sign} {valuePair.Item2} = {result}");
            }
        }
        //watch.Stop();
        //Console.WriteLine($"Input size: {n}");
        //Console.WriteLine($"Milliseconds elapsed: {watch.ElapsedMilliseconds}");
    }

    private static int GetOrCreateNode(Dictionary<long, int> dict, long key, AdjancencyListNetworkFlow networkFlowSolver)
    {
        if (dict.ContainsKey(key))
            return dict[key];

        var node = networkFlowSolver.Graph.CreateNewNode();
        node.Result = key;
        // Connect result node to sink
        // Capacity 1, since only 1 of each result is permitted
        networkFlowSolver.Graph.AddEdge(node.Id, networkFlowSolver.Sink.Id, 1);

        dict[key] = node.Id; 
        return node.Id;
    }
}