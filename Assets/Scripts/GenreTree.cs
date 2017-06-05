using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenreTree{
    public Game.Genre Name;
    public int CurExp;
    public int MaxExp;
    public int CurLvl;

    public GenreTree(int genre)
    {
        Name = (Game.Genre)genre;
        CurExp = 0;
        MaxExp = 1000;
        CurLvl = 0;
    }

    public void CheckLevelUp()
    {
        while(CurExp >= MaxExp)
        {
            CurLvl++;
            CurExp -= MaxExp;
            MaxExp = (int)(MaxExp*1.3);
        }
    }
}
