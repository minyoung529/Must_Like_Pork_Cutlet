using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutletMove : MonoBehaviour
{
    private ParticleSystem particle;
    private ParticleSystemRenderer particleRenderer;
    private SpriteRenderer spriteRenderer;
    private Sprite[] cutletSprites;

    [SerializeField] private SpriteRenderer hammer;
    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material criticalMaterial;

    [SerializeField] private SpriteRenderer criticalUI;
    [SerializeField] private Sprite[] sprites;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //나중에 옮기기
        cutletSprites = Resources.LoadAll<Sprite>("Sprites/Cutlet");
    }

    public void Move()
    {
        particle.Play();

        if (GameManager.Instance.UIManager.GetCount() > GameManager.Instance.GetMaxCutletCnt() - 1)
        {
            StartCoroutine(Moving());
        }
    }

    private IEnumerator Moving()
    {
        transform.DOMove(new Vector2(-2.7f, transform.position.y), 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(-1.3f, transform.position.y);
    }

    public void SetSprite(int num)
    {
        if (num > -1)
            spriteRenderer.sprite = cutletSprites[0];
        if (num > 3)
            spriteRenderer.sprite = cutletSprites[1];
        if (num > 6)
            spriteRenderer.sprite = cutletSprites[2];
    }

    [System.Obsolete]
    public void CriticalHit(int num)
    {
        StartCoroutine(CriticalUI(num));

        if (num == 1) return;

        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
        particle.startSize = 0.3f; //이ㅏㅅㅇ해ㅠㅠ
        particleRenderer.material = criticalMaterial;
        particle.DOPlay();
        StartCoroutine(WaitingToChangeMaterial());
        particle.startSize = 0.15f;
    }

    [System.Obsolete]
    private IEnumerator WaitingToChangeMaterial()
    {
        yield return new WaitForSeconds(particle.duration);
        particleRenderer.material = originalMaterial;
    }

    private IEnumerator CriticalUI(int num)
    {
        float time = 0.2f;

        criticalUI.transform.position = new Vector2(Random.Range(-1.6f, -0.9f), Random.Range(-0.7f, 0.45f));
        criticalUI.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15f, 15f)));
        criticalUI.transform.DOScale(0.7f, 0.35f);
        criticalUI.sprite = sprites[num];

        yield return new WaitForSeconds(0.35f);

        criticalUI.transform.DOScale(0f, time / 2);
    }
}