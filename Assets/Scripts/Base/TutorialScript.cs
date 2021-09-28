using System.Collections.Generic;

[System.Serializable]
public class TutorialScript
{
    [UnityEngine.TextArea(3,10)]
    public List<string> scripts = new List<string>();
}
