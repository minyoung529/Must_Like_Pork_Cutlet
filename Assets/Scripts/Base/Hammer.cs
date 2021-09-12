
[System.Serializable]
public class Hammer
{
    public string name;
    public string grade;
    public int clickCount;
    public int xp;
    public int level;
    public bool isSold;

    public bool GetIsSold()
    {
        return isSold;
    }
}
