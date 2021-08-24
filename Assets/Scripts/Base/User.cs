using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string nickname;
    public long energy;
    public List<Soldier> soldierList = new List<Soldier>();
    
}
