using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
namespace Knapsack;

public class TreeNode : IComparable<TreeNode>
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

    public int CompareTo(TreeNode? other) =>
        this.U.CompareTo(other!.U);
}

