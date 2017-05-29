using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int N;
    public TileType[] tileTypes;//Типы пола
    public WallType[] wallTypes;//Типы стен
    public GameObject floorsMassive;//Объект на сцене для группировки полов
    public GameObject wallsMassive;//Объект на сцене для группировки стен
    public GameObject borderPrefab;
    public GameObject creationPoint;
    public GameObject GameName;

    public Game CurrentGame;
    public Developer Developer;

    private bool gameCreation = false;
    private TimeSpan timeToDevelop = new TimeSpan(0,0,20);
	// Use this for initialization
	void Start () {
        Developer = new Developer();
        CurrentGame = new Game();
        TileMap();
        Messenger.Broadcast<TimeSpan>("Change Develop Time", timeToDevelop);
        Messenger.Broadcast<int>("Change Game Price", CurrentGame.Price);
    }
	
	// Update is called once per frame
	void Update () {

        if (gameCreation)
        {
            if( CurrentGame.DevelopEndTime > DateTime.Now)
            {
                Messenger.Broadcast<float>("Change Dev Slider", (float)((timeToDevelop - (CurrentGame.DevelopEndTime - DateTime.Now)).TotalSeconds / timeToDevelop.TotalSeconds));
            }
            else
            {
                gameCreation = false;
                Messenger.Broadcast<bool>("Game Creation", gameCreation);
                StopCoroutine("GeneratePoints");
                CurrentGame.CalculateRating();
                GetComponent<Developer>().AddGame(CurrentGame);
                //Debug.Log("Game Name: " + CurrentGame.Name + "\nBoost points: " + CurrentGame.BoostPoints + " Bugs: " + CurrentGame.Bugs + "\nAll Boost Points: " + CurrentGame.AllBoostPoints + "\nAll Points: " + CurrentGame.AllPoints);
                //Debug.Log("Rating: " + CurrentGame.Rating);
            }
        }
    }

    void TileMap()
    {
        int i = 0, j = 0;
        float X = 0.5f, Y = 0.375f;
        for (i = 0; i < N; i++)
        {
            for (j = 0; j < N; j++)
            {
                //Создаем клетку пола в нужной координате и делаем его дочерним
                (Instantiate(tileTypes[0].tileVisualPrefab, new Vector3((j - i) * X, (i + j) * (-Y), 0), Quaternion.identity)).transform.parent = floorsMassive.transform;
                //Аналогично для стен
                //Правые
                if (i == 0)
                {
                    (Instantiate(wallTypes[0].wallVisualPrefab, new Vector3((j * X) + X / 2.0f, (j * -Y) + Y / 2.0f, 0 - (j / 10.0f)), Quaternion.identity)).transform.parent = wallsMassive.transform;
                }
                //Левые
                if (j == 0)
                {
                    (Instantiate(wallTypes[0].wallVisualPrefab, new Vector3(((-i) * X) - X / 2.0f, (i * -Y) + Y / 2.0f, 0 - (j / 10.0f)), new Quaternion(0, 180.0f, 0, 1.0f))).transform.parent = wallsMassive.transform;
                }
                if (i == N - 1)
                {
                    Instantiate(borderPrefab, new Vector3((j - i) * X - X / 2.0f, (i + j) * (-Y) - Y / 2.0f, -0.1f), new Quaternion(0, 180.0f, 0, 1.0f));
                }
                if (j == N - 1)
                {
                    Instantiate(borderPrefab, new Vector3((j - i) * X + X / 2.0f, (i + j) * (-Y) - Y / 2.0f, -0.1f), Quaternion.identity);
                }
            }
        }
    }

    public void ChangeProjectName()
    {
        if (GameName.GetComponent<InputField>().text == "" || Developer.Gold < CurrentGame.Price)
        {
            Messenger.Broadcast<bool>("Interact With Create Button", false);
        }
        else
        {
            Messenger.Broadcast<bool>("Interact With Create Button", true);
        }
    }

    public void CreateProject()
    {
        gameCreation = true;
        CurrentGame = new Game();
        Messenger.Broadcast<bool>("Game Creation", gameCreation);
        Messenger.Broadcast<float>("Change Dev Slider", 0.0f);

        CurrentGame.Name = GameName.GetComponent<InputField>().text;
        CurrentGame.CalculateDevelopTime(timeToDevelop);
        CurrentGame.CalculateSynergy();
        StartCoroutine("GeneratePoints");
        Developer.Gold -= CurrentGame.Price;
        Messenger.Broadcast<int>("Change Gold", Developer.Gold);
    }

    IEnumerator GeneratePoints()
    {
        while (true)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                Instantiate(creationPoint);
                CurrentGame.AllPoints += 1;
            }
            //Debug.Log(UnityEngine.Random.Range(0, 2));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
