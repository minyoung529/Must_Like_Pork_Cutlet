using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : MonoBehaviour
{
    [SerializeField] private PanelBase panelBase;
    private List<PanelBase> panels = new List<PanelBase>();

    void Start()
    {
        User user = GameManager.Instance.CurrentUser;
        int count = Mathf.Max(user.cutlets.Count, user.partTimerList.Count, user.hammerList.Count);
        InstantiatePanel(panelBase, panelBase.transform.parent, count);
    }

    public void InstantiatePanel(PanelBase template, Transform transform, int count)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
            panel = obj.GetComponent<PanelBase>();
            panels.Add(panel);
            obj.SetActive(true);
            continue;
        }

        template.gameObject.SetActive(false);
    }

    public void OnClickCategory(int num)
    {
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].Init(i, null, num);
        }
    }

    public void OnClickIllustratedBook()
    {
        OnClickCategory(0);
    }
}
