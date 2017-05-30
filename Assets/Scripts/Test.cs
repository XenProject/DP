using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test {

    public string Name;
    public int Gold;
    public float[] Numbers;
    public List<Game> Games;

    public Test()
    {
        Name = "Default";
        Gold = 1500;
        Numbers = new float[]{ 1.0f, 2.0f,3.0f};
        Games = new List<Game>();
    }
}
