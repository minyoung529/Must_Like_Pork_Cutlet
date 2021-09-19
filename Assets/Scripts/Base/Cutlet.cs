using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cutlet
{
    public int code;
    public string name;
    [TextArea]
    public string info;
    public int addMoney;
    public ulong price;
    public int level;
    public bool isSold;


    public void LevelUp()
    {
        // 0 1 2 3 4 
        level++;
        price += (ulong)
            Mathf.RoundToInt(Mathf.Pow(level - 1, 2) *
            (Mathf.Pow(level, code == 0 ? 2 : 2 + code * 0.06f) - 1 * level + 50) *
            Mathf.Pow(1.6f, code));

        addMoney += Mathf.RoundToInt(Mathf.Pow(level, 1.54f) * 2.9f);
        isSold = true;
    }

    public bool GetIsSold()
    {
        return isSold;
    }

    public void SetPrice(ulong price)
    {
        this.price = price;
    }
}
