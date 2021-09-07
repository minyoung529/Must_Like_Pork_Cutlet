using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private User user = null;
    public User CurrentUser { get { return user; } }

    private string SAVE_PATH = "";
    private readonly string SAVE_FILENAME = "/SaveFile.txt";

    private long clickMoney = 1;
    public long onClickMoney { get { return clickMoney; } }


    private void Start()
    {
        SAVE_PATH = Application.dataPath + "/Save";
        //Application.persistentDataPath (나중에 안드로이드)
        if(!Directory.Exists(SAVE_PATH))
        {
            Directory.CreateDirectory(SAVE_PATH);
        }

        LoadFromJson();
    }

    private void LoadFromJson()
    {
        string json = "";

        if(File.Exists(SAVE_PATH + SAVE_FILENAME))
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

    private void OnApplicationPause(bool pause)
    {
        //SaveToJson();
    }

    private void OnApplicationQuit()
    {
        SaveToJson();
    }

    public User GetUser()
    {
        return user;
    }

    //다른 것들(GameObject, Transform)을 저장하려면 Json 플러그인
    //안하는 게 좋음
}
