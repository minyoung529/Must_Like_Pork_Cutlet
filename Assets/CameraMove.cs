using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    Camera mainCamera;
    private void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    public void FightCameraMoving()
    {
        transform.DOMoveY(-3f, 1f);
    }

    public void MainSceneMoving()
    {
        transform.DOMove(Vector3.zero, 1f);
    }

    public void StartRandomColor()
    {
        StartCoroutine(RandomColor());
    }

    private IEnumerator RandomColor()
    {
        for (int i = 0; i < 20; i++)
        {
            mainCamera.DOColor(new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255), 0.5f);
            yield return new WaitForSeconds(0.25f);
        }
    }
}
