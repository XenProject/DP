using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int N;
    public TileType[] tileTypes;//Типы пола
    public WallType[] wallTypes;//Типы стен
    public GameObject floorsMassive;//Объект на сцене для группировки полов
    public GameObject wallsMassive;//Объект на сцене для группировки стен
    public GameObject borderPrefab;//Префаб границы пола
    public GameObject creationPoint;//Префаб Поинтов
    public GameObject infoPanel;//Панель с информацией о таланте
    public Button learnButton;//Кнопка изучения таланта

    public Developer developer;//Класс разработчика
    public Development development;//Класс разработки

    private bool gameCreation = false;//Создается ли игра в данный момент?
    private CreateManager createManager;//Подключаем скрипт
    private Game developGame;//Разрабатываемая игра в данный момент
    private Talant lastSelectTalant;//Последний нажатый талант
    private int chanceToCreationPoint = 50;//Шанс появления Поинта

    public Talant[] talants;//Массив талантов
	// Use this for initialization
	void Start () {
        string talantsText = Resources.Load("talants", typeof(TextAsset)).ToString();
        createManager = GetComponent<CreateManager>();
        TileMap();
        talants = JsonConvert.DeserializeObject<Talant[]>(talantsText);//Загружаем таланты из json файла
        if (PlayerPrefs.HasKey("Developer"))//Имеется ли сохранение
        {
            Load();//Загрузка
            Messenger.Broadcast<Developer>("Update Game List", developer);
        }
        else
        {
            developer = new Developer();//Новый игрок
        }
        //Обновляем UI
        Messenger.Broadcast<int>("Change Gold", developer.Gold);
        Messenger.Broadcast("Update Stats");
        TalantsUpdate();
    }
	
	// Update is called once per frame
	void Update () {
        /**************Для Тестов**********************/
        if (Input.GetKeyDown(KeyCode.R))
        {
            developer.AddGame(developer.Games[developer.Games.Count-1]);
            Messenger.Broadcast<Developer>("Update Game List", developer);
        }
        /*********************************/
        if (gameCreation)//Если создается игра
        {
            if( development.DevelopmentEndTime > DateTime.Now)//И если время окончания разработки еще не наступило
            {
                Messenger.Broadcast<float>("Change Dev Slider", (float)((createManager.timeToDev - 
                    (development.DevelopmentEndTime - DateTime.Now)).TotalSeconds / createManager.timeToDev.TotalSeconds));
            }
            else
            {//Выпускаем игру
                gameCreation = false;
                Messenger.Broadcast<bool>("Game Creation", gameCreation);
                StopCoroutine("GeneratePoints");
                development.PublishGame(developer, developGame );
                developer.GenreTrees[(int)developGame.genre].CurExp += (int)(120*developGame.Rating);
                developer.GenreTrees[(int)developGame.genre].CheckLevelUp();
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

    //Создаем стены, пол и границы
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
        createManager.OnGameNameChange();
        Messenger.Broadcast<bool>("Game Creation", gameCreation);
        Messenger.Broadcast<float>("Change Dev Slider", 0.0f);
        Messenger.Broadcast<int>("Change Gold", developer.Gold);
        CalculateChanceCreate();
        StartCoroutine("GeneratePoints");
    }

    void Save()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString( "Developer", JsonUtility.ToJson(developer) );
        if (gameCreation)
        {
            PlayerPrefs.SetInt("GameCreation", 1);
            PlayerPrefs.SetString("Development", JsonConvert.SerializeObject(development) );
            PlayerPrefs.SetString("DevelopGame", JsonUtility.ToJson(developGame) );
        }
        else PlayerPrefs.SetInt("GameCreation", 0);

        Dictionary<int, int> id_lvl = new Dictionary<int, int>();
        for (int i = 0; i < talants.Length; i++)
        {
            if (talants[i].CurLvl == 0) continue;
            id_lvl.Add(talants[i].ID, talants[i].CurLvl);
        }
        PlayerPrefs.SetString("Talants", JsonConvert.SerializeObject(id_lvl));
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
        Dictionary<int, int> id_lvl = JsonConvert.DeserializeObject<Dictionary<int, int>>( PlayerPrefs.GetString("Talants") );
        for(int i = 0; i < talants.Length; i++)
        {
            if (id_lvl.ContainsKey(i))
            {
                talants[i].CurLvl = id_lvl[i];
            }
        }
    }

    public void CalculateChanceCreate()
    {
        chanceToCreationPoint = 50;
        for (int i = 0; i < talants.Length; i++)
        {
            if (talants[i].CurLvl == 0) continue;
            for (int j = 0; j < talants[i].Mods.Count; j++)
            {
                if (talants[i].Mods[j].Name == ModifierName.IncreaseChancePoint)
                {
                    chanceToCreationPoint += talants[i].Mods[j].BuffedRatio;
                }
            }
        }
    }

    public void TalantsUpdate()
    {
        for (int i =0;i<talants.Length;i++)
        {
            Talant curTalant = talants[i];
            GameObject treeGo = FindObject(curTalant.TreeName);
            GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Talant"));
            Talant reqTalant = null;
            if(curTalant.ReqTalant != -1) reqTalant = talants[curTalant.ReqTalant];

            go.transform.SetParent(treeGo.transform);
            go.transform.localScale = new Vector3(1,1,1);
            go.transform.name = curTalant.Name;
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(curTalant.IconPath);
            go.GetComponent<Button>().onClick.AddListener(delegate { OnInfoButton(curTalant.ID); });
            if(reqTalant!=null && reqTalant.MaxLvl != reqTalant.CurLvl) go.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f,1.0f);
            if (curTalant.CurLvl == curTalant.MaxLvl) go.GetComponent<Image>().color = new Color(0, 1, 0);
            go.GetComponentInChildren<Text>(true).text = curTalant.CurLvl.ToString();
            if (curTalant.CurLvl > 0) go.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
            curTalant.CalculateMods();
        }
    }

    public void OnInfoButton(int idClickedTalant)
    {
        learnButton.interactable = false;
        Text[] stats = infoPanel.transform.GetChild(1).GetComponentsInChildren<Text>(true);
        foreach(Text stat in stats)
        {
            stat.transform.parent.gameObject.SetActive(false);
        }
        Talant clickedTalant = talants[idClickedTalant], reqTalant = null;
        if(clickedTalant.ReqTalant != -1)
        {
            reqTalant = talants[clickedTalant.ReqTalant];
        }
        lastSelectTalant = clickedTalant;

        infoPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = clickedTalant.Name + "\n\n" + clickedTalant.Description;
        if(clickedTalant.MaxLvl != clickedTalant.CurLvl)
        {
            if (clickedTalant.ReqCode != 0)
            {
                stats[0].text = clickedTalant.ReqCode.ToString();
                stats[0].transform.parent.gameObject.SetActive(true);
            }
            if (clickedTalant.ReqDesign != 0)
            {
                stats[1].text = clickedTalant.ReqDesign.ToString();
                stats[1].transform.parent.gameObject.SetActive(true);
            }
            if (clickedTalant.ReqCreative != 0)
            {
                stats[2].text = clickedTalant.ReqCreative.ToString();
                stats[2].transform.parent.gameObject.SetActive(true);
            }
            if (clickedTalant.ReqSound != 0)
            {
                stats[3].text = clickedTalant.ReqSound.ToString();
                stats[3].transform.parent.gameObject.SetActive(true);
            }
        }
        if( (reqTalant==null || (reqTalant.MaxLvl == reqTalant.CurLvl) ) && clickedTalant.MaxLvl != clickedTalant.CurLvl)
        {
            learnButton.interactable = true;
        }
        infoPanel.SetActive(true);
    }

    public void OnLearnButton()
    {
        lastSelectTalant.LevelUp();
        GameObject lastTalantGo = FindObject(lastSelectTalant.Name);
        lastTalantGo.GetComponentInChildren<Text>(true).text = lastSelectTalant.CurLvl.ToString();
        lastTalantGo.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
        if (lastSelectTalant.MaxLvl == lastSelectTalant.CurLvl)
        {
            lastTalantGo.GetComponent<Image>().color = new Color(0,1,0);
            Talant refreshedTalant = null;
            if (lastSelectTalant.ID != talants.Length - 1) refreshedTalant = talants[lastSelectTalant.ID + 1];
            if (refreshedTalant != null) FindObject(refreshedTalant.Name).GetComponent<Image>().color = new Color(1,1,1,1);
        }
        lastSelectTalant.CalculateMods();
        OnInfoButton(lastSelectTalant.ID);
    }

    public int ReduceDevelopTime()
    {
        int deg = 0;
        for (int i = 0; i < talants.Length; i++)
        {
            if (talants[i].CurLvl == 0) continue;
            for(int j = 0; j < talants[i].Mods.Count; j++)
            {
                if(talants[i].Mods[j].Name == ModifierName.DevelopTime)
                {
                    deg += talants[i].Mods[j].BuffedRatio;
                }
            }
        }
        return deg;
    }

    public GameObject FindObject(string name)
    {
        Transform[] trs = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform t in trs)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }

    IEnumerator GeneratePoints()
    {
        while (true)
        {
            if (UnityEngine.Random.Range(0, 100) < chanceToCreationPoint)
            {
                Instantiate(creationPoint);
                development.AllPoints += 1;
            }
            //Debug.Log(UnityEngine.Random.Range(0, 2));
            yield return new WaitForSeconds(1.0f);
        }
    }
}
