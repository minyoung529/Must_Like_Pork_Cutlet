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

    private int index = 0; 

    public void Next()
    {
        playerText.text = "";
        playerText.DOText(playerScript[index],1f);
        index++;
    }

}
