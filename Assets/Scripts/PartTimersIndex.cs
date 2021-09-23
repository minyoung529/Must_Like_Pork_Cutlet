using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PartTimersIndex : MonoBehaviour, IPointerClickHandler
{
    private Animator animator;
    private int index;
    private Image image;

    private void Start()
    {
        index = transform.GetSiblingIndex();
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        animator.Play(GameManager.Instance.CurrentUser.partTimerList[index].englishName);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (image.color.a == 0) return;
        if (transform.parent.parent.name.Contains("Lobby")) return;
        GameManager.Instance.UIManager.ActivePartTimerInfo(index);
        SoundManager.Instance.WaterButton();
    }
}
