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
        => maxValue = value;

    public void SetCurValue(int value)
        => maxValue = value;

    public int GetReward()
    {
        return reward;
    }
}
