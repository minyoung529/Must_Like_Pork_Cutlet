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
        float randomX = Random.Range(-32.4f, 32.4f);
        transform.SetSiblingIndex(0);
        guestImage.sprite = GameManager.Instance.UIManager.GetGuestSprite()[Random.Range(0, 2)];
        transform.DOLocalMove(new Vector2(randomX, 20), 0f);
        transform.DOLocalMove(new Vector2(randomX, 45f), 0.4f);

        yield return new WaitForSeconds(0.9f);

        transform.DOLocalMove(new Vector2(randomX, 20f), 0.3f).OnComplete(() => Despawn());
        GameManager.Instance.UIManager.SwapCutletsImage();
    }
}
