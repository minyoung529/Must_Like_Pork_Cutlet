using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanelTemplate : PanelBase
{
    [SerializeField] private Image bookItem;
    private Sprite[] sprites;
    private int index;

    private enum ButtonState
    {
        cutlet,
        partTimer,
        hammer,
        item
    }

    ButtonState buttonState;

    void Start()
    {
        buttonState = ButtonState.cutlet;
    }

    public override void Init(int num, Sprite sprite = null, int state = 0)
    {
        buttonState = JudgeState(state);
        index = num;
        SetUp();
        itemImage.color = Color.red;
    }

    private ButtonState JudgeState(int num)
    {
        switch(num)
        {
            case 0:
                return ButtonState.cutlet;
            case 1:
                return ButtonState.partTimer;
            case 2:
                return ButtonState.hammer;
            case 3:
                return ButtonState.item;
        }

        return ButtonState.cutlet;
    }

    protected override void SetUp()
    {
        switch (buttonState)
        {
            case ButtonState.cutlet:
                //if (Compare(GameManager.Instance.CurrentUser.cutlets.Count)) break;
                bookItem.sprite = GameManager.Instance.uiManager.GetCutletSprite()[index];
                break;

            case ButtonState.partTimer:
                if (Compare(GameManager.Instance.CurrentUser.partTimerList.Count)) break;
                bookItem.sprite = GameManager.Instance.uiManager.GetPartTimerSprite()[index];
                break;

            case ButtonState.hammer:
                if (Compare(GameManager.Instance.CurrentUser.hammerList.Count)) break;
                bookItem.sprite = GameManager.Instance.uiManager.GetHammerSprites()[index];
                break;
        }

    }

    private bool Compare(int count)
    {
        if (index > count)
        {
            gameObject.SetActive(false);
            return true;
        }
        return false;
    }
}
