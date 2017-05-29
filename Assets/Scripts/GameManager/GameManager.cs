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
    public GameObject devTimeText;
    public GameObject GoldText;
    public GameObject GamePriceText;

    public Game CurrentGame;
    public Developer Developer;

    private GameObject gameName;
    private GameObject createGameBut;
    private bool gameCreation = false;
    private GameObject developSlider;
    private GameObject newGameButton;
    private TimeSpan timeToDevelop = new TimeSpan(0,0,20);
	// Use this for initialization
	void Start () {
        Developer = new Developer();
        CurrentGame = new Game();
        TileMap();
        devTimeText.GetComponent<Text>().text = "Development time: ";
        if (timeToDevelop.Hours != 0) devTimeText.GetComponent<Text>().text += timeToDevelop.Hours.ToString() + "h ";
        if (timeToDevelop.Minutes != 0) devTimeText.GetComponent<Text>().text += timeToDevelop.Minutes.ToString() + "m ";
        if (timeToDevelop.Seconds != 0) devTimeText.GetComponent<Text>().text += timeToDevelop.Seconds.ToString() + "s";
        GoldText.GetComponent<Text>().text = Developer.Gold.ToString();
        GamePriceText.GetComponent<Text>().text = CurrentGame.Price.ToString();
    }
	
	// Update is called once per frame
	void Update () {

        if (gameCreation)
        {
            if( CurrentGame.DevelopEndTime > DateTime.Now)
            {
                developSlider.GetComponent<Slider>().value = (float)( (timeToDevelop - (CurrentGame.DevelopEndTime - DateTime.Now) ).TotalSeconds / timeToDevelop.TotalSeconds);
                developSlider.GetComponentInChildren<Text>().text = ((int)(developSlider.GetComponent<Slider>().value * 100)).ToString() + "%";
            }
            else
            {
                gameCreation = false;
                GameObject go = GameObject.Find("CreationPoints");
                for(int  i = 0; i < go.transform.childCount; i++)
                {
                    DestroyImmediate(go.transform.GetChild(i));
                }
                newGameButton.SetActive(true);
                developSlider.SetActive(false);
                StopCoroutine("GeneratePoints");
                CurrentGame.CalculateRating();
                GetComponent<Developer>().AddGame(CurrentGame);
                Debug.Log("Game Name: " + CurrentGame.Name + "\nBoost points: " + CurrentGame.BoostPoints + " Bugs: " + CurrentGame.Bugs + "\nAll Boost Points: " + CurrentGame.AllBoostPoints + "\nAll Points: " + CurrentGame.AllPoints);
                Debug.Log("Rating: " + CurrentGame.Rating);
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
        if (gameName.GetComponent<InputField>().text == "" || Developer.Gold < CurrentGame.Price)
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
        CurrentGame = new Game();
        newGameButton = GameObject.Find("NewGameButton");
        newGameButton.SetActive(false);
        developSlider = GameObject.Find("DevelopSlider");
        developSlider.GetComponent<Slider>().value = 0.0f;
        CurrentGame.Name = gameName.GetComponent<InputField>().text;
        CurrentGame.CalculateDevelopTime(timeToDevelop);
        CurrentGame.CalculateSynergy();
        gameCreation = true;
        StartCoroutine("GeneratePoints");
        Developer.Gold -= CurrentGame.Price;
        GoldText.GetComponent<Text>().text = Developer.Gold.ToString();
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
