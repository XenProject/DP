using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Talant{
    public int ID;
    public string TreeName;
    public string Name;
    public string IconPath;
    public int ReqCode;
    public int ReqDesign;
    public int ReqCreative;
    public int ReqSound;
    public int ReqTalant;//строка?не использовать? сделать ИД?
    public int ReqGold;
    public List<Modifier> Mods;
    public string Description;
    public int CurLvl;
    public int MaxLvl;

    public Talant()
    {
        ID = 0;
        TreeName = "CodeTree";
        Name = "Default Talant";
        ReqCode = 0;
        ReqCreative = 0;
        ReqDesign = 0;
        ReqSound = 0;
        ReqGold = 0;
        ReqTalant = -1;
        Mods = new List<Modifier>();
        Description = "";
        IconPath = "";
        CurLvl = 0;
        MaxLvl = 1;
    }

    public void LevelUp()
    {
        CurLvl++;
        ReqCode += (int)(ReqCode * 1.3f);
        ReqDesign += (int)(ReqDesign * 1.3f);
        ReqCreative += (int)(ReqCreative * 1.3f);
        ReqSound += (int)(ReqSound * 1.3f);
    }

    public void CalculateMods()
    {
        for(int i = 0; i < Mods.Count; i++)
        {
            Mods[i].CalculateBuffedRatio(CurLvl);
        }
    }
}
