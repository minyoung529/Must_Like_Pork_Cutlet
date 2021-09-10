using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region ����
    [SerializeField] private GameObject partTimerpanelTemplate;
    [SerializeField] private GameObject hammerPanelTemplate;
    [SerializeField] private GameObject cutletPanelTemplate;

    [SerializeField] private Text moneyText;
    private Text countText;

    private ScrollRect scrollRect;

    private RectTransform hammerTransform;
    private RectTransform cutletTransform;
    private RectTransform partTimerTransform;

    private int count = 0;

    private List<PanelBase> upgradePanels = new List<PanelBase>();
    [SerializeField] private CutletMove cutlet;
    #endregion

    Sprite[] partTimerButtonImages;

    private void Start()
    {
        FirstSetting();
        SettingUpgradePanel();
    }

    private void SettingUpgradePanel()
    {
        //InstantiatePanel(cutletPanelTemplate, cutletTransform, GameManager.Instance.CurrentUser.cutlets.Count);
        InstantiatePanel(partTimerpanelTemplate, partTimerTransform, GameManager.Instance.CurrentUser.partTimerList.Count, partTimerButtonImages);
        //InstantiatePanel(hammerPanelTemplate, hammerTransform, GameManager.Instance.CurrentUser.hammerList.Count);
    }

    private void InstantiatePanel(GameObject template, RectTransform rectTransform, int count, Sprite[] sprites)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template, rectTransform);
            panel = obj.GetComponent<PanelBase>();
            panel.Init(i, sprites[i]);
            upgradePanels.Add(panel);
            obj.SetActive(true);
        }

        template.SetActive(false);
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
        moneyText.text = GameManager.Instance.CurrentUser.money.ToString() + "��";
        foreach (PanelBase upgradePanels in upgradePanels)
        {
            upgradePanels.Inactive();
        }
    }

    public void SetContent(RectTransform content)
    {
        scrollRect.content = content;
        hammerTransform.gameObject.SetActive(false);
        cutletTransform.gameObject.SetActive(false);
        partTimerTransform.gameObject.SetActive(false);
        content.gameObject.SetActive(true);
    }

    private void FirstSetting()
    {
        scrollRect = partTimerpanelTemplate.transform.parent.parent.parent.gameObject.GetComponent<ScrollRect>();
        cutletTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(0).gameObject.GetComponent<RectTransform>();
        partTimerTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(1).gameObject.GetComponent<RectTransform>();
        hammerTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(2).gameObject.GetComponent<RectTransform>();
        countText = moneyText.transform.parent.GetChild(1).gameObject.GetComponent<Text>();

        scrollRect.content = cutletTransform;
        cutletTransform.gameObject.SetActive(true);
        partTimerTransform.gameObject.SetActive(false);
        hammerTransform.gameObject.SetActive(false);

        partTimerButtonImages = Resources.LoadAll<Sprite>("Sprites/PartTimerButtonImage");
        UpdatePanel();
    }

    public int GetCount()
    {
        return count;
    }
}
