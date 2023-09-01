namespace Week1_ProblemA_StablePerfectMatching.Domain;

public class ProposerVertex : Vertex
{
    public Stack<string> Preferences { get; set; }
    public bool IsExhausted => Preferences.Count == 0;

    public ProposerVertex(string name, IEnumerable<string> preferences) : base(name)
    {
        Preferences = new Stack<string>();
        foreach (var pName in preferences.Reverse())
        {
            Preferences.Push(pName);
        }
    }
}