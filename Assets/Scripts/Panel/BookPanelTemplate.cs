using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPanelTemplate : PanelBase
{
    private Sprite[] sprites;
    private int index;

    private PartTimer partTimer;
    private Cutlet cutlet;
    private Hammer hammer;

    [SerializeField] private Text infoText;
    [SerializeField] private Text detailInfoText;
    [SerializeField] private Image leftImage;
    [SerializeField] private GameObject plate;

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

    public override void SetUp()
    {
        switch (buttonState)
        {
            case ButtonState.cutlet:
                if (Compare(GameManager.Instance.CurrentUser.cutlets.Count)) break;
                Debug.Log(itemImage.sprite);
                Debug.Log(GameManager.Instance.UIManager.GetCutletSprite());
                Debug.Log(GameManager.Instance.UIManager.GetCutletSprite()[index]);
                Debug.Log(GameManager.Instance.UIManager);
                Debug.Log(itemImage);

                itemImage.sprite = GameManager.Instance.UIManager.GetCutletSprite()[index];
                Lock(cutlet.GetIsSold());
                break;

            case ButtonState.partTimer:
                if (Compare(GameManager.Instance.CurrentUser.partTimerList.Count)) break;
                itemImage.sprite = GameManager.Instance.UIManager.GetPartTimerSprite()[index];
                Lock(partTimer.GetIsSold());
                break;

            case ButtonState.hammer:
                if (Compare(GameManager.Instance.CurrentUser.hammerList.Count)) break;
                itemImage.sprite = GameManager.Instance.UIManager.GetHammerSprites()[index];
                Lock(hammer.GetIsSold());
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

        if(index < user.cutlets.Count)
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
        switch(buttonState)
        {
            case ButtonState.cutlet:
                if (!cutlet.GetIsSold()) return;
                SetLeftInformation(cutlet.name, cutlet.info, itemImage.sprite);
                break;

            case ButtonState.partTimer:
                if (!partTimer.GetIsSold()) return;
                SetLeftInformation(partTimer.name, partTimer.story, itemImage.sprite);
                break;

            case ButtonState.hammer:
                if (!hammer.GetIsSold()) return;
                SetLeftInformation(hammer.name, hammer.info, itemImage.sprite);
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
            plate.SetActive(false);

        nameText.text = name;
        infoText.text = info;
        detailInfoText.text = info;
        leftImage.sprite = sprite;
    }

    private void Lock(bool isSold)
    {
        if(!isSold)
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
}
