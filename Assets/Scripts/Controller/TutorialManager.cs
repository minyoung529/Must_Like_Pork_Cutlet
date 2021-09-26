using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [TextArea]
    [SerializeField] private List<string> playerScript = new List<string>();
    [SerializeField] private Text playerText;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject storyPanel;
    [SerializeField] private GameObject lobbyPanel;

    private int index = 0;

    private void Start()
    {
        if(!GameManager.Instance.CurrentUser.isTutorial)
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
        if(index == playerScript.Count - 1)
        {
            tutorialPanel.SetActive(false);
        }

        playerText.text = "";
        playerText.DOText(playerScript[index],1f);
        index++;

        GameManager.Instance.CurrentUser.isTutorial = true;
    }

    private void OnEnable()
    {
        Next();
    }
}
