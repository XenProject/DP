using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour {

    public Text developerGoldText;
    public Text devTimeText;

    public GameObject creationPointsPanel;
    public GameObject newGameButton;
    public GameObject developSlider;
    public GameObject gameCreationPanel;

    public GameObject gameListPanel;
    public GameObject gamePrefab;
    public GameObject talantTree2;
    public Image genreIcon;
    public Slider expSlider;

    private GameManager gm;
    public Text codeText;
    public Text designText;
    public Text creativeText;
    public Text soundText;
    // Use this for initialization
    void Awake () {
        Messenger.AddListener<int>("Change Gold", OnChangeGold);
        Messenger.AddListener<float>("Change Dev Slider", OnChangeDevSlider);
        Messenger.AddListener<bool>("Game Creation", GameCreation);
        Messenger.AddListener<TimeSpan>("Change Develop Time", OnChangeTimeOfDevelop);
        Messenger.AddListener<Game>("Publish Game", OnPublishGame);
        Messenger.AddListener<Developer>("Update Game List", UpdateGameList);
        Messenger.AddListener("Update Stats", UpdateStats);
    }

    void Start()
    {
        gm = gameObject.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnChangeGold(int gold)
    {
        developerGoldText.text = gold.ToString();
    }

    void OnChangeDevSlider(float value)
    {
        developSlider.GetComponent<Slider>().value = value;
        developSlider.GetComponentInChildren<Text>().text = ((int)(developSlider.GetComponent<Slider>().value * 100)).ToString() + "%";
    }

    void GameCreation(bool isCreation)
    {
        newGameButton.SetActive(!isCreation);
        developSlider.transform.parent.gameObject.SetActive(isCreation);
        if (!isCreation)
        {
            while (creationPointsPanel.transform.childCount != 0)
            {
                DestroyImmediate(creationPointsPanel.transform.GetChild(0).gameObject);
            }
        }
        else
        {
            gameCreationPanel.SetActive(false);
        }
    }

    void OnChangeTimeOfDevelop(TimeSpan timeToDevelop)
    {
        devTimeText.text = "Development time: ";
        if (timeToDevelop.Hours != 0) devTimeText.text += timeToDevelop.Hours.ToString() + "h ";
        if (timeToDevelop.Minutes != 0) devTimeText.text += timeToDevelop.Minutes.ToString() + "m ";
        if (timeToDevelop.Seconds != 0) devTimeText.text += timeToDevelop.Seconds.ToString() + "s";
    }

    void OnPublishGame(Game game)
    {
        GameObject go = Instantiate(gamePrefab);
        go.transform.SetParent(gameListPanel.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponentInChildren<Text>().text = game.Info();
        go.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Platforms/" + game.platform.ToString());
    }

    void UpdateGameList(Developer developer)
    {
        while (gameListPanel.transform.childCount != 0){ 
            DestroyImmediate( gameListPanel.transform.GetChild(0).gameObject);
        }
        for(int i = 0; i < developer.Games.Count; i++)
        {
            GameObject go = Instantiate(gamePrefab);
            go.transform.SetParent(gameListPanel.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.GetComponentInChildren<Text>().text = developer.Games[i].Info();
            go.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Platforms/" + developer.Games[i].platform.ToString());
        }
    }

    void UpdateStats()
    {
        if(gm == null) gm = gameObject.GetComponent<GameManager>();
        codeText.text = gm.developer.Coding.ToString();
        designText.text = gm.developer.Design.ToString();
        creativeText.text = gm.developer.Creative.ToString();
        soundText.text = gm.developer.Sound.ToString();
    }

    public void ChangeStateGameList()
    {
        gameListPanel.transform.parent.parent.gameObject.SetActive(!(gameListPanel.transform.parent.parent.gameObject.activeInHierarchy));
    }

    public void ChangeStateGO(GameObject go)
    {
        go.SetActive(!(go.activeInHierarchy));
    }

    public void ActiveGenre(int genre)
    {
        GenreTree genreTree = gm.developer.GenreTrees[genre];
        for (int i=0;i< Enum.GetValues(typeof(Game.Genre)).Length; i++)
        {
            talantTree2.transform.GetChild(i).gameObject.SetActive(false);
            if(i==genre) talantTree2.transform.GetChild(i).gameObject.SetActive(true);
        }
        talantTree2.SetActive(true);
        genreIcon.sprite = Resources.Load<Sprite>("Sprites/UI/Genres/" + ((Game.Genre)genre).ToString() );
        genreIcon.GetComponentInChildren<Text>().text = genreTree.CurLvl.ToString(); 
        expSlider.value = (float)genreTree.CurExp / genreTree.MaxExp;
        expSlider.GetComponentInChildren<Text>().text = genreTree.CurExp.ToString() + "/" + genreTree.MaxExp.ToString();

    }
}
