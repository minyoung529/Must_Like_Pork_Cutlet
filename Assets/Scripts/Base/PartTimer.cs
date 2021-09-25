using UnityEngine;

[System.Serializable]
public class PartTimer
{
    public int code;
    public string name;
    public string englishName;
    [TextArea]
    public string story;
    public int level;
    public ulong price;
    public int mps;
    public bool isReward;

    public void LevelUp()
    {
        level++;
        price += (ulong)Mathf.RoundToInt(Mathf.Pow(level - 1, 2) * Mathf.Pow(level, 1.15f) - 1 * level + 22);
        mps += (int)(mps * 1.7f);
    }

    public bool GetIsSold()
    {
        return (level > 0);
    }

    public void SetPrice(ulong price)
    {
        this.price = price;
    }

    public void SetIsReward(bool isReward) { this.isReward = isReward; }
}
