using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour {

    public int N;
    public TileType[] tileTypes;//Типы пола
    public WallType[] wallTypes;//Типы стен
    public GameObject floorsMassive;//Объект на сцене для группировки полов
    public GameObject wallsMassive;//Объект на сцене для группировки стен
	// Use this for initialization
	void Start () {
        int i = 0, j = 0;
        float X = 0.5f, Y = 0.375f;
        for (i = 0; i < N; i++)
        {
            for (j=0;j<N;j++)
            {
                //Создаем клетку пола в нужной координате и делаем его дочерним
                (Instantiate(tileTypes[0].tileVisualPrefab, new Vector3( (j-i)*X, (i+j)*(-Y), 1), Quaternion.identity)).transform.parent = floorsMassive.transform;
                //Аналогично для стен
                //Правые
                if (i == 0)
                {
                    (Instantiate(wallTypes[0].wallVisualPrefab, new Vector3((j * X)+X/2.0f, (j * -Y) + Y/2.0f, 1-(j/10.0f) ), Quaternion.identity)).transform.parent= wallsMassive.transform;
                }
                //Левые
                if (j == 0)
                {
                    (Instantiate(wallTypes[0].wallVisualPrefab, new Vector3(( (-i) * X) - X/2.0f, (i * -Y) + Y/2.0f, 1 - (j / 10.0f)), new Quaternion(0,180.0f,0,1.0f))).transform.parent=wallsMassive.transform;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
