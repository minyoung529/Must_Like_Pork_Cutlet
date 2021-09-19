using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestScroll : MonoBehaviour
{
    [SerializeField] private PanelBase panelBase;
    private Transform contents;

    void Start()
    {
        contents = transform.GetChild(0).GetChild(0);
        Debug.Log(GameManager.Instance.questManager.GetQuests().Count);
        InstantiatePanel(panelBase, contents, GameManager.Instance.questManager.GetQuests().Count);
    }

    public void InstantiatePanel(PanelBase template, Transform transform, int count)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
            panel = obj.GetComponent<PanelBase>();
            panel.Init(i);
            obj.SetActive(true);
            continue;
        }

        template.gameObject.SetActive(false);
    }
}
