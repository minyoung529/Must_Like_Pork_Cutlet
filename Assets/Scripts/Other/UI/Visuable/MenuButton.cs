using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuButton : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject[] texts;
    List<RectTransform> children = new List<RectTransform>();
    private bool isActive = true;
    private IllustratedBookController bookButton;

    private int illustratedBook = 0;
    private int quest = 2;
    private float distance = 170f;

    private void Start()
    {
        bookButton = FindObjectOfType<IllustratedBookController>();

        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i).GetComponent<RectTransform>());
        }
    }

    private void Update()
    {
        CheckQuest();
        CheckIllustratedBook();

        if((CheckQuest() || CheckIllustratedBook()) && !isActive)
        {
            effect.SetActive(true);
        }

        else
        {
            effect.SetActive(false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isActive)
        {
            StartCoroutine(SetMenu());
            ActiveTexts(true);
        }

        else
        {
            StartCoroutine(UnSetMenu());
            ActiveTexts(false);
        }
    }

    private IEnumerator SetMenu()
    {
        float increasement = 0f;
        for (int i = 0; i < children.Count - 1; i++)
        {
            increasement -= distance;
            children[i].gameObject.SetActive(true);
            children[i].DOAnchorPosX(increasement, 0.1f).SetEase(Ease.Flash);
            yield return new WaitForSeconds(0.05f);
        }

        isActive = true;
        yield break;
    }

    private IEnumerator UnSetMenu()
    {
        for (int i = children.Count - 2; i > -1; i--)
        {
            children[i].DOAnchorPosX(0f, 0.1f).SetEase(Ease.Flash);
            yield return new WaitForSeconds(0.05f);
            children[i].gameObject.SetActive(false);
        }

        isActive = false;
        yield break;
    }

    private bool CheckQuest()
    {
        if(GameManager.Instance.QuestManager.CheckIsReward())
        {
            children[quest].GetChild(0).gameObject.SetActive(true);
            return true;
        }

        else
        {
            children[quest].GetChild(0).gameObject.SetActive(false);
            return false;
        }
    }

    private bool CheckIllustratedBook()
    {
        if(bookButton.CheckIsReward())
        {
            children[illustratedBook].GetChild(0).gameObject.SetActive(true);
            return true;
        }
        else
        {
            children[illustratedBook].GetChild(0).gameObject.SetActive(false);
            return false;
        }
    }

    private void ActiveTexts(bool isActive)
    {
        for(int i = 0; i<texts.Length; i++)
        {
            texts[i].SetActive(isActive);
        }
    }
}
