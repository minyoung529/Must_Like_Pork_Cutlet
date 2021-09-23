using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveScale : MonoBehaviour
{
    public void Active()
    {
        gameObject.SetActive(true);
        transform.DOScale(0f, 0f);
        transform.DOScale(1f, 0.4f);
    }

    public void Inactive()
    {
        SoundManager.Instance.CancelButton();
        transform.DOScale(new Vector3(0f, 1f, 0f), 0.2f);
    }
}
