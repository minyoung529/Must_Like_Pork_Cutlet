using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private List<Quest> questLists;

    public List<Quest> GetQuests()
    {
        return questLists;
    }
}
