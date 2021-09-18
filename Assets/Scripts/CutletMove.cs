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

    [SerializeField] private GameObject criticalUI;

    private void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        particleRenderer = particle.gameObject.GetComponent<ParticleSystemRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cutletSprites = Resources.LoadAll<Sprite>("Sprites/Cutlet");
    }

    public void Move()
    {
        particle.Play();

        if (GameManager.Instance.uiManager.GetCount() > GameManager.Instance.GetMaxCutletCnt() - 1)
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
    public void CriticalHit()
    {
        StartCoroutine(CriticalUI());
        hammer.DOColor(Color.red, 0.1f).OnComplete(() => hammer.DOColor(Color.white, 0.1f));
        spriteRenderer.DOColor(Color.red, 0.1f).OnComplete(() => spriteRenderer.DOColor(Color.white, 0.1f));
        //particleMaterial.SetColor("_Color", new Color32(255, 0, 0, 255));
        particle.startSize = 0.3f;
        particleRenderer.material = criticalMaterial;
        particle.DOPlay();
        StartCoroutine(WaitingToChangeMaterial());
        //particleMaterial.SetColor("_Color", new Color32(255, 255, 255, 255));
        particle.startSize = 0.15f;
    }

    [System.Obsolete]
    private IEnumerator WaitingToChangeMaterial()
    {
        yield return new WaitForSeconds(particle.duration);
        particleRenderer.material = originalMaterial;
    }

    private IEnumerator CriticalUI()
    {
        float time = 0.2f;
        criticalUI.transform.position = new Vector2(Random.Range(-1.6f, -0.9f), Random.Range(-0.7f, 0.45f));
        criticalUI.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(-15f, 15f)));
        criticalUI.transform.DOScale(0.7f, time * 2f);
        yield return new WaitForSeconds(time * 2f);
        criticalUI.transform.DOScale(0f, time / 2);
    }
}