﻿using System;
namespace Week1_ProblemA_StablePerfectMatching.Domain;

public class Vertex
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Vertex? Match { get; set; }
    public bool IsMatched => Match != null;

    public Vertex(int id, string name)
	{
		Id = id;
		Name = name;
        Match = null;
    }

    public void MatchWithVertex(Vertex vertex)
    {
        Match = vertex;
        vertex.Match = this;
    }
}

