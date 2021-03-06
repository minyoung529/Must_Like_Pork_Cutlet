using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustratedBookPanel : PanelBase
{
    private int index;

    private PartTimer partTimer;
    private Cutlet cutlet;
    private Hammer hammer;

    [SerializeField] private Text infoText;
    [SerializeField] private Text detailInfoText;
    [SerializeField] private Image leftImage;
    [SerializeField] private GameObject plate;
    [SerializeField] private IllustratedBookController bookButton;
    [SerializeField] private Image reward;
    [SerializeField] private GameObject effect;

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

        SetData();
        SetUp();

        ActiveEffect();
    }

    private ButtonState JudgeState(int num)
    {
        switch (num)
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

    public override void SetUp()
    {
        switch (buttonState)
        {
            case ButtonState.cutlet:
                if (Compare(GameManager.Instance.CurrentUser.cutlets.Count)) break;
                itemImage.sprite = GameManager.Instance.UIManager.cutletSprites[index];
                Lock(cutlet.GetIsSold());
                break;

            case ButtonState.partTimer:
                if (Compare(GameManager.Instance.CurrentUser.partTimerList.Count)) break;
                itemImage.sprite = GameManager.Instance.UIManager.partTimerSprites[index];
                Lock(partTimer.GetIsSold());
                break;

            case ButtonState.hammer:
                if (Compare(GameManager.Instance.CurrentUser.hammerList.Count)) break;
                itemImage.sprite = GameManager.Instance.UIManager.hammerSprites[index];
                Lock(hammer.isSold);
                break;
        }

    }

    private bool Compare(int count)
    {
        if (index >= count)
        {
            gameObject.SetActive(false);
            return true;
        }
        else
        {
            gameObject.SetActive(true);
            return false;
        }
    }

    private void SetData()
    {
        User user = GameManager.Instance.CurrentUser;

        if (index < user.cutlets.Count)
        {
            cutlet = user.cutlets[index];
        }

        if (index < user.partTimerList.Count)
        {
            partTimer = user.partTimerList[index];
        }

        if (index < user.hammerList.Count)
        {
            hammer = user.hammerList[index];
        }
    }

    public void OnClickButton()
    {
        switch (buttonState)
        {
            case ButtonState.cutlet:
                if (cutlet == null) return;
                if (!cutlet.GetIsSold()) return;
                SoundManager.Instance.BpButton();
                SetLeftInformation(cutlet.name, cutlet.info, itemImage.sprite);
                bookButton.SetPanel(this);
                break;

            case ButtonState.partTimer:
                if (partTimer == null) return;
                if (!partTimer.GetIsSold()) return;
                SoundManager.Instance.BpButton();
                SetLeftInformation(partTimer.name, partTimer.story, itemImage.sprite);
                bookButton.SetPanel(this);
                break;

            case ButtonState.hammer:
                if (hammer == null) return;
                if (!hammer.isSold) return;
                SoundManager.Instance.BpButton();
                SetLeftInformation(hammer.name, hammer.info, itemImage.sprite);
                bookButton.SetPanel(this);
                break;
        }
    }

    private void SetLeftInformation(string name, string info, Sprite sprite)
    {
        if (buttonState == ButtonState.cutlet)
        {
            plate.SetActive(true);
        }
        else
        {
            plate.SetActive(false);
        }

        nameText.text = name;
        infoText.text = info;
        detailInfoText.text = info;
        leftImage.sprite = sprite;

        if (CheckIsRewarded())
        {
            reward.color = Color.green;
        }

        else
        {
            reward.color = Color.gray;
        }
    }

    private void Lock(bool isSold)
    {
        if (!isSold)
        {
            itemImage.color = Color.black;
            buttonImage.color = Color.gray;
        }
        else
        {
            itemImage.color = Color.white;
            buttonImage.color = Color.white;
        }
    }

    public bool IsReward()
    {
        if (hammer != null)
        {
            if (hammer.isSold && !hammer.isReward)
            {
                return true;
            }
        }

        if (partTimer != null)
        {
            if (partTimer.GetIsSold() && !partTimer.isReward)
            {
                return true;
            }
        }

        if (cutlet != null)
        {
            if (cutlet.GetIsSold() && !cutlet.isReward)
            {
                return true;
            }
        }

        return false;
    }

    public void OnClickReward()
    {
        if (!CheckIsRewarded()) return;
        GameManager.Instance.AddDiamond(30);
        SoundManager.Instance.RewardSound();

        switch (buttonState)
        {
            case ButtonState.cutlet:
                cutlet?.SetIsReward(true);
                break;

            case ButtonState.partTimer:
                partTimer?.SetIsReward(true);
                break;

            case ButtonState.hammer:
                hammer?.SetIsReward(true);
                break;
        }

        reward.color = Color.gray;
    }

    private bool CheckIsRewarded()
    {
        switch (buttonState)
        {
            case ButtonState.cutlet:
                if (cutlet == null) return false;
                if (cutlet.GetIsSold() && !cutlet.isReward)
                    return true;
                break;

            case ButtonState.partTimer:
                if (partTimer == null) return false;
                if (partTimer.GetIsSold() && !partTimer.isReward)
                    return true;
                break;

            case ButtonState.hammer:
                if (hammer == null) return false;
                if (hammer.isSold && !hammer.isReward)
                    return true;
                break;
        }

        return false;
    }

    public void ActiveEffect()
    {
        effect.SetActive(CheckIsRewarded());
    }
}