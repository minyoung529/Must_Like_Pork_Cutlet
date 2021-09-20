using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private User user;
    public User CurrentUser { get { return user; } }

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";

    private ulong cutletMoney = 100;
    private ulong mpsMoney = 0;
    private int clickCount;

    public UIManager uiManager { get; private set; }
    public QuestManager questManager { get; private set; }

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

        user.hammerList[0].SetIsSold(true);
    }

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
        questManager = GetComponent<QuestManager>();
        InvokeRepeating("EarnMoneyPerSecond", 0f, 1f);
        SetUser();
        SetCutletPrice();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            user.money += 100000;
            uiManager.UpdatePanel();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            user.diamond += 100;
            uiManager.UpdatePanel();
        }
    }

    public void EarnMoneyPerSecond()
    {
        mpsMoney = 0;

        foreach (PartTimer partTimer in user.partTimerList)
        {
            if (partTimer.GetIsSold())
            {
                mpsMoney += (ulong)partTimer.mps * (ulong)Mathf.RoundToInt((float)partTimer.level);
                AddMoney((ulong)partTimer.mps * (ulong)Mathf.RoundToInt((float)partTimer.level), true);
            }
        }

        user.Quests[1].PlusCurValue(1);
        questManager.UpdateQuest();
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

    public ulong GetCutletPrice()
    {
        return cutletMoney;
    }

    public ulong GetMPS()
    {
        return mpsMoney;
    }

    public void AddMoney(ulong money, bool isAdd)
    {
        if (isAdd)
            CurrentUser.money += money;
        else
            CurrentUser.money -= money;

        uiManager.UpdatePanel();
    }

    public void AddDiamond(int diamond)
    {
        CurrentUser.diamond += diamond;
        uiManager.UpdatePanel();
    }

    private void SetUser()
    {
        foreach (Cutlet cutlet in user.cutlets)
        {
            cutlet.SetPrice((ulong)Mathf.Round(Mathf.Pow(cutlet.code, 2) * Mathf.Pow(cutlet.code + 1, 3.85f) - 1 * cutlet.code + 1 + 100));
        }
    }

    public bool RandomSelecting(float percentage)
    {
        float random = Random.Range(0, 100);
        if (percentage > random) return true;
        else return false;
    }

    public float Randoms(params float[] percentages)
    {
        float max = 0;
        float random = 0;

        foreach (float percentage in percentages)
        {
            max += percentage;
        }

        random = Random.Range(0f, max);

        foreach (float percentage in percentages)
        {
            if (random < percentage)
            {
                return percentage;
            }
        }

        return 0;
    }
}
