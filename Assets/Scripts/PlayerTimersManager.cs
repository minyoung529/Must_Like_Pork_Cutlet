using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimersManager : MonoBehaviour
{
    public Animator[] partTiemrAnimators;

    void Start()
    {
        partTiemrAnimators = new Animator[GameManager.Instance.CurrentUser.partTimerList.Count];
        for (int i = 0; i < GameManager.Instance.CurrentUser.partTimerList.Count; i++)
        {
            partTiemrAnimators[i] = transform.GetChild(i).GetComponent<Animator>();
        }

        for (int i = 0; i < partTiemrAnimators?.Length; i++)
        {
            partTiemrAnimators[i].Play(GameManager.Instance.CurrentUser.partTimerList[i].englishName);
        }
    }
}
