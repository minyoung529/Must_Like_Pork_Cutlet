using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FightRandomColor : MonoBehaviour
{
    private Image image;
    private bool isEnable;
    private FightTimer fightTimer;

    void Start()
    {
        image = GetComponent<Image>();
        fightTimer = FindObjectOfType<FightTimer>();
    }

    private void OnEnable()
    {
        isEnable = true;
        StartCoroutine(RandomColor());
    }

    private void OnDisable()
    {
        isEnable = false;
        fightTimer.SetFillAmount(0f);
    }

    private IEnumerator RandomColor()
    {
        yield return new WaitForSeconds(3f);

        while (isEnable)
        {
            image.DOColor(new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255), 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
