using SQLite4Unity3d;
using System.Collections.Generic;

public class SettingsTable {

    [PrimaryKey, AutoIncrement]
    public int index { get; set; }

    public string language { get; set; }
    public string questionPack { get; set; }
    public string difficulty { get; set; }

    public bool category_science { get; set; }
    public bool category_war { get; set; }
    public bool category_politics { get; set; }
    public bool category_others { get; set; }


    /// <summary>
    /// checks if parameters are set correctly.
    /// if not then parameters are set to default values
    /// </summary>
    public void CheckValidity()
    {
        if (language.Length == 0)
            language = "CZ";
        if (questionPack.Length == 0)
            questionPack = "česko";
        if (difficulty.Length == 0)
            difficulty = "easy";
    }

    public override string ToString()
    {
        string s = "[settings] \n";
        s += "index: " + index + "\n";
        s += "language: " + language+"\n";
        s += "questionPack: " + questionPack + "\n";
        s += "difficulty: " + difficulty + "\n";
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
