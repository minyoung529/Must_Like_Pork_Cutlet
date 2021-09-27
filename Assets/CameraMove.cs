using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMove : MonoBehaviour
{
    public void FightCameraMoving()
    {
        transform.DOMoveY(-3f, 1f);
    }

    public void MainSceneMoving()
    {
        transform.DOMove(Vector3.zero, 1f);
    }
}
