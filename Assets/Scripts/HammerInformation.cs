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

    protected int index = 0;
    private Hammer hammer;

    protected virtual void SetUp()
    {
        grade.text = hammer.grade;
        itemName.text = hammer.name;
        levelText.text = hammer.level.ToString();
        information.text = hammer.info;
        ability.text
        = string.Format("´ÙÁü·Â {0}\n´ÙÁü´ç +{1}¿ø\nÄ¡¸íÅ¸ +{2}%",
        hammer.clickCount, hammer.clickPerMoney, hammer.criticalHit);
        itemImage.sprite = GameManager.Instance.UIManager.GetHammerSprites()[hammer.code];
        SetColor();
    }

    public virtual void Next()
    {
        if (index == GameManager.Instance.CurrentUser.hammerList.Count - 1) index = -1;
        index++;
        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index);
        SetUp();
    }

    public virtual void Previous()
    {
        if (index == 0) index = GameManager.Instance.CurrentUser.hammerList.Count;
        index--;
        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index);
        SetUp();
    }

    private void SetColor()
    {
        Color32 color;

        if(hammer.grade == "common")
        {
            color = new Color32(159, 159, 159, 255);
            gradeOutline.effectColor = color;
            grade.color = color;
        }

        else if(hammer.grade == "rare")
        {
            color = new Color32(191, 92, 255, 255);
            gradeOutline.effectColor = color;
            grade.color = color;
        }

        else if(hammer.grade == "legendary")
        {
            color = new Color32(255, 230, 0, 255);
            gradeOutline.effectColor = color;
            grade.color = color;
        }
    }

    public virtual void SetIndex(int index)
    {
        this.index = index;
        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == this.index);
        SetUp();
    }

    public int GetIndex()
    {
        return index;
    }
}
