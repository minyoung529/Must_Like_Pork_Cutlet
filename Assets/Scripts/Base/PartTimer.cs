using UnityEngine;

[System.Serializable]
public class PartTimer
{
    public string name;
    public int level;
    public long price;
    public int mps;

    public void PlusMPS()
    {
        price = Mathf.RoundToInt(Mathf.Pow(level - 1, 2) * Mathf.Pow(level, 1.15f) - 1 * level + 22);  
    }
}
