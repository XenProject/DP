using Newtonsoft.Json;
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
    
    public Developer developer;
    public Development development;

    private bool gameCreation = false;
    private CreateManager createManager;
    private Game developGame;
	// Use this for initialization
	void Start () {
        createManager = GetComponent<CreateManager>();
        TileMap();
        if (PlayerPrefs.HasKey("Developer"))
        {
            Load();
            Messenger.Broadcast<Developer>("Update Game List", developer);
        }
        else
        {
            developer = new Developer();
        }
        Messenger.Broadcast<int>("Change Gold", developer.Gold);
        Messenger.Broadcast("Update Stats");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            developer.AddGame(developer.Games[developer.Games.Count-1]);
            Messenger.Broadcast<Developer>("Update Game List", developer);
        }
        //Возможно перенесу все это в корутин!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!(оптимизация)
        if (gameCreation)
        {
            if( development.DevelopmentEndTime > DateTime.Now)
            {
                Messenger.Broadcast<float>("Change Dev Slider", (float)((createManager.timeToDev - 
                    (development.DevelopmentEndTime - DateTime.Now)).TotalSeconds / createManager.timeToDev.TotalSeconds));
            }
            else
            {
                gameCreation = false;
                Messenger.Broadcast<bool>("Game Creation", gameCreation);
                StopCoroutine("GeneratePoints");
                development.PublishGame(developer, developGame );
                Messenger.Broadcast<Game>("Publish Game", developGame);
                developGame = null;
                development = null;
                //Debug.Log("Game Name: " + CurrentGame.Name + "\nBoost points: " + CurrentGame.BoostPoints + " Bugs: " + CurrentGame.Bugs + "\nAll Boost Points: " + CurrentGame.AllBoostPoints + "\nAll Points: " + CurrentGame.AllPoints);
                //Debug.Log("Rating: " + CurrentGame.Rating);
            }
        }
    }

    void OnApplicationQuit()
    {
        Save();
    }
    
    void OnApplicationPause(bool pause)
    {
        if(pause)
            Save();
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

    public void CreateProject()
    {
        gameCreation = true;
        development = new Development(createManager.timeToDev);
        developGame = new Game(createManager.curPlatform, createManager.curGenre, createManager.curTheme, createManager.gameNameInput.text);
        createManager.gameNameInput.text = "";
        Messenger.Broadcast<bool>("Game Creation", gameCreation);
        Messenger.Broadcast<float>("Change Dev Slider", 0.0f);
        Messenger.Broadcast<int>("Change Gold", developer.Gold);
        StartCoroutine("GeneratePoints");
    }

    void Save()
    {
        PlayerPrefs.SetString( "Developer", JsonUtility.ToJson(developer) );
        if (gameCreation)
        {
            PlayerPrefs.SetInt("GameCreation", 1);
            PlayerPrefs.SetString("Development", JsonConvert.SerializeObject(development) );
            PlayerPrefs.SetString("DevelopGame", JsonUtility.ToJson(developGame) );
        }
        else PlayerPrefs.SetInt("GameCreation", 0);
        Debug.Log(JsonUtility.ToJson(developer));
    }

    void Load()
    {
        Debug.Log("Load!");
        developer = JsonUtility.FromJson<Developer>(PlayerPrefs.GetString("Developer"));
        if(PlayerPrefs.GetInt("GameCreation") == 1)
        {
            developGame = JsonUtility.FromJson<Game>(PlayerPrefs.GetString("DevelopGame"));
            development = JsonConvert.DeserializeObject<Development>(PlayerPrefs.GetString("Development"));
            StartCoroutine("GeneratePoints");
            gameCreation = true;
            Messenger.Broadcast<bool>("Game Creation", gameCreation);
        }
    }

    IEnumerator GeneratePoints()
    {
        while (true)
        {
            if (UnityEngine.Random.Range(0, 2) == 1)
            {
                Instantiate(creationPoint);
                development.AllPoints += 1;
            }
            //Debug.Log(UnityEngine.Random.Range(0, 2));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
