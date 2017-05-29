using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour {

    public GameObject DeveloperGoldText;
    public GameObject GamePriceText;
    public GameObject DevelopSlider;
    public GameObject NewGameButton;
    public GameObject CreateGameBut;
    public GameObject CreationPointsPanel;
    public GameObject DevTimeText;
	// Use this for initialization
	void Awake () {
        Messenger.AddListener<int>("Change Gold", OnChangeGold);
        Messenger.AddListener<int>("Change Game Price", OnChangeGamePrice);
        Messenger.AddListener<float>("Change Dev Slider", OnChangeDevSlider);
        Messenger.AddListener<bool>("Game Creation", GameCreation);
        Messenger.AddListener<bool>("Interact With Create Button", InteractWithCreateButton);
        Messenger.AddListener<TimeSpan>("Change Develop Time", OnChangeTimeOfDevelop);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnChangeGold(int gold)
    {
        DeveloperGoldText.GetComponent<Text>().text = gold.ToString();
    }

    void OnChangeGamePrice(int price)
    {
        GamePriceText.GetComponent<Text>().text = price.ToString();
    }

    void OnChangeDevSlider(float value)
    {
        DevelopSlider.GetComponent<Slider>().value = value;
        DevelopSlider.GetComponentInChildren<Text>().text = ((int)(DevelopSlider.GetComponent<Slider>().value * 100)).ToString() + "%";
    }

    void GameCreation(bool isCreation)
    {
        NewGameButton.SetActive(!isCreation);
        DevelopSlider.SetActive(isCreation);
        if (!isCreation)
        {
            for (int i = 0; i < CreationPointsPanel.transform.childCount; i++)
            {
                DestroyImmediate(CreationPointsPanel.transform.GetChild(i));
            }
        }
    }

    void InteractWithCreateButton(bool value)
    {
        CreateGameBut.GetComponent<Button>().interactable = value;
    }

    void OnChangeTimeOfDevelop(TimeSpan timeToDevelop)
    {
        DevTimeText.GetComponent<Text>().text = "Development time: ";
        if (timeToDevelop.Hours != 0) DevTimeText.GetComponent<Text>().text += timeToDevelop.Hours.ToString() + "h ";
        if (timeToDevelop.Minutes != 0) DevTimeText.GetComponent<Text>().text += timeToDevelop.Minutes.ToString() + "m ";
        if (timeToDevelop.Seconds != 0) DevTimeText.GetComponent<Text>().text += timeToDevelop.Seconds.ToString() + "s";
    }
}
