using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MyBalls : MonoBehaviour {

    #region Variables
    public List<string> textures;
    private Vector3 pos = new Vector3(0, 0, 0);
    public GameObject ball;
    private int i, x;
    private int rt_x;
    public GameObject Scrollrect;
    public GameObject SetBtn;
    public GameObject CurrentBtn;
    private string texname;
    public Sprite SettedBtnSkin;
    public Sprite NormalBtnSkin;
    
    public Transform container;
    public GameObject row;

    #endregion

    void Start () {
        x = -360;
        i = 0;
        LoadSkin();
    }

    void LoadSkin()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Open, FileAccess.ReadWrite);
        Save save = (Save)bf.Deserialize(file);
        textures = new List<string>(save.texture);
        file.Close();

        container.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2((textures.Count * 200) + 200, 0);
        foreach (string str in textures)
        {
            GameObject r = Instantiate(row) as GameObject;
            r.transform.SetParent(container);

            Transform transformBtn = r.gameObject.transform.GetChild(0);

            Button btn = transformBtn.GetComponentInChildren<Button>();
            btn.name = str;
            btn.onClick.AddListener(() => SetSkin(str));

            Transform b = r.gameObject.transform.GetChild(1);
            b.GetComponent<Renderer>().material.mainTexture = Resources.Load("BallsTexture/" + str) as Texture;

        }
    }

    private void GetPos()
    {
        x += 360;
        pos = new Vector3(x, 0, 400);
    }

    public void SetSkin(string n)
    {
        if (n != null)
        {
            CurrentBtn.GetComponent<Button>().image.sprite = NormalBtnSkin;
            CurrentBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "Set Skin!";
            CurrentBtn.GetComponent<Button>().image.rectTransform.sizeDelta = new Vector2(300, 160);
            CurrentBtn.GetComponent<Button>().interactable = true;
        }
        CurrentBtn = EventSystem.current.currentSelectedGameObject;
        n = CurrentBtn.name;
        PlayerPrefs.SetString("Skin", n);
        CurrentBtn.GetComponent<Button>().image.sprite = SettedBtnSkin;
        CurrentBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "";
        CurrentBtn.GetComponent<Button>().image.rectTransform.sizeDelta = new Vector2(100, 100);
        CurrentBtn.GetComponent<Button>().interactable = false;
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}