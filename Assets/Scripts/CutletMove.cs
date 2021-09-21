using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CutletMove : MonoBehaviour
{
    private ParticleSystem particle;
    private ParticleSystem.MainModule psMain;
    private ParticleSystemRenderer particleRenderer;
    private SpriteRenderer spriteRenderer;
    private Sprite[] cutletSprites;

    [SerializeField] private Material originalMaterial;
    [SerializeField] private Material criticalMaterial;

    [SerializeField] private SpriteRenderer criticalUI;
    [SerializeField] private Sprite[] sprites;

    [SerializeField] private Transform offset;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        psMain = particle.main;
        particleRenderer = particle.GetComponent<ParticleSystemRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //나중에 옮기기
        cutletSprites = Resources.LoadAll<Sprite>("Sprites/Cutlet");
        transform.position = new Vector2(offset.position.x, transform.position.y);
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
        transform.DOMove(new Vector2(offset.position.x - 2f, transform.position.y), 0.2f);
        yield return new WaitForSeconds(0.2f);
        transform.position = new Vector3(offset.position.x, transform.position.y);
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

    public void CriticalHit(int num)
    {
        StartCoroutine(CriticalUI(num));

        if (num == 1) return;

        psMain.startSize = 0.3f;
        particleRenderer.material = criticalMaterial;
        particle.DOPlay();
        StartCoroutine(WaitingToChangeMaterial());
        psMain.startSize = 0.15f;
    }

    private IEnumerator WaitingToChangeMaterial()
    {
        yield return new WaitForSeconds(psMain.duration);
        particleRenderer.material = originalMaterial;
    }

    private IEnumerator CriticalUI(int num)
    {
        criticalUI.transform.DOScale(1.3f,0.1f);
        criticalUI.sprite = sprites[num];

        yield return new WaitForSeconds(0.2f);

        criticalUI.transform.localScale = Vector2.zero; ;
    }
}