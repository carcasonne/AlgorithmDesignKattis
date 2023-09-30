// using Knapsack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
string line;

var i = 0;

while ((line = Console.ReadLine()) != null)
{
    var items = new List<KnapsackItem>();
    var lines = line.Trim().Split(' ');

    var C = Int32.Parse(lines[0]);
    var n = Int32.Parse(lines[1]);

    for (int j = 0; j < n; j++)
    {
        var stringItem = Console.ReadLine()!.Trim().Split(' ').Select(Int32.Parse).ToList();
        var item = new KnapsackItem(j, stringItem[0], stringItem[1]);
        items.Add(item);
    }

    var solution = TableCalculation(items, C);
    Console.WriteLine($"{solution.Items.Count}");
    // Also print all items herein
    var toPrint = String.Join(" ", solution.Items.Select(x => x.Index));
    if (toPrint != String.Empty)
        Console.WriteLine(toPrint);
}

TreeNode TableCalculation(List<KnapsackItem> items, int capacity)
{
    int W = capacity + 1;
    int n = items.Count + 1;

    int[,] table = new int[n, W];
    // for (int w = 0; w < W; w++)
    //    table[0, w] = 0;

    for(int i = 1; i <= items.Count; i++)
    {
        var item = items[i-1];
        for (int w = 1; w <= capacity; w++)
        {
            if (item.Weight > w)
            {
                table[i, w] = table[i - 1, w];
            }
            else
            {
                var dropThis = table[i - 1, w];
                var pickThis = item.Profit + table[i - 1, w - item.Weight];
                var opt = Math.Max(dropThis, pickThis);
                table[i, w] = opt;
            }
        }
    }

    // var optimal = table[n - 1, W - 1];

    var solutionItems = new List<KnapsackItem>();
    var ii = items.Count;
    var ww = capacity;

    while(ii > 0)
    {
        var value = table[ii, ww];
        var above = table[ii - 1, ww];

        if(value != above)
        {
            var item = items[ii-1];
            solutionItems.Add(item);
            ww = ww - item.Weight;
        }

        ii = ii - 1;
    }

    return new TreeNode(-1, -1, -1, -1, solutionItems);
}

public class TreeNode
{
    public int Z { get; set; }
    public int Level { get; set; }
    public double U { get; set; }
    public int UsedCapacity { get; set; }
    public List<KnapsackItem> Items { get; set; }

    public TreeNode(int z, int l, double u, int usedC, List<KnapsackItem> items)
    {
        Z = z;
        Level = l;
        U = u;
        UsedCapacity = usedC;
        Items = items;
    }
}

public class KnapsackItem
{
    public int Index { get; set; }
    public int Profit { get; set; }
    public int Weight { get; set; }

    public KnapsackItem(int index, int p, int w)
    {
        Index = index;
        Profit = p;
        Weight = w;
    }
}
