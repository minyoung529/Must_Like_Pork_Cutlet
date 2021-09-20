using UnityEngine;

[System.Serializable]
public class Hammer
{
    public int code;
    public int amount;
    public string name;
    public string grade;
    [TextArea]
    public string info;
    public int clickCount;
    public int clickPerMoney;
    public float criticalHit;
    public int level;
    private bool isSold;

    public bool GetIsSold()
    {
        return isSold;
    }

    public void SetIsSold(bool isSold)
    {
        this.isSold = isSold;
    }
}
