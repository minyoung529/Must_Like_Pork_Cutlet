using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelTemplate;

    private TextAsset testText;
    [SerializeField]
    private List<TestEntity> testEntities = new List<TestEntity>();

    [SerializeField] private Text moneyText;
    private Text countText;

    int count = 0;

    //private List<PorkCutletUpgradePanel> cutletUpgradePanels = new List<PorkCutletUpgradePanel>();


    private void Start()
    {
        //SettingUpgradePanel();
        LoadResources();
        countText = moneyText.transform.parent.GetChild(1).gameObject.GetComponent<Text>();
    }

    private void SettingUpgradePanel()
    {
        PorkCutletUpgradePanel panel;
        for (int i = 0; i < GameManager.Instance.CurrentUser.cutletList.Count; i++)
        {
            Instantiate(panelTemplate, panelTemplate.transform.parent);
            panel = panelTemplate.GetComponent<PorkCutletUpgradePanel>();
            panel.Init(i);
            //cutletUpgradePanels.Add(panel);
        }
    }

    public void OnClickPork()
    {
        count++;
        countText.text = count.ToString();

        if (count > 9)
        {
            GameManager.Instance.CurrentUser.money += GameManager.Instance.onClickMoney;
            UpdatePanel();
            count = 0;
            countText.text = count.ToString();
        }
    }

    private void LoadResources()
    {
        int cnt = 0;
        testText = Resources.Load<TextAsset>("Data/Test2");
        string test = testText.ToString();

        string[] line = test.Split('\n');

        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            for (int j = 0; j < row.Length; j++)
            {
                cnt = 0;

                testEntities[i].num = Int32.Parse(row[cnt]);
                cnt++;
                testEntities[i].name = row[cnt];
                cnt++;
                testEntities[i].code = Int32.Parse(row[cnt]);
            }

        }
    }

    private void UpdatePanel()
    {
        moneyText.text = GameManager.Instance.CurrentUser.money.ToString();
    }
}
