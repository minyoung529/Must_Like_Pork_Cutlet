using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    #region ����
    [Header("�г� ���ø�")]
    [SerializeField] private GameObject partTimerpanelTemplate;
    [SerializeField] private GameObject hammerPanelTemplate;
    [SerializeField] private GameObject cutletPanelTemplate;

    [Header("���� Ʈ��������")]
    [SerializeField] private Transform partTimersTransform;
    [SerializeField] private Transform cutletsTransform;

    [Header("�� �ؽ�Ʈ")]
    [SerializeField] private Text moneyText;
    private Text countText;

    [Header("�մ�")]
    [SerializeField]
    private Image guest;

    private ScrollRect scrollRect;

    private RectTransform hammerTransform;
    private RectTransform cutletTransform;
    private RectTransform partTimerTransform;

    private int count = 0;
    private ulong oldMoney;

    private List<PanelBase> upgradePanels = new List<PanelBase>();
    [Header("���� ������")]
    [SerializeField] private CutletMove cutlet;
    #endregion

    private Sprite[] partTimerSprites;
    private Sprite[] cutletSprites;
    private Sprite[] guestSprites;
    private Image[] partTimersImage;
    private Image[] cutletsImage;

    private void Start()
    {
        FirstSetting();
        SettingUpgradePanel();
        ActivePartTimers();
    }

    private void SettingUpgradePanel()
    {
        InstantiatePanel(cutletPanelTemplate, cutletTransform, GameManager.Instance.CurrentUser.cutlets.Count, cutletSprites);
        InstantiatePanel(partTimerpanelTemplate, partTimerTransform, GameManager.Instance.CurrentUser.partTimerList.Count, partTimerSprites);
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
            countText.text = count.ToString();
            cutlet.SetSprite(count);
            CutletsOnTable();
        }
    }

    public void UpdatePanel()
    {
        moneyText.text = GameManager.Instance.CurrentUser.money + "��";

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
        List<PartTimer> partTimers = GameManager.Instance.CurrentUser.partTimerList;
        List<Cutlet> cutlets = GameManager.Instance.CurrentUser.cutlets;

        scrollRect = partTimerpanelTemplate.transform.parent.parent.parent.gameObject.GetComponent<ScrollRect>();
        cutletTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(0).gameObject.GetComponent<RectTransform>();
        partTimerTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(1).gameObject.GetComponent<RectTransform>();
        hammerTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(2).gameObject.GetComponent<RectTransform>();
        countText = moneyText.transform.parent.GetChild(1).gameObject.GetComponent<Text>();

        partTimersImage = new Image[partTimers.Count];
        cutletsImage = new Image[cutletsTransform.childCount];

        for (int i = 0; i < partTimers.Count; i++)
        {
            partTimersImage[i] = partTimersTransform.GetChild(i).gameObject.GetComponent<Image>();
        }

        for (int i = 0; i < cutletsTransform.childCount; i++)
        {
            cutletsImage[i] = cutletsTransform.GetChild(i).gameObject.GetComponent<Image>();
        }

        cutletTransform.gameObject.SetActive(true);
        partTimerTransform.gameObject.SetActive(false);
        hammerTransform.gameObject.SetActive(false);

        partTimerSprites = Resources.LoadAll<Sprite>("Sprites/PartTimerImage");
        cutletSprites = Resources.LoadAll<Sprite>("Sprites/CutletImage");
        guestSprites = Resources.LoadAll<Sprite>("Sprites/Guest");
        UpdatePanel();
    }

    public int GetCount()
    {
        return count;
    }

    public void ActivePartTimers()
    {
        List<PartTimer> partTimers = GameManager.Instance.CurrentUser.partTimerList;

        for (int i = 0; i < partTimers.Count; i++)
        {
            if (partTimers[i].GetIsSold())
            {
                partTimersImage[i].color = Color.white;
            }
        }
    }

    private void CutletsOnTable()
    {
        int num = 0;

        for (int i = 0; i < GameManager.Instance.CurrentUser.cutlets.Count; i++)
        {
            if (GameManager.Instance.CurrentUser.cutlets[i].isSold)
                num++;
        }

        if (num == 0) return;

        num = 0;

        for (int i = 0; i < cutletsImage.Length; i++)
        {
            if (cutletsImage[i].color == Color.clear)
            {
                num = i;
                break;
            }
        }

        cutletsImage[num].sprite = cutletSprites[RandomCutletNumber()];
        cutletsImage[num].color = Color.white;

        StartCoroutine(InstantiateGuest());
    }

    private int RandomCutletNumber()
    {
        List<int> numbers = new List<int>();
        int number;

        for (int i = 0; i < GameManager.Instance.CurrentUser.cutlets.Count; i++)
        {
            if (GameManager.Instance.CurrentUser.cutlets[i].GetIsSold())
            {
                numbers.Add(i);
            }
        }

        number = Random.Range(0, numbers.Count);
        Debug.Log(number);
        Debug.Log(numbers.Count);

        return numbers[number];
    }

    IEnumerator InstantiateGuest()
    {
        float time = Random.Range(2f, 4.5f);
        float randomX = Random.Range(-32.4f, 32.4f);
        yield return new WaitForSeconds(time);
        guest.sprite = guestSprites[Random.Range(0, 2)];
        guest.gameObject.SetActive(true);
        guest.transform.DOLocalMove(new Vector2(randomX, 45f), 0.4f);
        yield return new WaitForSeconds(0.4f);
        guest.transform.DOLocalMove(new Vector2(randomX, 20f), 0.3f);

        cutletsImage[0].color = Color.clear;
        cutletsImage[0].transform.SetAsLastSibling();
        SwapCutletsImage();
    }

    private void SwapCutletsImage()
    {
        Image temp = cutletsImage[0];

        for (int i = 0; i < cutletsImage.Length; i++)
        {
            if (i < cutletsImage.Length - 1)
                cutletsImage[i] = cutletsImage[i + 1];
            else
                cutletsImage[i] = temp;
        }
    }
}
