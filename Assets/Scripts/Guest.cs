using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Guest : MonoBehaviour
{
    [SerializeField] private Image guestImage;

    private void OnEnable()
    {
        StartCoroutine(GuestMove());
    }

    private void Despawn()
    {
        transform.SetParent(GameManager.Instance.Pool);
        gameObject.SetActive(false);
    }

    private IEnumerator GuestMove()
    {
        float randomX = Random.Range(-300.4f, 280f);
        transform.SetSiblingIndex(0);
        guestImage.sprite = GameManager.Instance.UIManager.guestSprites[Random.Range(0, 2)];
        transform.DOLocalMove(new Vector2(randomX, 150f), 0f);
        transform.DOLocalMove(new Vector2(randomX, 347f), 0.4f);

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.UIManager.SwapCutletsImage();
        SoundManager.Instance.NyamSound();
        yield return new WaitForSeconds(0.4f);

        transform.DOLocalMove(new Vector2(randomX, 150f), 0.3f).OnComplete(() => Despawn());
    }
}
