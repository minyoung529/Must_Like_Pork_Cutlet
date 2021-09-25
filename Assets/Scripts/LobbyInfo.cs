using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyInfo : MonoBehaviour
{
    [SerializeField] ActiveScale info;
    private bool isActive;

    public void OnClickInfo()
    {
        if(isActive)
        {
            SoundManager.Instance.PaperSound();
            info.Inactive();
            isActive = false;
        }
        else
        {
            SoundManager.Instance.PaperSound();
            info.Active();
            isActive = true;
        }
    }
}
