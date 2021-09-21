using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerInformation : MonoBehaviour
{
    [SerializeField] private Text grade;
    [SerializeField] protected Text itemName;
    [SerializeField] protected Text information;
    [SerializeField] protected Text ability;
    [SerializeField] private Text nextAbility;

    [SerializeField] protected Text levelText;

    [SerializeField] protected Image itemImage;
    [SerializeField] private Outline gradeOutline;

    [SerializeField] private Image nextItemImage;
    [SerializeField] private Outline nextGradeOutline;

    [SerializeField] private Text myAmountText;
    [SerializeField] private Text nextAmountText;
    [SerializeField] private Text fusionAmountText;

    [SerializeField] private Text fusionName;
    [SerializeField] private Text fusionNextHammerName;

    [SerializeField] private ParticleSystem particle;

    protected int index = 0;
    private int hammerAmount = 0;
    private Hammer hammer;
    private Hammer nextHammer;

    private bool isFirst = true;

    protected virtual void SetUp()
    {
        UpdateEnforce();

        if (isFirst)
            hammerAmount = hammer.amount / 5;

        itemImage.sprite = GameManager.Instance.UIManager.GetHammerSprites()[hammer.code];
        nextItemImage.sprite = GameManager.Instance.UIManager.GetHammerSprites()[nextHammer.code];
        myAmountText.text = string.Format("{0}(<color=red>-{1}</color>)", hammer.amount, hammerAmount * 5);
        nextAmountText.text = string.Format("{0}(<color=lime>+{1}</color>)", nextHammer.amount, hammerAmount);
        fusionAmountText.text = hammerAmount.ToString();
        fusionName.text = hammer.name;
        fusionNextHammerName.text = nextHammer.name;
        SetColor(hammer.grade, grade, gradeOutline);
        SetColor(nextHammer.grade, null, nextGradeOutline);
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
    }

    public int GetIndex()
    {
        return index;
    }

    private void UpdateEnforce()
    {
        grade.text = hammer.grade;
        itemName.text = hammer.name;
        levelText.text = hammer.level.ToString();
        information.text = hammer.info;
        ability.text
        = string.Format("´ÙÁü·Â {0}\n´ÙÁü´ç +{1}¿ø\nÄ¡¸íÅ¸ +{2}%",
        hammer.clickCount, hammer.clickPerMoney, hammer.criticalHit);
    }

    public void OnClickFusion()
    {
        if (hammerAmount == 0) return;
        hammer.amount -= hammerAmount * 5;
        nextHammer.amount += hammerAmount;
        particle.Play();
        SetUp();
    }
}
