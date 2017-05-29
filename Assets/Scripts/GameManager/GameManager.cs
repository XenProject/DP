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

    private Game game;
    private GameObject gameName;
    private GameObject createGameBut;
    private bool gameCreation = false;
    private GameObject developSlider;
    private GameObject newGameButton;
    private TimeSpan timeToDevelop = new TimeSpan(0,0,20);
	// Use this for initialization
	void Start () {
        game = GetComponent<Game>();
        TileMap();
	}
	
	// Update is called once per frame
	void Update () {
        if (gameCreation)
        {
            if( game.DevelopEndTime > DateTime.Now)
            {
                developSlider.GetComponent<Slider>().value = (float)( (timeToDevelop - (game.DevelopEndTime - DateTime.Now) ).TotalSeconds / timeToDevelop.TotalSeconds);
                developSlider.GetComponentInChildren<Text>().text = ((int)(developSlider.GetComponent<Slider>().value * 100)).ToString() + "%";
            }
            else
            {
                GameObject go = GameObject.Find("CreationPoints");
                for(int  i = 0; i < go.transform.childCount; i++)
                {
                    DestroyImmediate(go.transform.GetChild(i));
                }
                gameCreation = false;
                newGameButton.SetActive(true);
                developSlider.SetActive(false);
                StopCoroutine("GeneratePoints");
                game.CalculateRating();
                Debug.Log("Game Name: " + game.Name + "\nBoost points: " + game.BoostPoints + " Bugs: " + game.Bugs + "\nAll Boost Points: " + game.AllBoostPoints + "\nAll Points: " + game.AllPoints);
                Debug.Log("Rating: " + game.Rating);
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
        if(gameName==null) gameName = GameObject.Find("GameName");
        if(createGameBut==null) createGameBut = GameObject.Find("CreateGame");
        if (gameName.GetComponent<InputField>().text == "")
        {
            createGameBut.GetComponent<Button>().interactable = false;
        }
        else
        {
            createGameBut.GetComponent<Button>().interactable = true;
        }
    }

    public void CreateProject()
    {
        newGameButton = GameObject.Find("NewGameButton");
        newGameButton.SetActive(false);
        developSlider = GameObject.Find("DevelopSlider");
        gameCreation = true;
        game.Name = gameName.GetComponent<InputField>().text;
        game.CalculateDevelopTime(timeToDevelop);
        developSlider.GetComponent<Slider>().value = 0.0f;
        game.CalculateSynergy();
        StartCoroutine("GeneratePoints");
    }

    IEnumerator GeneratePoints()
    {
        while (true)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                Instantiate(creationPoint);
                game.AllPoints += 1;
            }
            //Debug.Log(UnityEngine.Random.Range(0, 2));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
