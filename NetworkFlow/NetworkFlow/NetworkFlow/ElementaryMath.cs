using System;
using System.Collections.Generic;
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
        Node source, sink;
        var resultToNode = new Dictionary<int, int>();
        var nodeToResult = new Dictionary<int, int>();
        var nodeToContent = new Dictionary<int, Tuple<int, int>>();
        var nodeCount = 0;

        (source, nodeCount) = CreateNode(nodeCount);
        (sink, nodeCount) = CreateNode(nodeCount);

        var graph = new Graph
        {
            Edges = new List<Edge>(),
            Nodes = new List<Node>() { source, sink },
            Source = source,
            Sink = sink,
        };

        var n = Int32.Parse(Console.ReadLine()!);
        for(int i = 0; i < n; i++)
        {
            Node baseNode;
            var ab = Console.ReadLine()!.Split(' ').Select(Int32.Parse).ToList();
            var a = ab[0];
            var b = ab[1];

            (baseNode, nodeCount) = CreateNode(nodeCount);
            graph.Nodes.Add(baseNode);
            nodeToContent[baseNode.Id] = new Tuple<int, int>(a, b);

            var sourceEdge = new Edge
            {
                From = source,
                To = baseNode,
                Capacity = 1,
            };

            AddEdgeToNode(sourceEdge, source, graph);

            Node plusNode, minusNode, mulNode;

            (plusNode, nodeCount)  = GetOrCreate(resultToNode, nodeToResult, a + b, graph, nodeCount);
            (minusNode, nodeCount) = GetOrCreate(resultToNode, nodeToResult, a - b, graph, nodeCount);
            (mulNode, nodeCount)   = GetOrCreate(resultToNode, nodeToResult, a * b, graph, nodeCount);

            var plusEdge = new Edge
            {
                From = baseNode,
                To = plusNode,
                Capacity = 1,
                Content = "+",
            };
            var minusEdge = new Edge
            {
                From = baseNode,
                To = minusNode,
                Capacity = 1,
                Content = "-",
            };
            var mulEdge = new Edge
            {
                From = baseNode,
                To = mulNode,
                Capacity = 1,
                Content = "*",
            };
            AddEdgeToNode(plusEdge, baseNode, graph);
            AddEdgeToNode(minusEdge, baseNode, graph);
            AddEdgeToNode(mulEdge, baseNode, graph);
        }

        var newGraph = graph.FordFulkerson();
        var flow = newGraph.Source.Edges.Where(x => !x.IsReverse).Sum(x => x.Flow);

        if(flow != n)
        {
            Console.WriteLine("impossible");
        }
        else
        {
            foreach(var edge in source.Edges)
            {
                var (a,b) = nodeToContent[edge.To.Id];

                //var flowNode = newGraph.Nodes.Single(x => x.Id == edge.To.Id);
                //var flowTo = flowNode.Edges.Single(x => !x.IsReverse && x.Flow == 1).To;
                //var realNode = edge.To.Edges.Single(x => x.To.Id == flowTo.Id).To;
                var flowNode = newGraph.Nodes.Single(x => x.Id == edge.To.Id);
                var flowEdge = flowNode.Edges.Single(x => !x.IsReverse && x.Flow == 1);
                var result = nodeToResult[flowEdge.To.Id];
                var sign = flowEdge.Content;
                Console.WriteLine($"{a} {sign} {b} = {result}");
            }
        }
    }

    static (Node, int) CreateNode(int nodeCount)
    {
        var node = new Node
        {
            Id = nodeCount,
            Edges = new List<Edge>(),
        };
        nodeCount++;
        return (node, nodeCount);
    }

    static void AddEdgeToNode(Edge edge, Node node, Graph graph)
    {
        node.Edges.Add(edge);
        graph.Edges.Add(edge);
    }

    static (Node, int) GetOrCreate(Dictionary<int, int> dict, Dictionary<int, int> reverseDict, int key, Graph graph, int nodeCount)
    {
        if(dict.ContainsKey(key)) 
        {
            var nodeId = dict[key];
            return (graph.Nodes.Single(x => x.Id == nodeId), nodeCount);
        }

        Node node;
        (node, nodeCount) = CreateNode(nodeCount);
        graph.Nodes.Add(node);
        dict[key] = node.Id;
        reverseDict[node.Id] = key;

        var sinkEdge = new Edge
        {
            From = node,
            To = graph.Sink,
            Capacity = 1,
        };

        AddEdgeToNode(sinkEdge, node, graph);

        return (node, nodeCount);
    }
}
