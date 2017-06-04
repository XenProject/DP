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
    public GameObject borderPrefab;
    public GameObject creationPoint;
    public GameObject infoPanel;
    public Button learnButton;

    public Developer developer;
    public Development development;

    private bool gameCreation = false;
    private CreateManager createManager;
    private Game developGame;
    private Talant lastSelectTalant;

    public TreeTalant[] trees;
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
        trees = TreeInit();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            developer.AddGame(developer.Games[developer.Games.Count-1]);
            Messenger.Broadcast<Developer>("Update Game List", developer);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            File.WriteAllText(Application.dataPath + "/test.json", JsonConvert.SerializeObject(trees));
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
        createManager.OnGameNameChange();
        Messenger.Broadcast<bool>("Game Creation", gameCreation);
        Messenger.Broadcast<float>("Change Dev Slider", 0.0f);
        Messenger.Broadcast<int>("Change Gold", developer.Gold);
        StartCoroutine("GeneratePoints");
    }

    void Save()
    {
        PlayerPrefs.SetString("Trees", JsonConvert.SerializeObject(trees));
        PlayerPrefs.SetString( "Developer", JsonUtility.ToJson(developer) );
        if (gameCreation)
        {
            PlayerPrefs.SetInt("GameCreation", 1);
            PlayerPrefs.SetString("Development", JsonConvert.SerializeObject(development) );
            PlayerPrefs.SetString("DevelopGame", JsonUtility.ToJson(developGame) );
        }
        else PlayerPrefs.SetInt("GameCreation", 0);
        //Debug.Log(JsonUtility.ToJson(developer));
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

    public TreeTalant[] TreeInit()
    {
        TreeTalant[] tt;
        if (!PlayerPrefs.HasKey("Trees"))
        {
            tt = JsonConvert.DeserializeObject<TreeTalant[]>(File.ReadAllText(Application.dataPath + "/talants.json"));
        }
        else
        {
            tt = JsonConvert.DeserializeObject<TreeTalant[]>(PlayerPrefs.GetString("Trees"));
        }
        for (int i =0;i<tt.Length;i++)
        {
            GameObject treeGo = FindObject(tt[i].Name);
            for(int j = 0; j < tt[i].Talants.Count; j++)
            {
                Talant curTalant = tt[i].Talants[j];
                GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Talant"));
                go.transform.SetParent(treeGo.transform);
                go.transform.localScale = new Vector3(1,1,1);
                go.transform.name = curTalant.Name;
                go.GetComponent<Image>().sprite = Resources.Load<Sprite>(curTalant.IconPath);
                go.GetComponent<Button>().onClick.AddListener(delegate { OnInfoButton(go.transform.name); });
                if(j!=0 && tt[i].Talants[j-1].MaxLvl != tt[i].Talants[j - 1].CurLvl)
                {
                    go.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f,1.0f);
                }
                if (curTalant.CurLvl == curTalant.MaxLvl)
                {
                    go.GetComponent<Image>().color = new Color(0, 1, 0);
                }
                go.GetComponentInChildren<Text>(true).text = curTalant.CurLvl.ToString();
                if (curTalant.CurLvl > 0) go.GetComponentInChildren<Text>(true).gameObject.SetActive(true);
                curTalant.CalculateMods();
            }
        }
        return tt;
    }

    public void OnInfoButton(string nameClickedTalant)
    {
        learnButton.interactable = false;
        Text[] stats = infoPanel.transform.GetChild(1).GetComponentsInChildren<Text>(true);
        foreach(Text stat in stats)
        {
            stat.transform.parent.gameObject.SetActive(false);
        }
        Talant curTalant = null, reqTalant = null;
        for (int i = 0; i < trees.Length; i++)
        {
            for(int j = 0; j < trees[i].Talants.Count; j++)
            {
                curTalant = trees[i].Talants[j];
                if (j != 0) reqTalant = trees[i].Talants[j - 1];
                if (curTalant.Name == nameClickedTalant) break;
            }
        }
        lastSelectTalant = curTalant;
        infoPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = nameClickedTalant + "\n\n" + curTalant.Description;
        if(curTalant.MaxLvl != curTalant.CurLvl)
        {
            if (curTalant.ReqCode != 0)
            {
                stats[1].text = curTalant.ReqCode.ToString();
                stats[1].transform.parent.gameObject.SetActive(true);
            }
            if (curTalant.ReqDesign != 0)
            {
                stats[2].text = curTalant.ReqDesign.ToString();
                stats[2].transform.parent.gameObject.SetActive(true);
            }
            if (curTalant.ReqCreative != 0)
            {
                stats[3].text = curTalant.ReqCreative.ToString();
                stats[3].transform.parent.gameObject.SetActive(true);
            }
            if (curTalant.ReqSound != 0)
            {
                stats[4].text = curTalant.ReqSound.ToString();
                stats[4].transform.parent.gameObject.SetActive(true);
            }
        }
        if( (reqTalant==null || reqTalant.MaxLvl == reqTalant.CurLvl) && curTalant.MaxLvl != curTalant.CurLvl)
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
            for (int i = 0; i < trees.Length; i++)
            {
                for (int j = 0; j < trees[i].Talants.Count; j++)
                {
                    if (j != trees[i].Talants.Count - 1 && lastSelectTalant == trees[i].Talants[j]) refreshedTalant = trees[i].Talants[j + 1];
                }
            }
            if (refreshedTalant != null) FindObject(refreshedTalant.Name).GetComponent<Image>().color = new Color(1,1,1,1);
        }
        lastSelectTalant.CalculateMods();
        OnInfoButton(lastSelectTalant.Name);
    }

    public int ReduceDevelopTime()
    {
        int deg = 0;
        for (int i = 0; i < trees.Length; i++)
        {
            for (int j = 0; j < trees[i].Talants.Count; j++)
            {
                if (trees[i].Talants[j].CurLvl == 0) continue;
                for(int k = 0; k < trees[i].Talants[j].Mods.Count; k++)
                {
                    if(trees[i].Talants[j].Mods[k].Name == ModifierName.DevelopTime)
                    {
                        deg += trees[i].Talants[j].Mods[k].BuffedRatio;
                    }
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
