using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Game{

    public Platform platform;
    public Genre genre;
    public Theme theme;
    public string Name;
    public float Synergy;
    public float Rating;

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
    public Game()
    {
        platform = 0;
        genre = 0;
        theme = 0;
        Name = "Default";
        Rating = 0;
        CalculateSynergy();
    }

    public Game(Platform platform, Genre genre, Theme theme, string name)
    {
        this.platform = platform;
        this.genre = genre;
        this.theme = theme;
        Name = name;
        Rating = 0;
        CalculateSynergy();
    }
    /*******************************************/

    public void CalculateSynergy()
    {
        if (genre == Genre.RPG && theme == Theme.Adventure)
        {
            Synergy = 1;
        }
        else
        {
            Synergy = 0.75f;
        }
    }

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
