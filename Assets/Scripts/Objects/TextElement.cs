using SQLite4Unity3d;

public class TextElement {

    [PrimaryKey, AutoIncrement]
    public string code { get; set; }
    public string text_CZ{ get; set; }
    public string text_ENG{ get; set; }
}
