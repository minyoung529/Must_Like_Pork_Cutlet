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
        level++;

        price += (ulong)Mathf.RoundToInt(
            (Mathf.Pow(level + 1, 1.8f) *
            Mathf.Pow(level + 1, 1.25f) +
            Mathf.Pow(level, 1.2f) + code) *
            (code + 1) * 1.2f);


        addMoney += Mathf.RoundToInt(Mathf.Pow(level + 1, 1.54f) * Mathf.Pow(code + 1, 1.14f));
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

    public void SetAddMoney(int addMoney)
    {
        this.addMoney = addMoney;
    }
}
