using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkFlow;

public static class WaifUntilDark
{
    public static void AdjacencyMatrixSolve()
    {
        var nmp = Console.ReadLine()!.Split(' ');
        var n = Int32.Parse(nmp[0]); // Number of children
        var m = Int32.Parse(nmp[1]); // Number of toys
        var p = Int32.Parse(nmp[2]); // Number of toy categories

        var source = 0;
        var sink = 1;

        var childBase = 2;
        var toyBase = childBase + n;
        var categoryBase = toyBase + m;
        var nodeCount = categoryBase + p;

        var graph = new AdjancencyMatrixNetworkFlow(nodeCount, source, sink);

        // Children
        // format: <n> <t_1> ... <t_n>
        for (int i = 0; i < n; i++)
        {
            // Each child should be connected to the source with capacity 1
            var childIndex = childBase + i;
            graph.AddEdge(source, childIndex, 1);

            // Connect each child to all toys they like. Capacity 1, since they cannot pick the same toy twice (and are anyhow bounded by the earlier capacity of 1)
            var line = Console.ReadLine()!.Split(' ').Select(Int32.Parse).ToList();
            for(int j = 0; j < line[0]; j++)
            {
                var toyIndex = toyBase + line[1 + j] - 1; // Parsed toy indexes are 1-index-based
                graph.AddEdge(childIndex, toyIndex, 1);
            }

        }

        // Toy categories
        // format: <l> <t_1> ... <t_l> <r>
        var toyIsCategorised = new bool[m];
        for(int i = 0; i < p; i++)
        {
            var categoryIndex = categoryBase + i;

            // Each toy should be connected to its category, with capacity 1 (a toy cannot be chosen by 2 different children)
            var line = Console.ReadLine()!.Split(' ').Select(Int32.Parse).ToList();
            var l = line[0];
            var r = line[l + 1]; // Capacity from category to sink
            for (int j = 0; j < l; j++)
            {
                var toyNo = line[1 + j];
                var toyIndex = toyBase + toyNo - 1; // Parsed toy indexes are 1-index-based 
                graph.AddEdge(toyIndex, categoryIndex, 1);
                toyIsCategorised[toyNo - 1] = true;
            }

            // Category connected to sink with capacity r
            graph.AddEdge(categoryIndex, sink, r);
        }

        // Add direct edge from toy to sink if it is not categorized
        for(int i = 0; i < m; i++)
        {
            if (toyIsCategorised[i])
                continue;

            var toyIndex = toyBase + i;
            graph.AddEdge(toyIndex, sink, 1);
        }

        var flow = graph.FordFulkerson();
        Console.WriteLine(flow);
    }

}
