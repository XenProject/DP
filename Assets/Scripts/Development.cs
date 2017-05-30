using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Development {

    private DateTime _developmentEndTime;
    private int _boostPoints;
    private int _bugs;
    private int _allPoints;
    private int _allBoostPoints;

    /******************************/
    public int BoostPoints
    {
        get { return _boostPoints; }
        set { _boostPoints = value; }
    }

    public int Bugs
    {
        get { return _bugs; }
        set { _bugs = value; }
    }

    public int AllPoints
    {
        get { return _allPoints; }
        set { _allPoints = value; }
    }

    public int AllBoostPoints
    {
        get { return _allBoostPoints; }
        set { _allBoostPoints = value; }
    }

    public DateTime DevelopmentEndTime
    {
        get { return _developmentEndTime; }
        set { _developmentEndTime = value; }
    }
    /*********************************/
    public Development(TimeSpan timeToDev)
    {
        _boostPoints = 0;
        _allBoostPoints = 0;
        _bugs = 0;
        _allPoints = 0;
        _developmentEndTime = DateTime.Now + timeToDev;
        Debug.Log(_developmentEndTime);
    }
    public void PublishGame(Developer developer, Game publishedGame)
    {
        CalculateRating(publishedGame);
        developer.AddGame(publishedGame);
    }

    public void CalculateRating(Game game)
    {
        float rating = 0;
        rating = (game.Synergy * (((float)_boostPoints / _allBoostPoints) * 10)) - (_bugs / 3.0f) + UnityEngine.Random.Range(-1, 2);
        if (rating < 1) rating = 1;
        if (rating > 10) rating = 10;
        game.Rating = rating;
    }
}
