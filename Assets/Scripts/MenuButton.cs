using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuButton : MonoBehaviour, IPointerUpHandler
{
    List<RectTransform> children = new List<RectTransform>();
    private bool isActive;


    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive)
        {
            StartCoroutine(SetMenu());
        }

        else
        {
            StartCoroutine(UnSetMenu());
        }
    }

    private IEnumerator SetMenu()
    {
        float increasement = 0f;
        for (int i = 0; i < children.Count; i++)
        {
            increasement -= 14.5f;
            children[i].gameObject.SetActive(true);
            children[i].DOAnchorPosX(increasement, 0.1f).SetEase(Ease.Flash);
            yield return new WaitForSeconds(0.05f);
        }
        isActive = true;
        yield break;
    }

    private IEnumerator UnSetMenu()
    {
        float decreasement = 14.5f * (children.Count - 1);

        for (int i = children.Count - 1; i > -1; i--)
        {
            children[i].DOAnchorPosX(0, 0.1f).SetEase(Ease.Flash);
            decreasement -= 14.5f;
            yield return new WaitForSeconds(0.05f);
            children[i].gameObject.SetActive(false);
        }

        isActive = false;
        yield break;
    }
}
