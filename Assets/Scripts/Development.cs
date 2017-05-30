using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Development {

    public DateTime DevelopmentEndTime;
    public int BoostPoints;
    public int Bugs;
    public int AllPoints;
    public int AllBoostPoints;

    /*********************************/
    public Development(TimeSpan timeToDev)
    {
        BoostPoints = 0;
        AllBoostPoints = 0;
        Bugs = 0;
        AllPoints = 0;
        DevelopmentEndTime = DateTime.Now + timeToDev;
        Debug.Log(DevelopmentEndTime);
    }
    public void PublishGame(Developer developer, Game publishedGame)
    {
        CalculateRating(publishedGame);
        developer.AddGame(publishedGame);
    }

    public void CalculateRating(Game game)
    {
        float rating = 0;
        rating = (game.Synergy * (((float)BoostPoints / AllBoostPoints) * 10)) - (Bugs / 3.0f) + UnityEngine.Random.Range(-1, 2);
        if (rating < 1) rating = 1;
        if (rating > 10) rating = 10;
        game.Rating = rating;
    }
}
