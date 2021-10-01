using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CutletSoundButton : MonoBehaviour, IPointerUpHandler
{
    private bool isActive = false;
    private Image image;
    [SerializeField]
    private Sprite[] sprites;

    private int active = 0, inactive = 1;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isActive)
        {
            SoundManager.Instance.SetIsCutletSound(true);
            image.sprite = sprites[active];
            isActive = false;
        }

        else
        {
            SoundManager.Instance.SetIsCutletSound(false);
            image.sprite = sprites[inactive];
            isActive = true;
        }
    }
}
