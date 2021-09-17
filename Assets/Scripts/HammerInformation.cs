using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HammerInformation : MonoBehaviour
{
    [SerializeField] private Text grade;
    [SerializeField] private Text hammerName;
    [SerializeField] private Text information;
    [SerializeField] private Text ability;
    [SerializeField] private Text nextAbility;
    [SerializeField] private Text levelText;
    [SerializeField] private Image hammerImage;
    [SerializeField] private Outline gradeOutline;

    private int index = 0;
    private Hammer hammer;

    private void SetUp()
    {
        grade.text = hammer.grade;
        hammerName.text = hammer.name;
        levelText.text = hammer.level.ToString();
        information.text = hammer.info;
        ability.text
        = string.Format("´ÙÁü·Â {0}\n´ÙÁü´ç +{1}¿ø\nÄ¡¸íÅ¸ +{2}%",
        hammer.clickCount, hammer.clickPerMoney, hammer.criticalHit);
        hammerImage.sprite = GameManager.Instance.uiManager.GetHammerSprites()[hammer.code];
        SetColor();
    }

    public void Next()
    {
        if (index == GameManager.Instance.CurrentUser.hammerList.Count - 1) index = -1;
        index++;
        hammer = GameManager.Instance.CurrentUser.hammerList.Find(x => x.code == index);
        Debug.Log(index);
        SetUp();
    }

    public void Previous()
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

    public void SetIndex(int index)
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
