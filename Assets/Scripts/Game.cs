using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game{

    private Platform _platform;
    private Genre _genre;
    private Theme _theme;
    private string _name;
    private DateTime _developEndTime;
    private int _boostPoints;
    private int _bugs;
    private int _allPoints;
    private int _allBoostPoints;
    private float _synergy;
    private float _rating;
    private int _price;

    /********************************************/
    public enum Platform
    {
        PC,
        PS,
        Xbox
    }

    public enum Genre
    {
        RPG,
        Action,
        Strategy,
        Simulation,
        Shooter,
        Sport,
        Horror
    }

    public enum Theme
    {
        Adventure,
        Fantasy,
        Football,
        Economy,
        Vampire
    }
    /*****************************************/
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

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

    public DateTime DevelopEndTime
    {
        get { return _developEndTime; }
        set { _developEndTime = value; }
    }

    public float Synergy
    {
        get { return _synergy; }
        set { _synergy = value; }
    }

    public float Rating
    {
        get { return _rating; }
        set { _rating = value; }
    }

    public int Price
    {
        get { return _price; }
        set { _price = value; }
    }
    /******************Constructor**********************************/
    public Game()
    {
        _platform = 0;
        _genre = 0;
        _theme = 0;
        _name = "Default";
        _developEndTime = new DateTime(2050,5,31);
        _boostPoints = 0;
        _bugs = 0;
        _allBoostPoints = 0;
        _allPoints = 0;
        _synergy = 0.75f;
        _rating = 0;
        _price = 700;
    }
    /*******************************************/

    public void CalculateDevelopTime(TimeSpan developTime)
    {
        _developEndTime = DateTime.Now + developTime;
    }

    public void CalculateSynergy()
    {
        if (_genre == Genre.RPG && _theme == Theme.Adventure) _synergy = 1;
    }

    public void CalculateRating()
    {
        _rating = (_synergy * ( ( (float)_boostPoints / _allBoostPoints) * 10) ) - (_bugs / 3.0f) + UnityEngine.Random.Range(-1,2);
        if (_rating < 1) _rating = 1;
        if (_rating > 10) _rating = 10;
    }
}
