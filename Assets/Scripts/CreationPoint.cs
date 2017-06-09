using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreationPoint : MonoBehaviour {

    private bool isGood = true;

    private GameManager gameManager;
    private float aliveTime = 1.5f;
    private float deg;
    private float speed = 170.0f;
    private GameObject panel;
    private float width;
    private float height;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        panel = GameObject.Find("CreationPoints");
        transform.SetParent(panel.transform);
        transform.localScale = new Vector3(1, 1, 1);
        width = panel.GetComponent<RectTransform>().rect.width;
        height = panel.GetComponent<RectTransform>().rect.height;

        transform.localPosition = new Vector3(Random.Range(-width/2.0f, width/2.0f), Random.Range(-height / 2.0f, 0), 0);
        if (Random.Range(0,10)<=1)
        {
            isGood = false;
        }
        if (isGood)
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Buttons/boostbutton");
        }
        else
        {
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Buttons/bug");
        }
        Destroy(gameObject, aliveTime);
        //Debug.Log(transform.localPosition);
        //Debug.Log(transform.parent.GetComponent<RectTransform>().rect.width + "x" + transform.parent.GetComponent<RectTransform>().rect.height);
    }
	
	// Update is called once per frame
	void Update () {
        PointMotion();
    }

    public void OnMouseDown()
    {
        StartCoroutine( Clicked() );
    }

    void PointMotion()
    {
        Vector3 pos = transform.localPosition;
        deg = Mathf.Deg2Rad * Random.Range(0.0f, 180.0f);
        transform.localPosition += new Vector3(speed * Time.deltaTime * Mathf.Cos(deg), speed * Time.deltaTime * Mathf.Sin(deg), 0.0f);
        if(pos.x < width / -2.0f)
        {
            pos.x = width / -2.0f;
        }
        if (pos.x > width / 2.0f)
        {
            pos.x = width / 2.0f;
        }
    }

    IEnumerator Clicked()
    {
        WWWForm form = new WWWForm();
        form.AddField("playername", GameObject.Find("SaveObject").GetComponent<SaveObject>().playerName);
        if (isGood)
        {
            gameManager.developer.LastGame().BoostPoints += 1;
            form.AddField("boost", "1");
        }
        else
        {
            gameManager.developer.LastGame().Bugs += 1;
            form.AddField("bug", "1");
        }
        WWW www = new WWW("http://192.168.1.35/Game/AddPoint.php", form);
        yield return www;
        DestroyImmediate(gameObject);
    }
}
