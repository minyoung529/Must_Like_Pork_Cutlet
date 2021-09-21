using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartTimersIndex : MonoBehaviour, IPointerClickHandler
{
    Animator animator;
    int index;

    private void Start()
    {
        index = transform.GetSiblingIndex();
        animator = GetComponent<Animator>();
        animator.Play(GameManager.Instance.CurrentUser.partTimerList[index].englishName);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.UIManager.partTimerInfo().SetIndex(index);
    }
}
