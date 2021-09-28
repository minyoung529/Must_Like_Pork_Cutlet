using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightTimer : MonoBehaviour, IPointerUpHandler
{
    float maxTime = 120f;

    Image fillImage;
    private bool isFight = false;

    CameraMove cameraMove;

    [SerializeField]
    GameObject fight;

    void Start()
    {
        cameraMove = Camera.main.GetComponent<CameraMove>();
        fillImage = transform.GetChild(0).GetComponent<Image>();
        StartCoroutine(Timer());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isFight) return;
        GameManager.Instance.UIManager.ActiveInGame(false);
        cameraMove.FightCameraMoving();
        GameManager.Instance.UIManager.StartFignt();
        fight.SetActive(true);
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if(fillImage.fillAmount != 1)
            {
                fillImage.fillAmount += 1 / maxTime;
                isFight = false;
            }

            else
            {
                fillImage.fillAmount = Mathf.Clamp(fillImage.fillAmount, 0, 1f);
                isFight = true;
            }
        }
    }

    public void SetFillAmount(float amount)
    {
        fillImage.fillAmount = amount;
    }
}
