using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<TutorialScript> tutorialScript = new List<TutorialScript>();

    [SerializeField] private Text playerText;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Image black;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private Image arrow;

    private int index = 0;
    private int scriptIndex = 0;

    private bool isDoing;

    private void Start()
    {
        if (!GameManager.Instance.CurrentUser.isTutorial)
        {
            Camera.main.transform.position = new Vector3(3, 0, 0);
            storyPanel.gameObject.SetActive(true);
            tutorialPanel.SetActive(false);
            lobbyPanel.SetActive(false);
        }

        else
        {
            lobbyPanel.SetActive(true);
        }
    }
    public void Next()
    {
        if(index == tutorialScript.Count - 1 && scriptIndex==tutorialScript[index].scripts.Count - 1)
        {
            tutorialPanel.transform.DOScale(0f, 0.5f);
            GameManager.Instance.CurrentUser.isTutorial = true;
        }

        if (isDoing) return;
        black.rectTransform.DOAnchorPos(Vector3.zero, 0.5f);

        playerText.text = "";
        playerText.DOText(tutorialScript[index].scripts[scriptIndex], 1f);

        if (scriptIndex != tutorialScript[index].scripts.Count - 1)
        {
            scriptIndex++;
        }

        else
        {
            isDoing = true;
            StartCoroutine(Tutorial(index));
        }
    }

    private void OnEnable()
    {
        Next();
    }

    private IEnumerator Tutorial(int num)
    {
        switch (num)
        {
            case 0:
                black.rectTransform.DOAnchorPosX(-1015, 1f);
                yield return new WaitForSeconds(1f);
                arrow.rectTransform.position = new Vector2(1300, 526);

                break;
            case 1:
                black.rectTransform.DOAnchorPosX(1015, 1f);
                break;
        }
        scriptIndex = 0;
        index++;

    }

    public void SetIsDoing(bool isDoing)
    {
        this.isDoing = isDoing;
    }
}
