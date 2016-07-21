using SQLite4Unity3d;
using System.Collections.Generic;

public class SettingsTable {

    [PrimaryKey, AutoIncrement]
    public int index { get; set; }
    public string language { get; set; }
    public string questionPack { get; set; }
    public bool category_science { get; set; }
    public bool category_war { get; set; }
    public bool category_politics { get; set; }
    public bool category_others { get; set; }

    public override string ToString()
    {
        string s = "[settings] \n";
        s += "index: " + index + "\n";
        s += "language: " + language+"\n";
        s += "questionPack: " + questionPack + "\n";
        s += "category_science: " + category_science + "\n";
        s += "category_war: " + category_war + "\n";
        s += "category_politics: " + category_politics + "\n";
        s += "category_others: " + category_others + "\n";

        return s;
    }

    public List<Category> GetCategories()
    {
        List<Category> categories = new List<Category>();
        if (category_science)
            categories.Add(Category.Science);
        if (category_war)
            categories.Add(Category.War);
        if (category_politics)
            categories.Add(Category.Politics);
        if (category_others)
            categories.Add(Category.Others);

        return categories;
    } 
}
