using SQLite4Unity3d;

public class ScoreTable
{

    [PrimaryKey, AutoIncrement]
    public int index { get; set; }
    public int score{ get; set; }

    public override string ToString()
    {
        string s = "[score] \n";
        s += "index: " + index + ",";
        s += "score: " + score;
        return s;
    }
}
