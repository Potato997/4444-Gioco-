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
    public RectTransform rt;
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
        file.Close();
        textures = new List<string>(save.owned);

        foreach(string s in textures)
        {

            rt_x += 180;
            rt.sizeDelta = new Vector2(rt_x, 0);
        }

        foreach (string str in textures)
        {
            GetPos();
            GameObject instance = Instantiate(ball, pos, transform.rotation) as GameObject;
            instance.transform.SetParent(Scrollrect.transform);
            instance.transform.localScale = new Vector3(100, 100, 100);
            instance.GetComponent<Renderer>().material.mainTexture = Resources.Load("BallsTexture/" + str) as Texture;
            instance.name = "ball " + str;
            GameObject BtnInst = Instantiate(SetBtn, pos, transform.rotation) as GameObject;
            BtnInst.transform.SetParent(Scrollrect.transform);
            BtnInst.name = str;
            BtnInst.transform.position = new Vector3(x, -500, 400);
            BtnInst.GetComponent<Button>().onClick.AddListener(() => SetSkin());
            //CHANGE BUTTON TO SETTED OF THE SKIN ALREADY SETTED
            /*if (str == PlayerPrefs.GetString("Skin"))
            {
                BtnInst.GetComponent<Button>().image.sprite = SettedBtnSkin;
                BtnInst.GetComponent<Button>().GetComponentInChildren<Text>().text = "";
                BtnInst.GetComponent<Button>().image.rectTransform.sizeDelta = new Vector2(100, 100);
                BtnInst.GetComponent<Button>().interactable = false;
            }*/
            i++;
        }
    }

    private void GetPos()
    {
        x += 360;
        pos = new Vector3(x, 0, 400);
    }

    public void SetSkin()
    {
        if (texname != null)
        {
            CurrentBtn.GetComponent<Button>().image.sprite = NormalBtnSkin;
            CurrentBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = "Set Skin!";
            CurrentBtn.GetComponent<Button>().image.rectTransform.sizeDelta = new Vector2(300, 160);
            CurrentBtn.GetComponent<Button>().interactable = true;
        }
        CurrentBtn = EventSystem.current.currentSelectedGameObject;
        texname = CurrentBtn.name;
        PlayerPrefs.SetString("Skin", texname);
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