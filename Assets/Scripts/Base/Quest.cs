using UnityEngine;

[System.Serializable]
public class Quest
{
    public int code;
    public string questName;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private int currentValue;
    [SerializeField]
    private int reward;

    public int GetMaxValue()
    {
        return maxValue;
    }

    public int GetCurValue()
    {
        return currentValue;
    }

    public void SetMaxValue(int value)
    {
        maxValue = value;
    }

    public void SetCurValue(int value)
    {
        if (currentValue >= maxValue) return;
        currentValue = value;
    }

    public void PlusCurValue(int value)
    {
        if (currentValue >= maxValue) return;
        currentValue += value;
    }

    public void SetCurValueZero()
    => currentValue = 0;

    public int GetReward()
    {
        return reward;
    }
}
