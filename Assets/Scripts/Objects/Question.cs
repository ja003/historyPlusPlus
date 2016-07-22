using UnityEngine;
using System.Collections;

public class Question {
    public int id;
    public string alias = "";
    public string text = "";
    public bool completed = false;
    public bool solved = false;
    public Category category;
    public Period currentPeriod;
    public Period startPeriod;

    public int century = 666;
    public int decade = 666;
    public int year = 666;
    public int month = 666;
    public int day = 666;
    
    public string GetSolvedString()
    {
        string centuryString = century != 666 ? century + "" : "";
        string decadeString = decade != 666 ? decade + "" : "";
        string yearString = year != 666 ? year + "" : "";
        string monthString = month != 666 ? month + "" : "";
        string dayString = day != 666 ? day + "" : "";

        switch (currentPeriod)
        {
            case Period.century:
                return "";                

            case Period.decade:
                return centuryString + "_ _";
            case Period.year:
                return centuryString + decadeString + "_";
            case Period.month:
                return centuryString + decadeString + yearString;

            case Period.day:
                return centuryString + decadeString + yearString + " " +
                    monthString + " _ _";
            case Period.none:
                return centuryString + decadeString + yearString + " " +
                    monthString + " " + dayString;
        }
        return "";
    }

    public string GetPeriodAppendix()
    {
        switch (currentPeriod)
        {
            case Period.century:
                return "xx";
            case Period.decade:
                return "x";
            case Period.month:
                return " xx";
            default:
                return "";
        }
    }

    public string GetPeriodPrefix()
    {
        switch (currentPeriod)
        {
            case Period.decade:
                return century+"";
            case Period.year:
                return century+""+decade;
            case Period.month:
                return century + "" + decade + "" + year + " ";
            case Period.day:
                return century + "" + decade + "" + year + " " + month + " ";
            default:
                return "";
        }
    }

    private string GetCorrectAnswer()
    {
        Period tmpPeriod = currentPeriod;
        currentPeriod = Period.none;
        string output = GetSolvedString();
        currentPeriod = tmpPeriod;
        return output;
    }

    /// <summary>
    /// correct answer process
    /// - moves current period
    /// - if next period is not defined, question is completed
    /// </summary>
    public void CorrectAnswer()
    {
        switch (currentPeriod)
        {
            case Period.century:
                if (decade != 666)
                    currentPeriod = Period.decade;
                else
                    CompleteQuestion(true);
                break;

            case Period.decade:
                if (year != 666)
                    currentPeriod = Period.year;
                else
                    CompleteQuestion(true);
                break;

            case Period.year:
                if (month != 666)
                    currentPeriod = Period.month;
                else
                    CompleteQuestion(true);
                break;
            case Period.month:
                if (day != 666)
                    currentPeriod = Period.day;
                else
                    CompleteQuestion(true);
                break;
            case Period.day:
                CompleteQuestion(true);
                break;
        }
    }

    private void CompleteQuestion(bool solved)
    {
        completed = true;
        currentPeriod = Period.none;
        if (solved)
            this.solved = solved;
        
    }

    private void ResetQuestion()
    {
        currentPeriod = startPeriod;
        solved = false;
        completed = false;
    }

    public bool IsValid()
    {
        return alias.Length > 0 || text.Length > 0;
    }

    public override string ToString()
    {
        return completed + "," + category + "," + currentPeriod + "," +
            alias + "," + text + "," + 
            century + "," + decade;
    }

}
