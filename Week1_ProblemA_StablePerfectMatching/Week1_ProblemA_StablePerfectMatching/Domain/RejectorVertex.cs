namespace Week1_ProblemA_StablePerfectMatching.Domain;

public class RejectorVertex : Vertex
{
    public List<string> Preferences { get; set; }
    public RejectorVertex(int id, string name, IEnumerable<string> preferences) : base(id, name)
    {
        Preferences = preferences.ToList();
    }

    public bool PrefersToMatch(ProposerVertex proposer)
    {
        if (Match == null)
        {
            return true;
        }
        var priority = Preferences.First(x => x == Match.Name || x == proposer.Name);
        return priority == proposer.Name;
    }

    public void DumpInFavorOf(ProposerVertex newMatch)
    {
        if (Match == null)
        {
            Match = newMatch;
            newMatch.Match = this;
            return;
        }

        Match.Match = null;
        Match = newMatch;
        newMatch.Match = this;
    }
}