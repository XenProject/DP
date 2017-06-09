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
        Gold = 0;
        Coding = 0;
        Design = 0;
        Creative = 0;
        Sound = 0;
        GenreTrees = GenresInit();
        Games = new List<Game>();
    }
    /******************************************************/
    public void AddGame(Game game, bool sync = true)
    {
        Games.Add(game);
        if (sync)
        {
            WWWForm form = new WWWForm();
            form.AddField("playername", GameObject.Find("SaveObject").GetComponent<SaveObject>().playerName);
            form.AddField("gamename", game.Name);
            form.AddField("developendtime", ConvertToBdFormat(game.DevelopmentEndTime.ToUniversalTime().AddHours(3).ToString() ) );
            form.AddField("id_platform", game.platform.ToString());
            form.AddField("id_genre", game.genre.ToString());
            form.AddField("id_theme", game.theme.ToString());
            WWW www = new WWW("http://192.168.1.35/Game/AddGame.php", form);
        }
    }

    public void PublishGame()
    {
        //
        LastGame().IsCreated = 0;
        Messenger.Broadcast<int>("Game Creation", LastGame().IsCreated);
        LastGame().CalculateRating();
    
        GenreTrees[(int)LastGame().genre].CurExp += (int)(120 * LastGame().Rating);//Отдельный метод!!!!
        GenreTrees[(int)LastGame().genre].CheckLevelUp();

        WWWForm form = new WWWForm();
        form.AddField("playername", GameObject.Find("SaveObject").GetComponent<SaveObject>().playerName);
        form.AddField("rating", LastGame().Rating.ToString() );
        WWW www = new WWW("http://192.168.1.35/Game/PublishGame.php", form);

        Messenger.Broadcast<Developer>("Update Game List", this);
    }

    public Game LastGame()
    {
        if (Games.Count == 0) return null;
        return Games[Games.Count - 1];
    }

    public void SetGenreTrees(GenreTree[] trees)
    {
        int i = 0;
        foreach(var tree in trees)
        {
            tree.Name=(Game.Genre)i;
            i++;
        }
        GenreTrees = trees;
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

    string ConvertToBdFormat(string time)
    {
        string[] date = time.Split(' ');
        string[] tmp = date[0].Split('/');
        return tmp[2] + "-" + tmp[0] + "-" + tmp[1] + " " + date[1];
    }
}