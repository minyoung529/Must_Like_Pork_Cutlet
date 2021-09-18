using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class User
{
    public string nickname;
    public ulong onClickMoney;
    public ulong money;
    [SerializeField] private string userHammer;
    public List<PartTimer> partTimerList;
    public List<Hammer> hammerList;
    public List<Cutlet> cutlets;

    public string GetNickname()
    {
        return nickname;
    }

    public string UserHammer()
    {
        return userHammer;
    }

    public Hammer GetHammer()
    {
        Hammer hammer = hammerList.Find(x => x.name == userHammer);
        if (hammer == null) return hammerList[0];
        else return hammer;
    }

    public void UserHammer(string hammer)
    {
        userHammer = hammer;
    }

}