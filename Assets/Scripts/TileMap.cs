using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    public int N;
    public TileType[] tileTypes;
	// Use this for initialization
	void Start () {
        int i = 0, j = 0;
        float X = 0.5f, Y = 0.375f;
        for (i = 0; i < N; i++)
        {
            for (j=0;j<N;j++)
            {
                Instantiate(tileTypes[0].tileVisualPrefab, new Vector3( (j-i)*X, (i+j)*(-Y), 1), Quaternion.identity);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
