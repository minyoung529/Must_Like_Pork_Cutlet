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
    public bool isReward;


    public void LevelUp()
    {
        level++;

        price += (ulong)Mathf.RoundToInt(
            (Mathf.Pow(level + 1, 1.5f) *
            Mathf.Pow(level + 1, 1.5f) +
            Mathf.Pow(code, 1.2f)) *
            (code + 1) * 1.2f);


        addMoney += Mathf.RoundToInt(Mathf.Pow(level + 1.2f, 1.65f) * Mathf.Pow(code + 1.2f, 1.7f));
    }

    public int GetNextAddMoney()
    {
        if (!GetIsSold())
            return addMoney;

        return Mathf.RoundToInt(Mathf.Pow(level + 1.2f+1, 1.65f) * Mathf.Pow(code + 1.2f, 1.7f));
    }

    public bool GetIsSold()
    {
        return (level > 0);
    }

    public void SetPrice(ulong price)
    {
        if(code == 0)
        {
            this.price = 0;
            return;
        }
        this.price = price;
    }

    public void SetAddMoney(int addMoney)
    {
        this.addMoney = addMoney;
    }

    public void SetIsReward(bool isReward) { this.isReward = isReward; }

}
