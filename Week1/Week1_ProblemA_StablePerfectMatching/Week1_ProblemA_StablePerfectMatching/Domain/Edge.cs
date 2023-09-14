using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace Week1_ProblemA_StablePerfectMatching.Domain;

public class Edge : IEquatable<Edge>
{
    public readonly string FromName;
    public readonly string ToName;

    public Edge(string fromName, string toName)
    {
        FromName = fromName;
        ToName = toName;
    }

    public bool Equals(Object obj) => 
        obj is Edge ? Equals((Edge) obj) : false;

    public bool Equals(Edge? other)
    {
        if (other == null)
        {
            return false;
        }

        var sameOrder = this.FromName.Equals(other.FromName)
                            && this.ToName.Equals(other.ToName);

        var otherOrder = this.FromName.Equals(other.ToName)
                            && this.ToName.Equals(other.FromName);

        return sameOrder || otherOrder;
    }

    public override int GetHashCode()
    {
        return FromName.GetHashCode() ^ ToName.GetHashCode();
    }
}