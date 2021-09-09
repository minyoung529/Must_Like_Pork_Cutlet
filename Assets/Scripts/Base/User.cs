using System.Collections.Generic;

[System.Serializable]
public class User
{
    private string nickname;
    public ulong onClickMoney;
    public ulong money;
    public List<PartTimer> partTimerList;
    public List<Hammer> hammerList;
    public List<Cutlet> cutlets;

    public string GetNickname()
    {
        return nickname;
    }
}