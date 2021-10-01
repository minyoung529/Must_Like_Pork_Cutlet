using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    private Text message;
    private Image messageColor;

    void Start()
    {
        message = GetComponentInChildren<Text>();
        messageColor = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void OnClickMessage(string message, bool isError)
    {
        if(isError)
        {
            messageColor.color = new Color32(164, 0, 0, 255);
        }

        else
        {
            messageColor.color = new Color32(116, 248, 45, 255);
        }

        gameObject.SetActive(true);
        StartCoroutine(Effect(message));
    }

    private IEnumerator Effect(string message)
    {
        transform.DOScale(1f, 0.3f);
        this.message.text = message;
        yield return new WaitForSeconds(0.8f);
        transform.DOScale(0f, 0.2f);
        this.message.text = "";
        gameObject.SetActive(false);
    }
}
