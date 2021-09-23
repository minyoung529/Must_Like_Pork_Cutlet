using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<Quest> questLists;
    private List<QuestPanel> questPanels = new List<QuestPanel>();

    [SerializeField] private QuestPanel panelBase;
    [SerializeField] private Transform contents;

    void Start()
    {
        questLists = GameManager.Instance.CurrentUser.Quests;
        InstantiatePanel(panelBase, contents, questLists.Count);
    }

    public void InstantiatePanel(QuestPanel template, Transform transform, int count)
    {
        GameObject obj;
        QuestPanel panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
            obj.name = string.Format("Quest {0}", i);
            panel = obj.GetComponent<QuestPanel>();
            questPanels.Add(panel);
            panel.Init(i);
            obj.SetActive(true);
            continue;
        }

        template.gameObject.SetActive(false);
    }

    public void UpdateQuest()
    {
        foreach (QuestPanel panel in questPanels)
        {
            panel.SetUp();
        }
    }

    public bool CheckIsReward()
    {
        foreach (QuestPanel panel in questPanels)
        {
            if (panel.IsReward)
            {
                return true;
            }
        }

        return false;
    }
}