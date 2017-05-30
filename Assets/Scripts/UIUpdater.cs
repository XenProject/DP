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
    // Use this for initialization
    void Awake () {
        Messenger.AddListener<int>("Change Gold", OnChangeGold);
        Messenger.AddListener<float>("Change Dev Slider", OnChangeDevSlider);
        Messenger.AddListener<bool>("Game Creation", GameCreation);
        Messenger.AddListener<TimeSpan>("Change Develop Time", OnChangeTimeOfDevelop);
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
        developSlider.SetActive(isCreation);
        if (!isCreation)
        {
            for (int i = 0; i < creationPointsPanel.transform.childCount; i++)
            {
                DestroyImmediate(creationPointsPanel.transform.GetChild(i).gameObject);
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
}
