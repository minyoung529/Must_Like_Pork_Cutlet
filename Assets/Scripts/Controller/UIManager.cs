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

    [SerializeField] private Text moneyText;
    private Text countText;

    private ScrollRect scrollRect;

    private RectTransform hammerTransform;
    private RectTransform cutletTransform;
    private RectTransform staffTransform;

    int count = 0;

    //private List<PorkCutletUpgradePanel> cutletUpgradePanels = new List<PorkCutletUpgradePanel>();


    private void Start()
    {
        FirstSetting();
        //SettingUpgradePanel();
        UpdatePanel();
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

    private void UpdatePanel()
    {
        moneyText.text = GameManager.Instance.CurrentUser.money.ToString();
    }

    public void SetContent(RectTransform content)
    {
        scrollRect.content = content;
        hammerTransform.gameObject.SetActive(false);
        cutletTransform.gameObject.SetActive(false);
        staffTransform.gameObject.SetActive(false);
        content.gameObject.SetActive(true);
    }

    private void FirstSetting()
    {
        scrollRect = panelTemplate.transform.parent.parent.parent.gameObject.GetComponent<ScrollRect>();
        hammerTransform = scrollRect.transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>();
        cutletTransform = scrollRect.transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>();
        staffTransform = scrollRect.transform.GetChild(0).GetChild(2).gameObject.GetComponent<RectTransform>();
        countText = moneyText.transform.parent.GetChild(1).gameObject.GetComponent<Text>();
    }
}
