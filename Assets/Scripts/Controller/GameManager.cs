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

    public UIManager UIManager { get; private set; }
    public QuestManager QuestManager { get; private set; }
    [SerializeField] Transform poolTransform;
    public Transform Pool { get { return poolTransform; } }

    private int maxCutletCnt = 10;

    private void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";
        //SAVE_PATH = Application.persistentDataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();

        user.hammerList[0].amount++;
    }

    private void Start()
    {
        UIManager = GetComponent<UIManager>();
        QuestManager = GetComponent<QuestManager>();
        InvokeRepeating("EarnMoneyPerSecond", 0f, 1f);
        SetUser();
        SetCutletPrice();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            user.money += 100000;
            UIManager.UpdatePanel();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            user.diamond += 100;
            UIManager.UpdatePanel();
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
        QuestManager.UpdateQuest();
        UIManager.UpdatePanel();
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

        SAVE_PATH = Application.persistentDataPath + "/Save";

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

        UIManager.UpdatePanel();
    }

    public void AddDiamond(int diamond)
    {
        CurrentUser.diamond += diamond;
        UIManager.UpdatePanel();
    }

    private void SetUser()
    {
        float plus;

        foreach (Cutlet cutlet in user.cutlets)
        {
            if (!cutlet.isSold)
            {
                plus = (cutlet.code > 0) ? 4390 * Mathf.Pow(cutlet.code, 1.2f) : 128;
                Debug.Log(cutlet.code);
                cutlet.SetPrice((ulong)Mathf.Round
                    (Mathf.Pow(cutlet.code + 2.3f, cutlet.code > 0 ? 0.4f * cutlet.code : 1)
                    * Mathf.Pow(cutlet.code + 2, 3.35f) + plus));

                cutlet.SetAddMoney(Mathf.RoundToInt(Mathf.Pow(cutlet.code + 1, 5.4f)));
            }
        }

        foreach (PartTimer partTimer in user.partTimerList)
        {
            //if (!partTimer.isSold)
            {
                partTimer.SetPrice((ulong)Mathf.RoundToInt(Mathf.Pow(partTimer.code + 1, partTimer.code * 0.2f) + 9900
                    * Mathf.Pow(partTimer.code + 1, partTimer.code * 0.3f) * partTimer.code * 2f + 9900));

                //=ROUND(POWER(E15+1,E15*0.2)+9900*POWER(E15+1,E15*0.3)*(E15*1.7) + 9900,0)
            }
        }
    }

    public bool RandomSelecting(float percentage)
    {
        float random = Random.Range(0, 100);
        if (percentage > random) return true;
        else return false;
    }

    //public float Randoms(params float[] percentages)
    //{
    //    float max = 0;
    //    float random = 0;

    //    foreach (float percentage in percentages)
    //    {
    //        max += percentage;
    //    }

    //    random = Random.Range(0f, max);

    //    foreach (float percentage in percentages)
    //    {
    //        if (random < percentage)
    //        {
    //            return percentage;
    //        }
    //    }

    //    return 0;
    //}
}
