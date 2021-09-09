using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutletMove : MonoBehaviour
{
    public void Move()
    {
        if (GameManager.Instance.uiManager.GetCount() > GameManager.Instance.GetMaxCutletCnt() - 1)
        {
            StartCoroutine(Moving());
        }
    }

    private IEnumerator Moving()
    {
        transform.DOMove(new Vector2(-2.7f, transform.position.y), 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(-1.3f, transform.position.y);
    }
}
