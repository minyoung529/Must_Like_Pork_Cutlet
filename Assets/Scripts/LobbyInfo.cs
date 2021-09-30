using UnityEngine;
using DG.Tweening;

public class LobbyInfo : MonoBehaviour
{
    [SerializeField] Transform info;
    private bool isActive;

    public void OnClickInfo()
    {
        if(isActive)
        {
            SoundManager.Instance.PaperSound();
            info.DOScale(1, 0.3f);
            isActive = false;
        }
        else
        {
            SoundManager.Instance.PaperSound();
            info.DOScale(0, 0.3f);
            isActive = true;
        }
    }
}
