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

    [Header("슬라이더")]
    [SerializeField] private Slider playerSlider;
    [SerializeField] private Slider enemySlider;

    [Header("파이트 패널")]
    [SerializeField] private GameObject fightPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private Text fightCountText;

    [Header("인게임 패널")]
    [SerializeField] private GameObject inGameCanvas;

    [Header("황금 돈가스")]
    [SerializeField] private Image goldCutlet;

    [Header("나가기 패널")]
    [SerializeField] private Image exitPanel;

    private ErrorMessage message;

    private HammerInformation hammerInformation;
    private PartTimerInformation partTimerInformation;

    private int count = 0;

    private List<PanelBase> upgradePanels = new List<PanelBase>();
    private List<PanelBase> randomPanel = new List<PanelBase>();

    private CutletMove cutlet;
    #endregion
    #region 스프라이트 변수
    public Sprite[] partTimerSprites { get; private set; }
    public Sprite[] cutletSprites { get; private set; }
    public Sprite[] hammerSprites { get; private set; }
    public Sprite[] guestSprites { get; private set; }
    #endregion

    private Image[] partTimersImage;
    private Image[] cutletsImage;

    private void Start()
    {
        FirstSetting();
        ActivePartTimers();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (fightPanel.activeSelf) return;
            exitPanel.gameObject.SetActive(true);
            exitPanel.transform.DOScale(1f, 0.4f);
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

    public void OnClickPork()
    {
        Hammer hammer = GameManager.Instance.CurrentUser.GetHammer();

        if (GameManager.Instance.RandomSelecting(0.5f))
        {
            cutlet.CriticalHit(1);
            return;
        }

        if (GameManager.Instance.isGoldCutlet)
        {
            GameManager.Instance.AddMoney((ulong)hammer.clickPerMoney * 2, true);
        }

        else
        {
            GameManager.Instance.AddMoney((ulong)hammer.clickPerMoney, true);
        }
        

        if (GameManager.Instance.RandomSelecting(GameManager.Instance.CurrentUser.GetHammer().criticalHit))
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
        SoundManager.Instance.CutletClickSound();

        if (count > GameManager.Instance.maxCutletCnt - 1)
        {
            if (!GameManager.Instance.CurrentUser.isTutorial)
            {
                GameManager.Instance.TutorialManager.SetIsDoing(false);
                GameManager.Instance.TutorialManager.Next();
            }

            if (GameManager.Instance.isGoldCutlet)
            {
                GameManager.Instance.AddMoney(GameManager.Instance.cutletMoney * 2, true);
            }

            else
            {
                GameManager.Instance.AddMoney(GameManager.Instance.cutletMoney, true);
            }

            GameManager.Instance.CurrentUser.Quests[0].PlusCurValue(1);

            SoundManager.Instance.MoneySound();
            UpdatePanel();

            count -= 10;
            countText.text = count.ToString();
            cutlet.SetSprite(count);
            CutletsOnTable();
        }
    }

    public void UpdatePanel()
    {
        moneyText.text = string.Format("{0}\n{1}",
            GameManager.Instance.ConvertMoneyText(GameManager.Instance.CurrentUser.money),
            GameManager.Instance.CurrentUser.diamond);

        if (GameManager.Instance.isGoldCutlet)
        {
            mpsAndCutletText.text = string.Format("돈가스 가격 {0} x 2 / 초당 {1} x 2/ 클릭당 {2} x 2",
    GameManager.Instance.ConvertMoneyText(GameManager.Instance.cutletMoney),
    GameManager.Instance.ConvertMoneyText(GameManager.Instance.mpsMoney),
    GameManager.Instance.ConvertMoneyText((ulong)GameManager.Instance.CurrentUser.GetHammer().clickPerMoney));

        }
        else
        {
            mpsAndCutletText.text = string.Format("돈가스 가격 {0} / 초당 {1} / 클릭당 {2}",
    GameManager.Instance.ConvertMoneyText(GameManager.Instance.cutletMoney),
    GameManager.Instance.ConvertMoneyText(GameManager.Instance.mpsMoney),
    GameManager.Instance.ConvertMoneyText((ulong)GameManager.Instance.CurrentUser.GetHammer().clickPerMoney));

        }

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
        partTimerSprites = Resources.LoadAll<Sprite>("Sprites/PartTimerImage");
        cutletSprites = Resources.LoadAll<Sprite>("Sprites/CutletImage");
        guestSprites = Resources.LoadAll<Sprite>("Sprites/Guest");
        hammerSprites = Resources.LoadAll<Sprite>("Sprites/Hammer");

        cutlet = FindObjectOfType<CutletMove>();
        message = FindObjectOfType<ErrorMessage>();
        hammerInformation = FindObjectOfType<HammerInformation>();
        partTimerInformation = FindObjectOfType<PartTimerInformation>();

        hammerInformation.gameObject.SetActive(false);
        partTimerInformation.gameObject.SetActive(false);

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
            if (GameManager.Instance.CurrentUser.cutlets[i].GetIsSold())
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
        GameObject obj;
        yield return new WaitForSeconds(time);

        if (!CheckPool("Guest"))
        {
            obj = Instantiate(guest.gameObject, guest.transform.parent);
        }

        else
        {
            obj = ReturnPoolObj("Guest");
            obj.transform.SetParent(guest.transform.parent);
        }

        obj.SetActive(true);
    }

    private bool CheckPool(string name)
    {
        Transform pool = GameManager.Instance.Pool;

        for (int i = 0; i < pool.childCount; i++)
        {
            if (pool.GetChild(i).name.Contains(name))
            {
                return true;
            }
        }

        return false;
    }

    private GameObject ReturnPoolObj(string name)
    {
        Transform pool = GameManager.Instance.Pool;

        for (int i = 0; i < pool.childCount; i++)
        {
            if (pool.GetChild(i).name.Contains(name))
            {
                return pool.GetChild(i).gameObject;
            }
        }

        return null;
    }
    public void SwapCutletsImage()
    {
        cutletsImage[0].color = Color.clear;
        cutletsImage[0].transform.SetAsLastSibling();

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

        float rand = Random.Range(0f, 100f);
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

        if (rand < 0.4f)
        {
            rand = Random.Range(rareCnt, hammerList.Count);
        }

        else if (rand < 3.4f)
        {
            rand = Random.Range(commonCnt, rareCnt);
        }

        else
        {
            rand = Random.Range(0, commonCnt);
        }

        return (int)rand;
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

    public void ActiveHammerInfo(int index)
    {
        hammerInformation.gameObject.SetActive(true);
        hammerInformation.transform.DOScale(1f, 0.3f);
        hammerInformation.SetIndex(index);
    }

    public void ActivePartTimerInfo(int index)
    {
        partTimerInformation.gameObject.SetActive(true);
        partTimerInformation.transform.DOScale(1f, 0.3f);
        partTimerInformation.SetIndex(index);
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

    public void OnClickRandomHammer(int num)
    {
        if (GameManager.Instance.CurrentUser.diamond < num * 10)
        {
            message.OnClickMessage("돈이 부족합니다.", true);
            return;
        }

        RandomHammer(num);
        GameManager.Instance.AddDiamond(-num * 10);
        GameManager.Instance.CurrentUser.Quests[5].PlusCurValue(num);
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

        SoundManager.Instance.TadaSound();
        randomHammerTransform.parent.parent.gameObject.SetActive(true);
        randomHammerTransform.parent.parent.DOScale(1f, 0.2f);

        for (int i = 0; i < 10; i++)
        {
            if (i < amount)
            {
                if (randomPanel[i].GetHammer().grade == "rare")
                    time = 0.4f;
                else if (randomPanel[i].GetHammer().grade == "legendary")
                    time = 0.8f;
                else
                    time = 0f;

                randomPanel[i].transform.DOScale(0f, 0f);
                yield return new WaitForSeconds(time);
                SoundManager.Instance.LevelUpSound();
                randomPanel[i].gameObject.SetActive(true);
                randomPanel[i].transform.DOScale(1f, 0.3f);
                yield return new WaitForSeconds(0.3f);
            }

            else
            {
                randomPanel[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickStart()
    {
        GameManager.Instance.isGameStart = true;
        Camera.main.transform.DOLocalMoveY(0f, 1f);
    }

    public void MountingHammer(int index)
    {
        for (int i = 0; i < GameManager.Instance.CurrentUser.hammerList.Count; i++)
        {
            upgradePanels[index].Mounting();
        }
    }

    public void StopRandomPanelParticle()
    {
        foreach (PanelBase panel in randomPanel)
        {
            panel.Inactive();
        }
    }

    public void PlayerClickInFight()
    {
        playerSlider.value++;
    }

    private IEnumerator EnemyClick()
    {
        float random;

        while (enemySlider.value != enemySlider.maxValue && playerSlider.value != playerSlider.maxValue)
        {
            random = Random.Range(0.05f, 0.15f);
            yield return new WaitForSeconds(random);
            enemySlider.value++;
        }

        if (enemySlider.value == enemySlider.maxValue)
        {
            defeatPanel.SetActive(true);
            defeatPanel.transform.DOScale(1f, 0.3f);
            SoundManager.Instance.DefeatSound();
        }

        else if (playerSlider.value == playerSlider.maxValue)
        {
            victoryPanel.SetActive(true);
            victoryPanel.transform.DOScale(1f, 0.3f);
            GameManager.Instance.CurrentUser.diamond += 10;
            SoundManager.Instance.VictorySound();
            GameManager.Instance.CurrentUser.Quests[6].PlusCurValue(1);
        }
    }

    public void FightPanelPosition()
    {
        fightPanel.transform.DOMoveY(-3, 0f);
    }

    public void StartFignt()
    {
        StartCoroutine(CountText());
    }

    private IEnumerator CountText()
    {
        int count = 3;
        fightCountText.transform.parent.DOScale(1f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            fightCountText.text = count.ToString();
            count--;
            SoundManager.Instance.CountSound();
            yield return new WaitForSeconds(1f);
        }
        fightCountText.transform.parent.DOScale(0f, 0.3f).SetEase(Ease.InBounce);
        StartCoroutine(EnemyClick());
    }

    public void ActiveInGame(bool isActive)
    {
        if (!isActive)
        {
            inGameCanvas.transform.DOScale(new Vector3(0, 1, 0), 0f);
        }

        if (isActive)
        {
            inGameCanvas.transform.DOScale(1f, 0f);

            playerSlider.value = 0;
            enemySlider.value = 0;
        }
    }

    public void ActiveGoldCutlet()
    {
        if (GameManager.Instance.isGoldCutlet) return;
        if (goldCutlet.gameObject.activeSelf) return;

        float rand = Random.Range(0, 100f);

        if (rand < 0.009f)
        {
            StartCoroutine(GoldCutlet());
        }
    }

    private IEnumerator GoldCutlet()
    {
        goldCutlet.gameObject.SetActive(true);
        goldCutlet.rectTransform.DOScale(1f, 0.5f);
        goldCutlet.rectTransform.localPosition = new Vector2(Random.Range(-1300f, 1300f), Random.Range(-580f, 580f));
        yield return new WaitForSeconds(7f);
        goldCutlet.rectTransform.DOScale(0f, 0.3f).OnComplete(() => goldCutlet.gameObject.SetActive(false));
    }

    public void SetBGMVolume(Slider slider)
        => GameManager.Instance.CurrentUser.bgmVolume = slider.value;

    public void SetEffectSoundVolume(Slider slider)
        => GameManager.Instance.CurrentUser.effectSoundVolume = slider.value;
}