using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace Knapsack;

public class KnapsackItem : IComparer<KnapsackItem>, IComparable<KnapsackItem>
{
    public int Index { get; set; } // Necesarry due to kattis fuckery
    public int Profit { get; set; }
	public int Weight { get; set; }

	public KnapsackItem(int index, int p, int w)
	{
        Index = index;
        Profit = p;
		Weight = w;
	}

    public int Compare(KnapsackItem? x, KnapsackItem? y)
    {
		double thisBrøk = x!.Profit / x.Weight;
		double thatBrøk = y!.Profit / y.Weight;
		return thatBrøk.CompareTo(thisBrøk);
    }

    public int CompareTo(KnapsackItem? other)
    {
		return this.Compare(this, other);
    }
}

