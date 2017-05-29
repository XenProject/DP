using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    private string _platform = "PC";
    private string _genre = "RPG";
    private string _theme = "Adventure";
    private string _name = "Default Game";
    private DateTime _developEndTime;
    private int _boostPoints = 0;
    private int _bugs = 0;
    private int _allPoints = 0;
    private int _allBoostPoints = 0;
    private float _synergy = 0.75f;
    private float _rating = 0;

    public string Platform
    {
        get { return _platform; }
        set { _platform = value; }
    }

    public string Genre
    {
        get { return _genre; }
        set { _genre = value; }
    }

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
        get{ return  _synergy; }
        set{ _synergy = value ; }
    }

    public float Rating
    {
        get { return _rating; }
        set { _rating = value; }
    }
    /****************************************************/

    public void CalculateDevelopTime(TimeSpan developTime)
    {
        _developEndTime = DateTime.Now + developTime;
    }

    public void CalculateSynergy()
    {
        if (_genre == "RPG" && _theme == "Adventure") _synergy = 1;
    }

    public void CalculateRating()
    {
        _rating = (_synergy * ( ( (float)_boostPoints / _allBoostPoints) * 10) ) - (_bugs / 5.0f);
        if (_rating < 1) _rating = 1;
        if (_rating > 10) _rating = 10;
    }
}
