using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerInformation : MonoBehaviour
{
    #region 정보 변수
    [SerializeField] private Text grade;
    [SerializeField] protected Text itemName;
    [SerializeField] protected Text information;
    [SerializeField] protected Text ability;

    [SerializeField] protected Text levelText;

    [SerializeField] protected Image itemImage;
    [SerializeField] private Outline gradeOutline;

    [SerializeField] private Image nextItemImage;
    [SerializeField] private Outline nextGradeOutline;
    #endregion

    #region 융합 변수
    [SerializeField] private Text myAmountText;
    [SerializeField] private Text nextAmountText;
    [SerializeField] private Text fusionAmountText;

    [SerializeField] private Text fusionName;
    [SerializeField] private Text fusionNextHammerName;

    [SerializeField] private ParticleSystem particle;
    #endregion

    protected int index = 0;
    private int hammerAmount = 0;
    private Hammer hammer;
    private Hammer nextHammer;

    private bool isFirst = true;

    protected virtual void SetUp()
    {
        UpdateInfo();

        if (isFirst)
        {
            hammerAmount = hammer.amount / 5;
        }

        itemImage.sprite = GameManager.Instance.UIManager.hammerSprites[hammer.code];

        myAmountText.text = string.Format("{0}(<color=red>-{1}</color>)", hammer.amount, hammerAmount * 5);

        if (hammer.code != GameManager.Instance.CurrentUser.hammerList.Count - 1)
        {
            fusionNextHammerName.text = nextHammer.name;
            nextItemImage.sprite = GameManager.Instance.UIManager.hammerSprites[nextHammer.code];
            nextAmountText.text = string.Format("{0}(<color=lime>+{1}</color>)", nextHammer.amount, hammerAmount);
            SetColor(nextHammer.grade, null, nextGradeOutline);
        }

        fusionAmountText.text = hammerAmount.ToString();
        fusionName.text = hammer.name;
        SetColor(hammer.grade, grade, gradeOutline);
        ActiveFusion();
    }

    public virtual void Next()
    {
        isFirst = true;

        if (index == GameManager.Instance.CurrentUser.hammerList.Count - 1)
        {
            index = -1;
        }

        index++;

        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index);

        if (index != GameManager.Instance.CurrentUser.hammerList.Count - 1)
        {
            nextHammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index + 1);
        }

        SetUp();
        ActiveFusion();
    }

    public virtual void Previous()
    {
        isFirst = true;

        if (index == 0) index = GameManager.Instance.CurrentUser.hammerList.Count;
        index--;

        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index);
        if (index != GameManager.Instance.CurrentUser.hammerList.Count - 1)
        {
            nextHammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index + 1);
        }

        SetUp();
        ActiveFusion();
    }

    public void Plus()
    {
        if (hammerAmount + 1 <= hammer.amount / 5)
        {
            isFirst = false;
            hammerAmount++;
            SetUp();
        }
    }

    public void Minus()
    {
        if (hammerAmount != 0)
        {
            isFirst = false;
            hammerAmount--;
            SetUp();
        }
    }
    private void SetColor(string hammerGrade, Text grade, Outline gradeOutline)
    {
        Color32 color;

        if (hammerGrade == "common")
            color = new Color32(159, 159, 159, 255);

        else if (hammerGrade == "rare")
            color = new Color32(191, 92, 255, 255);

        else if (hammerGrade == "legendary")
            color = new Color32(255, 230, 0, 255);

        else color = Color.white;

        gradeOutline.effectColor = color;

        if (grade == null) return;
        grade.color = color;
    }

    public virtual void SetIndex(int index)
    {
        this.index = index;
        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == this.index);

        if (this.index != GameManager.Instance.CurrentUser.hammerList.Count - 1)
        {
            nextHammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == this.index + 1);
        }

        SetUp();
        ActiveFusion();
    }

    public int GetIndex()
    {
        return index;
    }

    private void UpdateInfo()
    {
        grade.text = hammer.grade;
        itemName.text = hammer.name;
        levelText.text = hammer.level.ToString();
        information.text = hammer.info;
        ability.text
        = string.Format("다짐력 {0}\n다짐당 +{1}원\n치명타 +{2:0.0#}%",
        hammer.clickCount, hammer.clickPerMoney, hammer.criticalHit);
    }

    public void OnClickFusion()
    {
        if (hammerAmount == 0) return;
        hammer.amount -= hammerAmount * 5;
        nextHammer.amount += hammerAmount;
        nextHammer.SetIsSold(true);

        if (hammer.amount == 0)
        {
            GameManager.Instance.CurrentUser.UserHammer(nextHammer.name);
            GameManager.Instance.UIManager.MountingHammer(nextHammer.code);
        }

        GameManager.Instance.CurrentUser.Quests[5].PlusCurValue(hammerAmount);
        SoundManager.Instance.DdiringSound();
        particle.Play();
        SetUp();
    }

    private void ActiveFusion()
    {
        if (hammer.code == GameManager.Instance.CurrentUser.hammerList.Count - 1)
        {
            myAmountText.transform.parent.gameObject.SetActive(false);
        }

        else
        {
            myAmountText.transform.parent.gameObject.SetActive(true);
        }
    }

    public void Enforce()
    {
        if (!hammer.isSold) return;
        if (GameManager.Instance.CurrentUser.diamond < 10) return;

        GameManager.Instance.CurrentUser.diamond -= 10;
        GameManager.Instance.UIManager.UpdatePanel();
        hammer.Enforce();
        particle.Play();
        UpdateInfo();
    }
}
