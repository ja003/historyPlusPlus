using SQLite4Unity3d;
using System;
using UnityEngine;

public class QuestionTable :ICloneable{
    [PrimaryKey]
    public string code { get; set; }
    public bool completed { get; set; }
    public bool solved { get; set; }
    public string category { get; set; }
    public string currentPeriod { get; set; }
    public string startPeriod { get; set; }
    public string endPeriod { get; set; }
    public string questionPack { get; set; }

    public string alias_CZ { get; set; }
    public string alias_ENG { get; set; }
    public string text_CZ { get; set; }
    public string text_ENG { get; set; }

    public int century { get; set; }
    public int decade { get; set; }
    public int year { get; set; }
    public int month { get; set; }
    public int day { get; set; }

    public int endCentury { get; set; }
    public int endDecade { get; set; }
    public int endYear { get; set; }
    public int endMonth { get; set; }
    public int endDay { get; set; }

    public override string ToString()
    {
        return "["+ code + "]: "+completed + "," + category + "," + currentPeriod + "," + 
            alias_CZ + "," + 
            century + "," + decade;
    }

    public Question GetQuestion(Language language)
    {
        Question q = new Question();
        q.code = code;
        q.completed = completed;
        q.solved = solved;
        q.category = (Category)Enum.Parse(typeof(Category), category);
        q.currentPeriod = (Period)Enum.Parse(typeof(Period), currentPeriod);
        q.startPeriod = (Period)Enum.Parse(typeof(Period), startPeriod);
        q.endPeriod = (Period)Enum.Parse(typeof(Period), endPeriod);
        q.questionPack= (QuestionPack)Enum.Parse(typeof(QuestionPack), questionPack);

        switch (language)
        {
            case Language.CZ:
                q.alias = alias_CZ;
                q.text= text_CZ;
                break;
            case Language.ENG:
                q.alias = alias_ENG;
                q.text = text_ENG;
                break;
            default:
                UnityEngine.Debug.Log("FAIL");
                q.alias = "FAIL";
                q.text = "FAIL";
                break;
        }

        q.century = century;
        q.decade = decade;
        q.year = year;
        q.month  = month;
        q.day = day;

        q.endCentury = endCentury;
        q.endDecade = endDecade;
        q.endYear = endYear;
        q.endMonth = endMonth;
        q.endDay = endDay;

        return q;
    }


    public object Clone()
    {
        QuestionTable q = new QuestionTable();
        q.code = code;
        q.completed = completed;
        q.category = category;
        q.currentPeriod = currentPeriod;
        q.startPeriod = startPeriod;
        q.endPeriod = endPeriod;

        q.alias_CZ = alias_CZ;
        q.alias_ENG = alias_ENG;
        q.text_CZ = text_CZ;
        q.text_ENG = text_ENG;

        q.century = century;
        q.decade = decade;
        q.year = year;
        q.month = month;
        q.day = day;

        q.endCentury = endCentury;
        q.endDecade = endDecade;
        q.endYear = endYear;
        q.endMonth = endMonth;
        q.endDay = endDay;

        return q;
    }
}
