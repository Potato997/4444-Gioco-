using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour{
    
    public Text Currency;
    public List<string> textures;
    private List<string> tempOwned = new List<string> { };
    private Vector3 pos = new Vector3(0, 0, 0);
    public GameObject ball;
    private int i, x;
    public GameObject Scrollrect;
    public GameObject BuyBtn;
    public GameObject TryBuyPanel;
    GameObject BtnInstance;
    public Button TryBuyButton;
    public RectTransform rt;
    private int rt_x;

    private int coins;
    

    void Start () {
        //File.Delete(Application.persistentDataPath + "/save.potato");
        x = -360;
        i = 0;
        rt_x = 0;
        coins = PlayerPrefs.GetInt("Coins");
        Currency.text = "Coins: " + coins;
        LoadSkin();
    }
    
    public void LoadSkin()
    {
        if (File.Exists(Application.persistentDataPath + "/save.potato"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Open, FileAccess.ReadWrite);
            Save save = (Save)bf.Deserialize(file);
            textures = new List<string>(save.texture);
            tempOwned = new List<string>(save.owned);
            file.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Create, FileAccess.ReadWrite);
            Save save = new Save();
            save.owned = new List<string>{"ball_01"};
            save.texture = new List<string>();
            Object[] loadtex = Resources.LoadAll("BallsTexture");
            Debug.Log(loadtex.Length);
            foreach(Object tx in loadtex)
            {
                string[] name = tx.ToString().Split(' ');
                save.texture.Add(name[0]);
            }
            save.texture.Remove("ball_01");
            textures = new List<string>(save.texture);
            bf.Serialize(file, save);
            file.Close();
        }
        

        Debug.Log(textures.Count);

        foreach (string s in textures)
        {
            rt_x -= 160;
            rt.sizeDelta = new Vector2(rt_x, 0);
        }

        foreach (string str in textures)
        {
            GetPos();
            GameObject instance = Instantiate(ball, pos, transform.rotation) as GameObject;
            instance.transform.SetParent(Scrollrect.transform);
            instance.transform.localScale = new Vector3(100, 100, 100);
            instance.GetComponent<Renderer>().material.mainTexture = Resources.Load("BallsTexture/" + str) as Texture;
            instance.name = str;
            BtnInstance = Instantiate(BuyBtn, pos, transform.rotation) as GameObject;
            BtnInstance.transform.SetParent(Scrollrect.transform);
            BtnInstance.name = str;
            BtnInstance.transform.position = new Vector3(x, -300, 0);
            BtnInstance.GetComponent<Button>().onClick.AddListener(() => TryBuy());
            i++;
        }
    }
    
    private void GetPos()
    {
        x += 360;
        pos = new Vector3(x, 0, 0);
    }

    public void Menu()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Create);
            Save save = new Save();
            save.owned = tempOwned;
            save.texture = textures;
            bf.Serialize(file, save);
            file.Close();
            SceneManager.LoadScene(0);
        }
        catch(System.Runtime.Serialization.SerializationException e)
        {
            Debug.Log(e);
        }
        Debug.Log("Saved");
        
    }

    public void TryBuy()
    {
        string texname = EventSystem.current.currentSelectedGameObject.name;
        TryBuyPanel.SetActive(true);
        TryBuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        TryBuyButton.GetComponent<Button>().onClick.AddListener(() => BuyTex(texname));
    }

    public void BuyTex(string name)
    {
        if(name != "")
        {
            if (coins >= 100)
            {
                Debug.Log(name);
                textures.Remove(name);
                tempOwned.Add(name);
                
                TryBuyPanel.SetActive(false);
                PlayerPrefs.SetString("Skin", name);
                coins -= 100;
                Debug.Log("Buy: " + name);
                PlayerPrefs.SetInt("Coins", coins);
                Currency.text = "Coins: " + coins;
                Button[] bt = FindObjectsOfType<Button>();
                foreach (Button b in bt)
                {
                    if (b.name == name)
                    {
                        b.gameObject.SetActive(false);
                        return;
                    }
                }
            }
            else
            {
                Debug.Log("Not enough money");
            }
        }
    }

    public void Cancel()
    {
        TryBuyPanel.SetActive(false);
    }

    public void AddMoney()
    {
        coins += 200;
        PlayerPrefs.SetInt("Coins", coins);
        Currency.text = "Coins: " + coins;
    }
}
