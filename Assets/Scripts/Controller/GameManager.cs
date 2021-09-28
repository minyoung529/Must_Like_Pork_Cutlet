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

    public ulong cutletMoney { get; private set; } = 100;
    public int maxCutletCnt { get; private set; } = 10;
    public ulong mpsMoney { get; private set; } = 0;

    public UIManager UIManager { get; private set; }
    public QuestManager QuestManager { get; private set; }
    public TutorialManager TutorialManager { get; private set; }


    [SerializeField] private Transform poolTransform;
    public Transform Pool { get { return poolTransform; } }

    private void Awake()
    {
        SAVE_PATH = Application.persistentDataPath + "/Save";
        //SAVE_PATH = Application.dataPath + "/Save";
        if (!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();

        if (user == null)
        {
            user.hammerList[0].amount++;
        }
    }

    public void Start()
    {
        UIManager = GetComponent<UIManager>();
        QuestManager = GetComponent<QuestManager>();
        TutorialManager = GetComponent<TutorialManager>();
        InvokeRepeating("EarnMoneyPerSecond", 0f, 1f);
        SetUser();
        SetCutletPrice();

        if (user.isTutorial)
        {
            Camera.main.transform.position = new Vector3(0f, 3f);
        }

        else
        {
            Camera.main.transform.position = new Vector3(3f, 0f);
        }

        ConvertMoneyText(5345344533);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            user.money += 100000;
            UIManager.UpdatePanel();
        }

        if (Input.GetKey(KeyCode.LeftShift))
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
        cutletMoney = 100;

        foreach (Cutlet cutlet in user.cutlets)
        {
            if (cutlet.GetIsSold())
            {
                cutletMoney += (ulong)cutlet.addMoney;
            }
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
            if (!cutlet.GetIsSold())
            {
                plus = (cutlet.code > 0) ? 4390 * Mathf.Pow(cutlet.code, 1.2f) : 128;
                cutlet.SetPrice((ulong)Mathf.Round
                    (Mathf.Pow(cutlet.code + 3.3f, cutlet.code > 0 ? 0.8f * cutlet.code : 1)
                    * Mathf.Pow(cutlet.code + 2, 4.65f) + plus));

                cutlet.SetAddMoney(Mathf.RoundToInt(Mathf.Pow(cutlet.code + 2, 5)));
            }
        }

        foreach (PartTimer partTimer in user.partTimerList)
        {
            if (!partTimer.GetIsSold())
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

    public string ConvertMoneyText(ulong money)
    {
        string moneyStr = "";
        string[] unitStr = { "만", "억", "조", "경", "해" };
        ulong offset = 10000;
        ulong extra = 0;

        //20000
        if (money > offset)
        {
            for (int i = 4; i >= 0; i--)
            {
                if (money >= Mathf.Pow(offset, i + 1))
                {
                    moneyStr += string.Format("{0}{1} ", (int)(money / Mathf.Pow(offset, i + 1)), unitStr[i]);
                    money -= (ulong)((int)(money / Mathf.Pow(offset, i + 1)) * Mathf.Pow(offset, i + 1));
                }
            }
        }

        return string.Format("{0}{1}원", moneyStr, money);
    }
}