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
    public List<Game> Games;

    /******************************************************/
    public Developer()
    {
        Gold = 1500;
        Coding = 0;
        Design = 0;
        Creative = 0;
        Sound = 0;
        Games = new List<Game>();
    }
    /******************************************************/
    public void AddGame(Game game)
    {
        Games.Add(game);
    }
}