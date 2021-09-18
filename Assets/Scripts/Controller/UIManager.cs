using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    #region º¯¼ö
    [Header("ÆÐ³Î ÅÛÇÃ¸´")]
    [SerializeField] private PanelBase partTimerpanelTemplate;
    [SerializeField] private PanelBase hammerPanelTemplate;
    [SerializeField] private PanelBase cutletPanelTemplate;
    [SerializeField] private PanelBase randomHammerTemplate;

    [Header("°¢°¢ Æ®·£½ºÆûµé")]
    [SerializeField] private Transform partTimersTransform;
    [SerializeField] private Transform cutletsTransform;

    [Header("µ· ÅØ½ºÆ®")]
    [SerializeField] private Text moneyText;
    private Text countText;
    private Text mpsAndCutletText;

    [Header("¼Õ´Ô")]
    [SerializeField] private Image guest;

    [Header("¸ÁÄ¡")]
    [SerializeField] private SpriteRenderer playerHammer;

    [Header("·£´ý »Ì±â")]
    [SerializeField] private RectTransform randomHammerTransform;

    [Header("¼³¸í ÆÐ³Î")]
    [SerializeField] private HammerInformation hammerInformation;
    [SerializeField] private PartTimerInformation partTimerInformation;

    private ScrollRect scrollRect;

    private RectTransform hammerTransform;
    private RectTransform cutletTransform;
    private RectTransform partTimerTransform;

    private int count = 0;

    private List<PanelBase> upgradePanels = new List<PanelBase>();
    private List<PanelBase> randomPanel = new List<PanelBase>();

    [Header("¸ÞÀÎ µ·°¡½º")]
    [SerializeField] private CutletMove cutlet;
    #endregion

    private Sprite[] partTimerSprites;
    private Sprite[] cutletSprites;
    private Sprite[] hammerSprites;
    private Sprite[] guestSprites;
    private Image[] partTimersImage;
    private Image[] cutletsImage;
    private Image[] randomHammerImage = new Image[10];

    private List<HammerPanel> hammerPanels;
    private List<CutletPanel> cutletPanels;
    private List<PartTimerPanel> partTimerPanels;

    private void Start()
    {
        FirstSetting();
        ActivePartTimers();

        for (int i = 0; i < 10; i++)
        {
            randomHammerImage[i] = randomHammerTransform.GetChild(i + 1).gameObject.GetComponent<Image>();
        }

        RandomHammer(10);
    }

    private void SettingUpgradePanel()
    {
        InstantiatePanel(hammerPanelTemplate, hammerTransform, GameManager.Instance.CurrentUser.hammerList.Count, hammerSprites);
        InstantiatePanel(cutletPanelTemplate, cutletTransform, GameManager.Instance.CurrentUser.cutlets.Count, cutletSprites);
        InstantiatePanel(partTimerpanelTemplate, partTimerTransform, GameManager.Instance.CurrentUser.partTimerList.Count, partTimerSprites);
        InstantiatePanel(randomHammerTemplate, randomHammerTransform, 10);
    }

    private void InstantiatePanel(PanelBase template, RectTransform rectTransform, int count, Sprite[] sprites = null)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, rectTransform);
            panel = obj.GetComponent<PanelBase>();

            if (template == randomHammerTemplate)
            {
                panel.Init(0, null);
                randomPanel.Add(panel);
                obj.SetActive(true);
                continue;
            }

            panel.Init(i, sprites[i]);
            upgradePanels.Add(panel);
            obj.SetActive(true);
        }

        template.gameObject.SetActive(false);
    }

    [Obsolete]
    public void OnClickPork()
    {
        Hammer hammer = GameManager.Instance.CurrentUser.GetHammer();

        GameManager.Instance.AddMoney((ulong)hammer.clickPerMoney, true);
        if(GameManager.Instance.RandomSelecting(GameManager.Instance.CurrentUser.GetHammer().criticalHit))
        {
            count += hammer.clickCount;
        }
        else
        {
            count += hammer.clickCount + 2;
            cutlet.CriticalHit();
        }

        countText.text = count.ToString();
        cutlet.Move();
        cutlet.SetSprite(count);

        if (count > GameManager.Instance.GetMaxCutletCnt() - 1)
        {
            GameManager.Instance.AddMoney(GameManager.Instance.GetCutletPrice(), true);
            UpdatePanel();

            count -= 10;
            countText.text = count.ToString();
            cutlet.SetSprite(count);
            CutletsOnTable();
        }
    }

    public void UpdatePanel()
    {
        moneyText.text = GameManager.Instance.CurrentUser.money + "¿ø";
        mpsAndCutletText.text = string.Format("µ·°¡½º °¡°Ý {0}¿ø / ÃÊ´ç {1}¿ø", GameManager.Instance.GetCutletPrice(), GameManager.Instance.GetMPS());
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

        scrollRect = partTimerpanelTemplate.transform.parent.parent.parent.gameObject.GetComponent<ScrollRect>();
        cutletTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(0).gameObject.GetComponent<RectTransform>();
        partTimerTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(1).gameObject.GetComponent<RectTransform>();
        hammerTransform = partTimerpanelTemplate.transform.parent.parent.GetChild(2).gameObject.GetComponent<RectTransform>();
        countText = moneyText.transform.parent.GetChild(1).gameObject.GetComponent<Text>();
        mpsAndCutletText = moneyText.transform.parent.GetChild(2).gameObject.GetComponent<Text>();

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
        hammerSprites = Resources.LoadAll<Sprite>("Sprites/Hammer");

        SettingUpgradePanel();

        ChangeHammerSprite(hammerSprites[GameManager.Instance.CurrentUser.GetHammer().code]);
        SetHammer(GameManager.Instance.CurrentUser.GetHammer().code);
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

    public void RandomHammer(int amount)
    {
        int rand;

        for (int i = 0; i < amount; i++)
        {
            rand = SelectRandom();
            randomPanel[i].Init(rand, hammerSprites[rand]);
        }
    }

    private int SelectRandom()
    {
        List<Hammer> hammerList = GameManager.Instance.CurrentUser.hammerList;
        int rand = Random.Range(0, 100);
        int commonCnt = 0;
        int rareCnt = 0;

        for (int i = 0; i < hammerList.Count; i++)
        {
            if (hammerList[i].grade == "common")
            {
                commonCnt++;
            }

            else if (hammerList[i].grade == "rare")
            {
                rareCnt++;
            }
        }

        if (rand < 10) // 0-9
        {
            rand = Random.Range(rareCnt, hammerList.Count);
        }

        else if (rand < 40) //10 - 49
        {
            rand = Random.Range(commonCnt, rareCnt);
        }

        else
        {
            rand = Random.Range(0, commonCnt);
        }

        return rand;
    }

    public void ChangeHammerSprite(Sprite sprite)
    {
        Debug.Log(sprite.name);
        playerHammer.sprite = sprite;
    }

    public void CheckHammer()
    {
        for (int i = 0; i < GameManager.Instance.CurrentUser.hammerList.Count; i++)
        {
            upgradePanels[i].SetActiveCheck();
        }
    }

    public Sprite[] GetHammerSprites()
    {
        return hammerSprites;
    }

    public Sprite[] GetPartTimerSprite()
    {
        return partTimerSprites;
    }

    public void ActiveHammerInfo(int index)
    {
        hammerInformation.gameObject.SetActive(true);
        hammerInformation.SetIndex(index);
    }

    public void SetHammer()
    {
        for (int i = 0; i < GameManager.Instance.CurrentUser.hammerList.Count; i++)
        {
            upgradePanels[i].SetActiveCheck();
        }

        upgradePanels[hammerInformation.GetIndex()].Mounting();
    }

    public void SetHammer(int index)
    {
        for (int i = 0; i < GameManager.Instance.CurrentUser.hammerList.Count; i++)
        {
            upgradePanels[i].SetActiveCheck();
        }

        upgradePanels[index].Mounting();
    }

    public PartTimerInformation partTimerInfo()
    {
        return partTimerInformation;
    }
}