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

    public bool isReward;
    public bool isSold;

    public void SetIsReward(bool isReward) { this.isReward = isReward; }

    public void SetIsSold(bool isSold)
    {
        this.isSold = isSold;
    }
}
