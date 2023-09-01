using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace Week1_ProblemA_StablePerfectMatching.Domain;

public class Vertex
{
    public string Name { get; set; }
    public Vertex? Match { get; set; }
    public bool IsMatched => Match != null;
    public bool IsProposer = false;
    public bool IsRejector = false;
    public Stack<Edge> ProposerPreferences { get; set; } = new Stack<Edge>();
    public bool IsExhausted => ProposerPreferences.Count == 0;
    public IEnumerable<string> RejectorPreferences { get; set; } = new List<string>();
    public Vertex(string name)
	{
        Name = name;
        Match = null;
    }

    public void MatchWith(Vertex vertex)
    {
        this.Match = vertex;
        vertex.Match = this;
    }

    public bool PrefersToMatch(Vertex proposer)
    {
        if (Match == null)
        {
            return true;
        }

        var priority = RejectorPreferences.First(x => x == Match.Name || x == proposer.Name);
        return priority == proposer.Name;
    }

    public void DumpInFavorOf(Vertex newMatch)
    {
        if (Match == null)
        {
            Match = newMatch;
            newMatch.Match = this;
            return;
        }

        Match.Match = null;
        this.Match = newMatch;
        newMatch.Match = this;
    }
}
