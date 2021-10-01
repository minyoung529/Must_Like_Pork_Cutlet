using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookRewardButton : MonoBehaviour
{
    private Image image;
        
    void Start()
    {
        image = GetComponent<Image>();
    }

    public void Inactive()
    {
        image.color = Color.gray;
    }

    public void Active()
    {
        image.color = Color.white;
    }

    public void OnClickReward(IllustratedBookPanel panel)
    {
        panel.OnClickReward();
    }
}
