using System;
namespace Week1_ProblemA_StablePerfectMatching.Domain;

public class Vertex
{
    public int Id { get; set; }
    public string Name { get; set; }
	public bool IsProposer { get; set; }
	public IEnumerable<string> Preferences { get; set; }
	
	public Vertex(int id, string name, IEnumerable<string> preferences, bool isProposer)
	{
		Id = id;
		Name = name;
		IsProposer = isProposer;
		Preferences = preferences;
	}
}

