using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookButton : MonoBehaviour
{
    [SerializeField] private PanelBase panelBase;
    private RectTransform contents;
    private List<PanelBase> panels = new List<PanelBase>();

    void Start()
    {
        User user = GameManager.Instance.CurrentUser;
        int count = Mathf.Max(user.cutlets.Count, user.partTimerList.Count, user.hammerList.Count);
        contents = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        InstantiatePanel(panelBase, contents, count);
    }

    public void InstantiatePanel(PanelBase template, RectTransform rectTransform, int count)
    {
        GameObject obj;
        PanelBase panel;

        for (int i = 0; i < count; i++)
        {
            obj = Instantiate(template.gameObject, rectTransform);
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
            panelBase.Init(i, null, num);
        }
    }
}
