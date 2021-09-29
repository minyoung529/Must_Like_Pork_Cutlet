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

    private int index = 0;
    private int scriptIndex = 0;

    private bool isDoing;
    private bool isTexting;

    public void Tutorial()
    {
        if (!GameManager.Instance.CurrentUser.isTutorial)
        {
            Camera.main.transform.position = new Vector3(3f, 0f, 0f);
            storyPanel.gameObject.SetActive(true);
            tutorialPanel.SetActive(false);
            lobbyPanel.transform.DOScale(0f, 0f);
            SoundManager.Instance.TutorialBackground();
        }

        else
        {
            lobbyPanel.transform.DOScale(1f, 0f);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            index = tutorialScript.Count - 1;
            scriptIndex = tutorialScript[index].scripts.Count - 1;
            Next();
        }
    }
    public void Next()
    {
        if (index == tutorialScript.Count - 1 && scriptIndex == tutorialScript[index].scripts.Count - 1)
        {
            tutorialPanel.transform.DOScale(0f, 0.5f);
            GameManager.Instance.CurrentUser.isTutorial = true;
        }

        if (isDoing) return;
        if (isTexting) return;

        black.rectTransform.DOAnchorPos(Vector3.zero, 0.5f);
        isTexting = true;
        playerText.text = "";
        playerText.DOText(tutorialScript[index].scripts[scriptIndex], 1f).OnComplete(() => isTexting = false);

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
