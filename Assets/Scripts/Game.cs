using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Game{

    public int IsCreated;

    public Platform platform;
    public Genre genre;
    public Theme theme;

    public string Name;
    public float Rating;
    public DateTime DevelopmentEndTime;
    public int BoostPoints;
    public int Bugs;

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
    /******************Constructor**********************************/

    public void CalculateDevelopEndTime(TimeSpan time)
    {
        DevelopmentEndTime = DateTime.Now + time;
    }

    public void CalculateRating()
    {
        if (BoostPoints * 0.2f > 3) Rating += 3; else Rating += BoostPoints * 0.2f;
        this.Rating += (5.0f - Bugs/3.0f);
    }

    public Game(Platform platform, Genre genre, Theme theme, string name, TimeSpan timeToDev)
    {
        this.IsCreated = 1;
        this.platform = platform;
        this.genre = genre;
        this.theme = theme;
        this.BoostPoints = 0;
        this.Bugs = 0;
        this.Name = name;
        this.Rating = 0;
        CalculateDevelopEndTime(timeToDev);
    }
    /*******************************************/

    public string Info()
    {
        return "<<" + this.Name + ">>" + "\n" + this.platform.ToString() + "\n" + this.genre.ToString()
            + "/" + this.theme.ToString() + "\n<i>Rating: " + ColorFromRating(this.Rating) + this.Rating.ToString("0.00") + "</color></i>";
    }

    string ColorFromRating(float rating)
    {
        if (rating < 4.0f)
        {
            return "<color=#ff0000ff>";
        }
        if (rating < 7.0f)
        {
            return "<color=#ffff00ff>";
        }
        if (rating <= 10.0f)
        {
            return "<color=#00ff00ff>";
        }
        return "";
    }
}
