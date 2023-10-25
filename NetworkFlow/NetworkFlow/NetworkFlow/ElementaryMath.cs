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
            networkFlowSolver.Graph.AddEdge(node.Id, plusNode, 1);
            networkFlowSolver.Graph.AddEdge(node.Id, mulNode, 1);
            networkFlowSolver.Graph.AddEdge(node.Id, minusNode, 1);
            networkFlowSolver.Graph.Sign[new Tuple<int, int>(node.Id, plusNode)]  = '+';
            networkFlowSolver.Graph.Sign[new Tuple<int, int>(node.Id, mulNode)]   = '*';
            networkFlowSolver.Graph.Sign[new Tuple<int, int>(node.Id, minusNode)] = '-';
        }

        var (flow, rGraph) = networkFlowSolver.FordFulkerson();
        if(flow != n)
        {
            Console.WriteLine("impossible");
        }
        else
        {
            // Due to above if statement, every neighbor must have a flow
            foreach(var valuePairNode in source.Neighbors)
            {
                // Find the edge from pair to result with flow
                Tuple<int, int> correctEdge = null;
                foreach(var possibleResultNode in networkFlowSolver.Graph.Nodes[valuePairNode].Neighbors)
                {
                    // Flow is stored in the residualGraph
                    var pair = new Tuple<int, int>(valuePairNode, possibleResultNode);
                    var resultFlow = rGraph.Flow[pair];
                    if(resultFlow == 0)
                    {
                        correctEdge = pair;
                        break;
                    }
                }

                if (correctEdge == null)
                    throw new Exception("FUCK");

                var valuePair = networkFlowSolver.Graph.Nodes[valuePairNode].ValuePair;
                var result    = networkFlowSolver.Graph.Nodes[correctEdge.Item2].Result;
                var sign      = networkFlowSolver.Graph.Sign[correctEdge];
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