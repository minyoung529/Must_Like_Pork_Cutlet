using UnityEngine;

[System.Serializable]
public class Cutlet
{
    public string name;
    public int addMoney;
    public ulong price;
    public int level;
    private bool isSold;


    public void LevelUp()
    {
        level++;
        price += (ulong) Mathf.RoundToInt(Mathf.Pow(level - 1, 2) * Mathf.Pow(level, 1.55f) - 1 * level + 22);
        addMoney += Mathf.RoundToInt(Mathf.Pow(level, 1.54f) * 3.7f);
        isSold = true;
    }

    public bool GetIsSold()
    {
        return isSold;
    }
}
