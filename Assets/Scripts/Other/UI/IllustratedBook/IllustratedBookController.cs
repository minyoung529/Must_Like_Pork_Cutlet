using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustratedBookController : MonoBehaviour
{
    [SerializeField] private IllustratedBookPanel panelBase;
    private List<IllustratedBookPanel> panels = new List<IllustratedBookPanel>();

    private IllustratedBookPanel bookPanelTemplate;

    void Start()
    {
        User user = GameManager.Instance.CurrentUser;
        int count = Mathf.Max(user.cutlets.Count, user.partTimerList.Count, user.hammerList.Count);
        InstantiatePanel(panelBase, panelBase.transform.parent, count);

        OnClickIllustratedBook();
        panels[0].OnClickButton();
    }

    public void InstantiatePanel(IllustratedBookPanel template, Transform transform, int count)
    {
        GameObject obj;
        IllustratedBookPanel panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
            panel = obj.GetComponent<IllustratedBookPanel>();
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

    public void SetPanel(IllustratedBookPanel panel)
    {
        bookPanelTemplate = panel;
    }

    public void OnClickReward()
    {
        bookPanelTemplate.OnClickReward();

        foreach (IllustratedBookPanel panel in panels)
            panel.ActiveEffect();
    }

    public bool CheckIsReward()
    {
        foreach (IllustratedBookPanel panel in panels)
        {
            if (panel.IsReward())
            {
                return true;
            }
        }

        return false;
    }
}
