using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class SaveObject : MonoBehaviour {

    public string playerName;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        //StartCoroutine(test());
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /*IEnumerator test()
    {
        WWW www = new WWW("http://192.168.1.35/Game/test.php");
        yield return www;

        DateTime data = DateTime.Parse(www.text);
        Debug.Log( DateTime.Now.ToUniversalTime().AddHours(3) - data );
    }*/
    /*IEnumerator test()
    {
        WWWForm form = new WWWForm();

        string dateStr = DateTime.Now.ToUniversalTime().AddHours(3).ToString();
        form.AddField("time", ConvertToBdFormat(dateStr) );
        WWW www = new WWW("http://192.168.1.35/Game/test.php", form);
        yield return www;
        Debug.Log(www.text);
    }

    string ConvertToBdFormat(string time)
    {
        string[] date = time.Split(' ');
        string[] tmp = date[0].Split('/');
        return tmp[2] + "-" + tmp[0] + "-" + tmp[1] + " " + date[1];
    }*/
}
