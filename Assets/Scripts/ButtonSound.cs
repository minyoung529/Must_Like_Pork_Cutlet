using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerUpHandler
{
    enum SoundType
    {
        pop, bp, water, close
    }

    [SerializeField] SoundType soundType;

    public void OnPointerUp(PointerEventData eventData)
    {
        switch(soundType)
        {
            case SoundType.pop:
                SoundManager.Instance.PopButton();
                break;
            case SoundType.bp:
                SoundManager.Instance.BpButton();
                break;
            case SoundType.water:
                SoundManager.Instance.WaterButton();
                break;
            case SoundType.close:
                SoundManager.Instance.CancelButton();
                break;
        }
    }
}
