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

public static class GraphFactory
{
    public static Graph GetWebsiteGraph()
    {
        return null;
    }
    public static Graph GetSlidesGraph()
    {
        var source = new Node
        {
            Id = 0,
            Edges = new List<Edge>()
        };
        var sink = new Node
        {
            Id = 1,
            Edges = new List<Edge>()
        };
        var node1 = new Node
        {
            Id = 2,
            Edges = new List<Edge>()
        };
        var node2 = new Node
        {
            Id = 3,
            Edges = new List<Edge>()
        };
        var node3 = new Node
        {
            Id = 4,
            Edges = new List<Edge>()
        };
        var node4 = new Node
        {
            Id = 5,
            Edges = new List<Edge>()
        };

        var nodes = new List<Node>
        {
            source, sink,
            node1, node2, node3, node4,
        };

        var edge0 = new Edge
        {
            From = source,
            To = node1,
            Capacity = 10,
        };
        var edge1 = new Edge
        {
            From = source,
            To = node2,
            Capacity = 10,
        };
        source.Edges.Add(edge0);
        source.Edges.Add(edge1);

        var edge2 = new Edge
        {
            From = node1,
            To = node2,
            Capacity = 2,
        };
        var edge3 = new Edge
        {
            From = node1,
            To = node3,
            Capacity = 4,
        };
        var edge4 = new Edge
        {
            From = node1,
            To = node4,
            Capacity = 8,
        };
        node1.Edges.Add(edge2);
        node1.Edges.Add(edge3);
        node1.Edges.Add(edge4);

        var edge5 = new Edge
        {
            From = node2,
            To = node4,
            Capacity = 9,
        };
        node2.Edges.Add(edge4);

        var edge6 = new Edge
        {
            From = node3,
            To = sink,
            Capacity = 10,
        };
        node3.Edges.Add(edge6);

        var edge7 = new Edge
        {
            From = node4,
            To = node3,
            Capacity = 6,
        };
        var edge8 = new Edge
        {
            From = node4,
            To = sink,
            Capacity = 10,
        };
        node4.Edges.Add(edge7);
        node4.Edges.Add(edge8);

        var edges = new List<Edge>
        {
            edge0, edge1, edge2, edge3, edge4, edge5, edge6, edge7, edge8
        };

        return new Graph
        {
            Source = source,
            Sink = sink,
            Nodes = nodes,
            Edges = edges,
        };
    }
}
