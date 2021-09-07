using System.Collections.Generic;

[System.Serializable]
public class User
{
    private string nickname;
    public long onClickMoney;
    public long money;
    public List<PorkCutlet> cutletList = new List<PorkCutlet>();

    public string GetNickname()
    {
        return nickname;
    }
}
