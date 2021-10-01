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
    public int mpsOffset;
    public bool isReward;

    public void LevelUp()
    {
        level++;
        price += (ulong)Mathf.RoundToInt(Mathf.Pow(level, 2) * Mathf.Pow(code, 1.5f) - level + 22);
        mps += mpsOffset;
    }

    public bool GetIsSold()
    {
        return (level > 0);
    }

    public int GetNextMPS()
    {
        if (!GetIsSold()) return mps;
        return mps + mpsOffset;
    }

    public void SetMPS(int mps)
    {
        mpsOffset = mps;
        this.mps = mpsOffset;
    }

    public void SetPrice(ulong price)
    {
        this.price = price;
    }

    public void SetIsReward(bool isReward) { this.isReward = isReward; }
}
