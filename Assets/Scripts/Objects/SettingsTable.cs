using SQLite4Unity3d;

public class SettingsTable {

    [PrimaryKey, AutoIncrement]
    public int index { get; set; }
    public string language { get; set; }
    public string questionPack { get; set; }
    public bool category_science { get; set; }
    public bool category_war { get; set; }
    public bool category_politics { get; set; }
    public bool category_others { get; set; }
}
