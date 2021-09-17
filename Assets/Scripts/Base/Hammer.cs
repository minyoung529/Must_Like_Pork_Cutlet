using UnityEngine;

[System.Serializable]
public class Hammer
{
    public int code;
    public string name;
    public string grade;
    [TextArea]
    public string info;
    public int clickCount;
    public int clickPerMoney;
    public float criticalHit;
    public int level;
    public bool isSold;

    public bool GetIsSold()
    {
        return isSold;
    }
}
