using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsertUser : MonoBehaviour {

    public InputField loginField;
    public InputField passwordField;
    public InputField playerNameField;
    public Button confirmButton;
    public GameObject loginPanel;
    public Text info;

    private string curLogin;
    private string curPassword;
    private string curPlayerName;
    private const string CreateUserURL = "http://192.168.1.35/Game/InsertUser.php";
    // Use this for initialization
    void Start () {
        loginField.onValueChanged.AddListener(delegate { OnLoginChanged(); });
        passwordField.onValueChanged.AddListener(delegate { OnPasswordChanged(); });
        playerNameField.onValueChanged.AddListener(delegate { OnPlayerNameChanged(); });
        confirmButton.onClick.AddListener(delegate { ConfirmButtonPressed(); });
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void OnLoginChanged()
    {
        curLogin = loginField.text;
    }

    public void OnPasswordChanged()
    {
        curPassword = passwordField.text;
    }

    public void OnPlayerNameChanged()
    {
        curPlayerName = playerNameField.text;
    }

    public void ConfirmButtonPressed()
    {
        StartCoroutine( CreateUser(curLogin, curPassword, curPlayerName) );
    }

    public IEnumerator CreateUser(string login, string password, string playerName)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);
        form.AddField("playername", playerName);

        WWW www = new WWW(CreateUserURL, form);
        yield return www;
        string[] tmp = www.text.Split('*');
        switch(tmp[0]){
            case "OK":
                info.gameObject.SetActive(false);
                Debug.Log(www.text);
                break;
            case "ERR":
                info.gameObject.SetActive(true);
                switch (tmp[1])
                {
                    case "login":
                        info.text = "This login is already taken!";
                        break;
                    case "playername":
                        info.text = "This player name is already taken!";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
    }
}
