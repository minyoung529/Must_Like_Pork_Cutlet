using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private List<Quest> questLists;
    private List<PanelBase> questPanels = new List<PanelBase>();

    [SerializeField] private PanelBase panelBase;
    [SerializeField] private Transform contents;

    void Start()
    {
        questLists = GameManager.Instance.CurrentUser.Quests;
        InstantiatePanel(panelBase, contents, questLists.Count);
    }

    public void InstantiatePanel(PanelBase template, Transform transform, int count)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
            panel = obj.GetComponent<PanelBase>();
            questPanels.Add(panel);
            panel.Init(i);
            obj.SetActive(true);
            continue;
        }

        template.gameObject.SetActive(false);
    }

    public void UpdateQuest()
    {
        foreach(PanelBase panel in questPanels)
        {
            panel.SetUp();
        }
    }
}
