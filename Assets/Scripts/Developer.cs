using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Developer {

    public int Gold;
    public int Coding;
    public int Design;
    public int Creative;
    public int Sound;
    public GenreTree[] GenreTrees;
    public List<Game> Games;

    /******************************************************/
    public Developer()
    {
        Gold = 1500;
        Coding = 0;
        Design = 0;
        Creative = 0;
        Sound = 0;
        GenreTrees = GenresInit();
        Games = new List<Game>();
    }
    /******************************************************/
    public void AddGame(Game game)
    {
        Games.Add(game);
    }

    private GenreTree[] GenresInit()
    {
        GenreTree[] genres = new GenreTree[Enum.GetValues(typeof(Game.Genre)).Length];
        for (int i = 0; i < genres.Length; i++)
        {
            genres[i] = new GenreTree(i);
        }
        return genres;
    }
}