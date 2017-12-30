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
    private List<string> tempOwned = new List<string>();
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


    public Transform ballContainer;
    public Transform btnContainer;
    public Transform container;
    public GameObject row;
    public Sprite success;

    string toastString;
    AndroidJavaObject currentActivity;


    void Start () {
        coins = PlayerPrefs.GetInt("Coins");
        Currency.text = "Coins: " + coins;
        LoadSkin();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { Menu(); }
    }

    public void LoadSkin()
    {
        if (File.Exists(Application.persistentDataPath + "/save.potato"))
        {
            Debug.Log("Data exist");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Open, FileAccess.ReadWrite);
            Save save = (Save)bf.Deserialize(file);
            textures = new List<string>(save.texture);
            tempOwned = new List<string>(save.owned);
            file.Close();

            Debug.Log("Textures: " + textures.Count);
            Debug.Log("Owned: " + tempOwned.Count);

            container.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2((textures.Count * 200) + 200, 0);
            foreach (string str in textures)
            {
                GameObject r = Instantiate(row) as GameObject;
                r.transform.SetParent(container);

                Transform transformBtn = r.gameObject.transform.GetChild(0);

                Button btn = transformBtn.GetComponentInChildren<Button>();
                btn.name = str;
                btn.onClick.AddListener(() => test(str));

                Transform b = r.gameObject.transform.GetChild(1);
                b.GetComponent<Renderer>().material.mainTexture = Resources.Load("BallsTexture/" + str) as Texture;
                
            }
        }
        else
        {
            Debug.Log("New data");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Create, FileAccess.ReadWrite);
            Save save = new Save
            {
                owned = new List<string> { "ball_01" },
                texture = new List<string>()
            };
            Texture[] texs = Resources.LoadAll<Texture>("BallsTexture");
            Debug.Log(texs.Length);
            container.GetComponentInChildren<RectTransform>().sizeDelta = new Vector2((texs.Length * 200) + 200, 0);

            foreach (Texture t in texs)
            {
                save.texture.Add(t.name);
                GameObject r = Instantiate(row) as GameObject;

                r.transform.SetParent(container);

                Transform transformBtn = r.gameObject.transform.GetChild(0);

                Button btn = transformBtn.GetComponentInChildren<Button>();
                btn.name = t.name;
                btn.onClick.AddListener(() => test(t.name));

                Transform b = r.gameObject.transform.GetChild(1);
                b.GetComponent<Renderer>().material.mainTexture = Resources.Load("BallsTexture/" + t.name) as Texture;
                
            }

            save.texture.Remove("ball_01");
            textures = new List<string>(save.texture);
            bf.Serialize(file, save);
            file.Close();
        }
    }

    public void test(string name)
    {

        string t = EventSystem.current.currentSelectedGameObject.name;

        TryBuyPanel.SetActive(true);
        TryBuyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        TryBuyButton.GetComponent<Button>().onClick.AddListener(() => BuyTex(name, t));
    }

    public void BuyTex(string name, string n)
    {
        if (name != "")
        {
            if (coins >= 100)
            {
                Debug.Log(name);
                textures.Remove(name);
                tempOwned.Add(name);

                TryBuyPanel.SetActive(false);
                PlayerPrefs.SetString("Skin", name);
                coins -= 100;
                Debug.Log("Buyed: " + name);
                PlayerPrefs.SetInt("Coins", coins);
                Currency.text = "Coins: " + coins;
                
                GameObject btnDis = GameObject.Find(n);
                
                btnDis.gameObject.SetActive(false);

                Toast(n);
            }
            else
            {
                Toast("Not enough money");
            }
        }
    }

    public void deleteData()
    {
        File.Delete(Application.persistentDataPath + "/save.potato");
        Debug.Log("Data deleted");
    }
    

    public void Menu()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = new FileStream(Application.persistentDataPath + "/save.potato", FileMode.Create);
            Save save = new Save
            {
                owned = new List<string> (tempOwned),
                texture = new List<string> (textures)

            };

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

    public void Toast(string msg)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            showToastOnUiThread(msg);
        }
    }

    void showToastOnUiThread(string toastString)
    {
        AndroidJavaClass UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        this.toastString = toastString;

        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(showToast));
    }

    void showToast()
    {
        Debug.Log("Running on UI thread");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
        AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", toastString);
        AndroidJavaObject toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
        toast.Call("show");
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
