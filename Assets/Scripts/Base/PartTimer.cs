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
        price += (ulong)Mathf.RoundToInt(Mathf.Pow(level, 2) * Mathf.Pow(code, 1.5f) - level + 22);

        if (mps == 1)
            mps++;
        else
            mps += (int)(mps * 0.7f);
    }

    public bool GetIsSold()
    {
        return (level > 0);
    }

    public int GetNextMPS()
    {
        return mps + (int)(mps * 0.7f);
    }

    public void SetMPS(int mps)
    {
        this.mps = mps;
    }

    public void SetPrice(ulong price)
    {
        this.price = price;
    }

    public void SetIsReward(bool isReward) { this.isReward = isReward; }
}
