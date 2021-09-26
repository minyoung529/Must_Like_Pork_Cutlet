using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : MonoBehaviour
{
    [SerializeField] private BookPanelTemplate panelBase;
    private List<BookPanelTemplate> panels = new List<BookPanelTemplate>();

    private BookPanelTemplate bookPanelTemplate;

    void Start()
    {
        User user = GameManager.Instance.CurrentUser;
        int count = Mathf.Max(user.cutlets.Count, user.partTimerList.Count, user.hammerList.Count);
        InstantiatePanel(panelBase, panelBase.transform.parent, count);
        OnClickCategory(0);
        panels[0].OnClickButton();
    }

    public void InstantiatePanel(BookPanelTemplate template, Transform transform, int count)
    {
        GameObject obj;
        BookPanelTemplate panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, transform);
            panel = obj.GetComponent<BookPanelTemplate>();
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

    public void SetPanel(BookPanelTemplate panel)
    {
        bookPanelTemplate = panel;
    }

    public void OnClickReward()
    {
        bookPanelTemplate.OnClickReward();

        foreach (BookPanelTemplate panel in panels)
            panel.ActiveEffect();
    }

    public bool CheckIsReward()
    {
        foreach (BookPanelTemplate panel in panels)
        {
            if (panel.IsReward())
            {
                return true;
            }
        }

        return false;
    }
}
