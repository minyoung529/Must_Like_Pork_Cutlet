using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActiveScale : MonoBehaviour
{
    public void Active()
    {
        transform.DOScale(1f, 0.2f);
    }

    public void InActive()
    {
        transform.DOScale(0f, 0.2f);
    }
}
