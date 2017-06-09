using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateManager : MonoBehaviour {

    public int goldToDevelopment;
    public TimeSpan timeToDev = new TimeSpan(0, 0, 30);

    public Dropdown platform;
    public Dropdown genre;
    public Dropdown theme;
    public InputField gameNameInput;
    public Button createBut;

    public Text gamePriceText;
    public Game.Platform curPlatform;
    public Game.Genre curGenre;
    public Game.Theme curTheme;

    private GameManager gameManager;
    // Use this for initialization
    void Start () {
        gameManager = GetComponent<GameManager>();
        PlatformsInit();
        GenresInit();
        ThemesInit();
        OnPlatformChange();//Для инициализации золота на проект
        OnGenreChange();
        OnThemeChange();
        gamePriceText.text = goldToDevelopment.ToString();

        platform.onValueChanged.AddListener(delegate { OnPlatformChange(); });
        genre.onValueChanged.AddListener(delegate { OnGenreChange(); });
        theme.onValueChanged.AddListener(delegate { OnThemeChange(); });
        gameNameInput.onValueChanged.AddListener(delegate { OnGameNameChange(); });
        createBut.onClick.AddListener(delegate { OnGameCreationClicked(); });

        Messenger.Broadcast<TimeSpan>("Change Develop Time", timeToDev);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DegOfTime()
    {
        timeToDev = new TimeSpan(0,0,30);
        int deg = gameManager.ReduceDevelopTime();
        timeToDev = TimeSpan.FromSeconds(timeToDev.TotalSeconds - ( timeToDev.TotalSeconds * (deg / 100.0f) ));
        Messenger.Broadcast<TimeSpan>("Change Develop Time", timeToDev);
    }

    public void OnPlatformChange()
    {
        curPlatform = (Game.Platform)platform.value;
        switch (curPlatform)
        {
            case Game.Platform.PC:
                goldToDevelopment = 800;
                break;
            case Game.Platform.PS:
                goldToDevelopment = 550;
                break;
            case Game.Platform.Xbox:
                goldToDevelopment = 400;
                break;
            default:
                goldToDevelopment = 700;
                break;
        }
        gamePriceText.text = goldToDevelopment.ToString(); 
        OnGameNameChange();
    }

    public void OnGenreChange()
    {
        curGenre = (Game.Genre)genre.value;
    }

    public void OnThemeChange()
    {
        curTheme = (Game.Theme)theme.value;
    }

    public void OnGameNameChange()
    {
        if (gameNameInput.text == "" || gameManager.developer.Gold < goldToDevelopment)
        {
            createBut.interactable = false;
        }
        else
        {
            createBut.interactable = true;
        }
    }

    public void OnGameCreationClicked()
    {
        Messenger.Broadcast<int>("Change Gold", -goldToDevelopment);
        gameManager.developer.AddGame(new Game(curPlatform, curGenre, curTheme, gameNameInput.text, timeToDev));
        gameNameInput.text = "";
        OnGameNameChange();
        gameManager.CreateProject();
    }

    void PlatformsInit()
    {
        platform.options.Clear();
        foreach (var p in Enum.GetValues(typeof(Game.Platform)))
        {
            platform.options.Add(new Dropdown.OptionData(p.ToString()));
        }
        platform.RefreshShownValue();
    }

    void GenresInit()
    {
        genre.options.Clear();
        foreach (var g in Enum.GetValues(typeof(Game.Genre)))
        {
            genre.options.Add(new Dropdown.OptionData(g.ToString()));
        }
        genre.RefreshShownValue();
    }

    void ThemesInit()
    {
        theme.options.Clear();
        foreach (var t in Enum.GetValues(typeof(Game.Theme)))
        {
            theme.options.Add(new Dropdown.OptionData(t.ToString()));
        }
        theme.RefreshShownValue();
    }
}