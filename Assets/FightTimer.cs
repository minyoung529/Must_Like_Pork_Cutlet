using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightTimer : MonoBehaviour, IPointerUpHandler
{
    int maxTime = 360;
    int curTime = 0;

    
    private bool isFight = false;

    CameraMove cameraMove;

    [SerializeField] GameObject fight;
    [SerializeField] Text timerText;
    [SerializeField] Text enemyText;
    [SerializeField] Image fillImage;

    void Start()
    {
        cameraMove = Camera.main.GetComponent<CameraMove>();
        StartCoroutine(Timer());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isFight) return;
        GameManager.Instance.UIManager.ActiveInGame(false);
        cameraMove.FightCameraMoving();
        GameManager.Instance.UIManager.StartFignt();
        fight.SetActive(true);
        SoundManager.Instance.OnClickFight();
        GameManager.Instance.CurrentUser.neighborFight++;
        enemyText.text = string.Format("¿·Áý µ·°¡½º ¾ÆÀú¾¾ / Level {0}", GameManager.Instance.CurrentUser.neighborFight);
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (fillImage.fillAmount != 1)
            {
                if (isFight)
                {
                    timerText.gameObject.SetActive(true);
                }

                curTime--;
                fillImage.fillAmount += (float)(1f / maxTime);

                if (curTime / 60 < 1)
                    timerText.text = string.Format("{0}ÃÊ", curTime % 60);

                else
                    timerText.text = string.Format("{0}ºÐ {1}ÃÊ", curTime / 60, curTime % 60);

                isFight = false;
            }

            else
            {
                fillImage.fillAmount = Mathf.Clamp(fillImage.fillAmount, 0, 1f);

                if (!isFight)
                {
                    timerText.gameObject.SetActive(false);
                    curTime = 360;
                }
                isFight = true;
            }
        }
    }

    public void SetFillAmount(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
