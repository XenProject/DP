using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Modifier{
    public ModifierName Name;
    public int BaseRatio;
    public int RatioPerLvl;
    public int BuffedRatio;

    public void CalculateBuffedRatio(int lvl)
    {
        if (lvl == 0)
        {
            BuffedRatio = 0;
        }
        else
        {
            BuffedRatio = BaseRatio + (RatioPerLvl * (lvl - 1));
        }
    }
}

public enum ModifierName
{
    DevelopTime,//0
    CostOfGold,//1
    Income,//2
    Rating//3
}
