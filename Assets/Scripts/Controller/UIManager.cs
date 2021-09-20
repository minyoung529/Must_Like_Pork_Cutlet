using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    #region 변수
    [Header("패널 템플릿")]
    [SerializeField] private PanelBase partTimerpanelTemplate;
    [SerializeField] private PanelBase hammerPanelTemplate;
    [SerializeField] private PanelBase cutletPanelTemplate;
    [SerializeField] private PanelBase randomHammerTemplate;

    [Header("각각 트랜스폼들")]
    [SerializeField] private Transform partTimersTransform;
    [SerializeField] private Transform cutletsTransform;

    [Header("재화 텍스트")]
    [SerializeField] private Text moneyText;
    [SerializeField] private Text countText;
    [SerializeField] private Text mpsAndCutletText;

    [Header("손님")]
    [SerializeField] private Image guest;

    [Header("망치")]
    [SerializeField] private SpriteRenderer playerHammer;

    [Header("랜덤 뽑기")]
    [SerializeField] private RectTransform randomHammerTransform;

    [Header("메인 스크롤")]
    [SerializeField] private ScrollRect scrollRect;

    private ErrorMessage message;

    private HammerInformation hammerInformation;
    private PartTimerInformation partTimerInformation;

    private int count = 0;

    private List<PanelBase> upgradePanels = new List<PanelBase>();
    private List<PanelBase> randomPanel = new List<PanelBase>();

    private CutletMove cutlet;
    #endregion

    private Sprite[] partTimerSprites;
    private Sprite[] cutletSprites;
    private Sprite[] hammerSprites;
    private Sprite[] guestSprites;

    private Image[] partTimersImage;
    private Image[] cutletsImage;
    private Image[] randomHammerImage = new Image[10];

    private void Start()
    {

        FirstSetting();
        ActivePartTimers();

        for (int i = 0; i < 10; i++)
        {
            randomHammerImage[i] = randomHammerTransform.GetChild(i + 1).gameObject.GetComponent<Image>();
        }
    }

    private void SettingUpgradePanel()
    {
        InstantiatePanel(hammerPanelTemplate, hammerPanelTemplate.transform.parent, GameManager.Instance.CurrentUser.hammerList.Count, hammerSprites);
        InstantiatePanel(cutletPanelTemplate, cutletPanelTemplate.transform.parent, GameManager.Instance.CurrentUser.cutlets.Count, cutletSprites);
        InstantiatePanel(partTimerpanelTemplate, partTimerpanelTemplate.transform.parent, GameManager.Instance.CurrentUser.partTimerList.Count, partTimerSprites);
        InstantiatePanel(randomHammerTemplate, randomHammerTransform, 10);
    }

    public void InstantiatePanel(PanelBase template, Transform transform, int count, Sprite[] sprites = null)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
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

        if (GameManager.Instance.RandomSelecting(5f))
        {
            cutlet.CriticalHit(1);
            return;
        }

        if (GameManager.Instance.RandomSelecting(GameManager.Instance.CurrentUser.GetHammer().criticalHit + 5f))
        {
            count += hammer.clickCount + 2;
            cutlet.CriticalHit(0);
        }

        else
        {
            count += hammer.clickCount;
        }

        countText.text = count.ToString();
        cutlet.Move();
        cutlet.SetSprite(count);

        if (count > GameManager.Instance.GetMaxCutletCnt() - 1)
        {
            GameManager.Instance.AddMoney(GameManager.Instance.GetCutletPrice(), true);
            GameManager.Instance.CurrentUser.Quests[0].PlusCurValue(1);
            GameManager.Instance.questManager.UpdateQuest();
            UpdatePanel();

            count -= 10;
            countText.text = count.ToString();
            cutlet.SetSprite(count);
            CutletsOnTable();
        }
    }

    public void UpdatePanel()
    {
        moneyText.text = string.Format("{0}원\n{1}", GameManager.Instance.CurrentUser.money, GameManager.Instance.CurrentUser.diamond);
        mpsAndCutletText.text = string.Format("돈가스 가격 {0}원 / 초당 {1}원", GameManager.Instance.GetCutletPrice(), GameManager.Instance.GetMPS());
        foreach (PanelBase upgradePanels in upgradePanels)
        {
            upgradePanels.Inactive();
        }
    }

    public void SetContent(RectTransform content)
    {
        scrollRect.content = content;
        hammerPanelTemplate.transform.parent.gameObject.SetActive(false);
        cutletPanelTemplate.transform.parent.gameObject.SetActive(false);
        partTimerpanelTemplate.transform.parent.gameObject.SetActive(false);
        content.gameObject.SetActive(true);
    }

    private void FirstSetting()
    {
        cutlet = FindObjectOfType<CutletMove>();
        message = FindObjectOfType<ErrorMessage>();
        hammerInformation = FindObjectOfType<HammerInformation>();
        partTimerInformation = FindObjectOfType<PartTimerInformation>();
        hammerInformation.gameObject.SetActive(false);
        partTimerInformation.gameObject.SetActive(false);
        message.gameObject.SetActive(false);

        List<PartTimer> partTimers = GameManager.Instance.CurrentUser.partTimerList;

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

        if (rand < 4) // 0-9
        {
            rand = Random.Range(rareCnt, hammerList.Count);
        }

        else if (rand < 32) //10 - 49
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

    public Sprite[] GetCutletSprite()
    {
        return cutletSprites;
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

    public void OnClickRandomHammer(int num)
    {
        if (GameManager.Instance.CurrentUser.diamond < num * 10)
        {
            message.OnClickMessage("돈이 부족합니다.", true);
            return;
        }

        RandomHammer(num);
        GameManager.Instance.AddDiamond(-num * 10);
        StartCoroutine(WaitingRandom(num));
        UpdatePanel();
    }

    private IEnumerator WaitingRandom(int amount)
    {
        float time = 0f;

        foreach (PanelBase randomPanel in randomPanel)
        {
            randomPanel.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.8f);

        randomHammerTransform.parent.parent.DOScale(1f, 0.4f).SetEase(Ease.InOutQuad).OnComplete(() => randomHammerTransform.parent.parent.DOShakeScale(1f));

        for (int i = 0; i < 10; i++)
        {
            if (i < amount)
            {
                if (randomPanel[i].GetHammer().grade == "rare")
                    time = 0.3f;
                else if (randomPanel[i].GetHammer().grade == "legendary")
                    time = 0.5f;
                else
                    time = 0f;

                randomPanel[i].transform.DOScale(0.3f, 0f);
                randomPanel[i].gameObject.SetActive(true);
                yield return new WaitForSeconds(time);
                randomPanel[i].transform.DOScale(1f, 0.3f);
            }

            else
            {
                randomPanel[i].gameObject.SetActive(false);
            }
        }
    }

}