using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Developer{

    private int _gold;
    private int _coding;
    private int _design;
    private int _creative;
    public List<Game> _games;

    /***************************************************/
	public int Coding
    {
        get { return _coding; }
        set { _coding = value; }
    }

    public int Design
    {
        get { return _design; }
        set { _design = value; }
    }

    public int Creative
    {
        get { return _creative; }
        set { _creative = value; }
    }

    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }

    public List<Game> Games
    {
        get { return _games; }
        set { _games = value; }
    }
    /******************************************************/
    public Developer()
    {
        _gold = 1500;
        _coding = 0;
        _design = 0;
        _creative = 0;
        _games = new List<Game>();
    }
    /******************************************************/
    public void AddGame(Game game)
    {
        _games.Add(game);
    }
}