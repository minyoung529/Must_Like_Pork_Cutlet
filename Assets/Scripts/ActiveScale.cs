using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveScale : MonoBehaviour
{
    public void Active()
    {
        gameObject.SetActive(true);
        transform.DOScale(1f, 0.4f);
    }

    public void InActive()
    {
        transform.DOScale(0f, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }
}
