using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUser : MonoBehaviour {

    public InputField loginField;
    public InputField passwordField;
    public Text info;
    public Button enterButton;

    private string curLogin;
    private string curPassword;

    private const string LoginUserURL = "http://192.168.1.35/Game/Login.php";

    // Use this for initialization
    void Start () {
        loginField.onValueChanged.AddListener(delegate { OnLoginChanged(); });
        passwordField.onValueChanged.AddListener(delegate { OnPasswordChanged(); });
        enterButton.onClick.AddListener(delegate { EnterButtonPressed(); });
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

    public void EnterButtonPressed()
    {
        StartCoroutine( OnLoginUser(curLogin,curPassword) );
    }

    IEnumerator OnLoginUser(string login, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", password);

        WWW www = new WWW(LoginUserURL, form);
        yield return www;
        string[] tmp = www.text.Split('*');
        if (tmp[0] == "OK")
        {
            info.gameObject.SetActive(false);
            GameObject.Find("SaveObject").GetComponent<SaveObject>().playerName = tmp[1];
            SceneManager.LoadScene(1);
        }
        else
        {
            info.gameObject.SetActive(true);
            passwordField.text = "";
            info.text = tmp[1];
        }
    }
}
