using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private User user;
    public User CurrentUser { get { return user; } }

    [SerializeField]
    private List<Hammer> hammers;
    [SerializeField]
    private List<Cutlet> cutlets;

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";


    private ulong cutletMoney = 100;

    public UIManager uiManager { get; private set; }

    private int maxCutletCnt = 10;

    private void Awake()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        //Application.persistentDataPath (나중에 안드로이드)
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
    }

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        InvokeRepeating("EarnMoneyPerSecond", 0f, 1f);
        SetCutletPrice();
    }

    public void EarnMoneyPerSecond()
    {
        foreach (PartTimer partTimer in user.partTimerList)
        {
            if (partTimer.GetIsSold())
                user.money += (ulong)partTimer.mps * (ulong)Mathf.RoundToInt((float)partTimer.level);
        }

        uiManager.UpdatePanel();
    }

    public void SetCutletPrice()
    {
        foreach (Cutlet cutlet in user.cutlets)
        {
            if (cutlet.GetIsSold())
                cutletMoney += (ulong)cutlet.addMoney;
        }
    }

    private void LoadFromJson()
    {
        string json = "";

        if (File.Exists(SAVE_PATH + SAVE_FILENAME))
        {
            Debug.Log("Load");

            json = File.ReadAllText(SAVE_PATH + SAVE_FILENAME);
            user = JsonUtility.FromJson<User>(json);
        }

        else
        {
            Debug.Log("Save, Load");
            SaveToJson();
            LoadFromJson();
        }
    }

    private void SaveToJson()
    {
        Debug.Log("SaveToJson");

        SAVE_PATH = Application.dataPath + "/Save";

        if (user == null) return;
        string json = JsonUtility.ToJson(user, true);
        File.WriteAllText(SAVE_PATH + SAVE_FILENAME, json, System.Text.Encoding.UTF8);
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    public int GetMaxCutletCnt()
    {
        return maxCutletCnt;
    }

    public List<Hammer> GetHammers()
    {
        return user.hammerList;
    }

    public List<Cutlet> GetCutlets()
    {
        return user.cutlets;
    }

    public ulong GetCutletPrice()
    {
        return cutletMoney;
    }
}
