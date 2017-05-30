using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game{

    private Platform _platform;
    private Genre _genre;
    private Theme _theme;
    private string _name;
    private float _synergy;
    private float _rating;

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
    /******************Constructor**********************************/
    public Game()
    {
        _platform = 0;
        _genre = 0;
        _theme = 0;
        _name = "Default";
        _rating = 0;
        CalculateSynergy();
    }

    public Game(Platform platform, Genre genre, Theme theme, string name)
    {
        _platform = platform;
        _genre = genre;
        _theme = theme;
        _name = name;
        _rating = 0;
        CalculateSynergy();
    }
    /*******************************************/

    public void CalculateSynergy()
    {
        if (_genre == Genre.RPG && _theme == Theme.Adventure)
        {
            _synergy = 1;
        }
        else
        {
            _synergy = 0.75f;
        }
    }

    public string Info()
    {
        return "Game Name: " + this._name + "\nPlatform: " + this._platform.ToString() + "\nGenre/Theme: " + this._genre.ToString()
            + "/" + this._theme.ToString() + "\nRating: " + this._rating.ToString();
    }
}
