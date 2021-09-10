using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject panelTemplate;
    [SerializeField]
    private GameObject hammerPanelTemplate;
    [SerializeField]
    private GameObject cutletPanelTemplate;


    [SerializeField] private Text moneyText;
    private Text countText;

    private ScrollRect scrollRect;

    private RectTransform hammerTransform;
    private RectTransform cutletTransform;
    private RectTransform itemTransform;

    private int count = 0;

    private List<PartTimerPanel> upgradePanels = new List<PartTimerPanel>();
    [SerializeField]
    private CutletMove cutlet;

    private void Start()
    {
        FirstSetting();
        SettingUpgradePanel();
    }

    private void SettingUpgradePanel()
    {
        PartTimerPanel panel;
        GameObject obj;

        for (int i = 0; i < GameManager.Instance.CurrentUser.partTimerList.Count; i++)
        {
            obj = Instantiate(panelTemplate, itemTransform);
            panel = obj.GetComponent<PartTimerPanel>();
            panel.Init(i);
            upgradePanels.Add(panel);
            obj.SetActive(true);
        }

        for (int i = 0; i < GameManager.Instance.GetHammers().Count; i++)
        {
            obj = Instantiate(hammerPanelTemplate, hammerTransform);
            panel = obj.GetComponent<HammerPanel>();
            panel.Init(i);
            upgradePanels.Add(panel);
            obj.SetActive(true);
        }
        Debug.Log(GameManager.Instance.GetCutlets().Count);

        for (int i = 0; i < GameManager.Instance.GetCutlets().Count; i++)
        {
            Debug.Log(GameManager.Instance.GetCutlets().Count);
            obj = Instantiate(cutletPanelTemplate, cutletTransform);
            panel = obj.GetComponent<CutletPanel>();
            panel.Init(i);
            upgradePanels.Add(panel);
            obj.SetActive(true);
        }
    }

    public void OnClickPork()
    {
        count++;
        countText.text = count.ToString();
        cutlet.Move();
        cutlet.SetSprite(count);

        if (count > GameManager.Instance.GetMaxCutletCnt() - 1)
        {
            GameManager.Instance.CurrentUser.money += GameManager.Instance.GetCutletPrice();
            UpdatePanel();
            count = 0;
            cutlet.SetSprite(count);
            countText.text = count.ToString();
        }
    }

    public void UpdatePanel()
    {
        moneyText.text = GameManager.Instance.CurrentUser.money.ToString() + "¿ø";
        foreach (PartTimerPanel upgradePanels in upgradePanels)
        {
            upgradePanels.Inactive();
        }
    }

    public void SetContent(RectTransform content)
    {
        scrollRect.content = content;
        hammerTransform.gameObject.SetActive(false);
        cutletTransform.gameObject.SetActive(false);
        itemTransform.gameObject.SetActive(false);
        content.gameObject.SetActive(true);
    }

    private void FirstSetting()
    {
        scrollRect = panelTemplate.transform.parent.parent.parent.gameObject.GetComponent<ScrollRect>();
        itemTransform = panelTemplate.transform.parent.parent.GetChild(0).gameObject.GetComponent<RectTransform>();
        cutletTransform = panelTemplate.transform.parent.parent.GetChild(1).gameObject.GetComponent<RectTransform>();
        hammerTransform = panelTemplate.transform.parent.parent.GetChild(2).gameObject.GetComponent<RectTransform>();
        countText = moneyText.transform.parent.GetChild(1).gameObject.GetComponent<Text>();

        cutletTransform.gameObject.SetActive(false);
        itemTransform.gameObject.SetActive(true);
        hammerTransform.gameObject.SetActive(false);
        UpdatePanel();
    }

    public int GetCount()
    {
        return count;
    }
}
