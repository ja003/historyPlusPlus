using UnityEngine;
using System.Collections;

public class Question {
    public string code;
    public string alias = "";
    public string text = "";
    public bool completed = false;
    public bool solved = false;
    public Category category;
    public Period currentPeriod;
    public Period startPeriod;
    public Period endPeriod;
    public QuestionPack questionPack;

    public int century = 666;
    public int decade = 666;
    public int year = 666;
    public int month = 666;
    public int day = 666;

    public int endCentury = 666;
    public int endDecade = 666;
    public int endYear = 666;
    public int endMonth = 666;
    public int endDay = 666;

    public string GetSolvedString()
    {
        string centuryString = (century != 666 && century != 0) ? century + "" : "";
        string decadeString = decade != 666 ? decade + "" : "";
        string yearString = year != 666 ? year + "" : "";
        string monthString = month != 666 ? month + "" : "";
        string dayString = day != 666 ? day + "" : "";

        string endCenturyString = (endCentury != 666 && endCentury != 0) ? endCentury + "" : "";
        string endDecadeString = endDecade != 666 ? endDecade + "" : "";
        string endYearString = endYear != 666 ? endYear + "" : "";
        string endMonthString = endMonth != 666 ? endMonth + "" : "";
        string endDayString = endDay != 666 ? endDay + "" : "";

        string output = "";

        switch (currentPeriod)
        {
            case Period.century:
                output +=  "";
                break;
            case Period.decade:
                output +=  centuryString + "_ _";
                break;
            case Period.year:
                output +=  centuryString + decadeString + "_";
                break;
            case Period.month:
                output +=  centuryString + decadeString + yearString;
                break;
            case Period.day:
                output +=  centuryString + decadeString + yearString + " " +
                    monthString + " _ _";
                break;
            case Period.none:
                output += centuryString + decadeString + yearString;
                if (monthString.Length > 0)
                    output += " " + monthString;
                if (dayString.Length > 0)
                    output += " " + dayString;
                break;
        }
        if (endCenturyString.Length > 0 || endDecadeString.Length > 0 || endYearString.Length > 0 ||
            endMonthString.Length > 0 || endDayString.Length > 0)
            output += " - ";

        output += endCenturyString + endDecadeString + endYearString;
        if (endMonthString.Length > 0)
            output += " " + endMonthString;
        if (endDayString.Length > 0)
            output += " " + endDayString;

        return output;
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

    public string GetCorrectAnswer()
    {
        Period tmpPeriod = currentPeriod;
        currentPeriod = Period.none;
        string output = GetSolvedString();
        currentPeriod = tmpPeriod;
        Debug.Log(output);
        return output;
    }

    /// <summary>
    /// correct answer process
    /// - moves current period
    /// - if next period is endPeriod, question is completed
    /// </summary>
    public void CorrectAnswer()
    {
        switch (currentPeriod)
        {
            case Period.century:
                if (endPeriod != Period.decade)
                    currentPeriod = Period.decade;
                else
                    CompleteQuestion(true);
                break;

            case Period.decade:
                if (endPeriod != Period.year)
                    currentPeriod = Period.year;
                else
                    CompleteQuestion(true);
                break;

            case Period.year:
                if (endPeriod != Period.month)
                    currentPeriod = Period.month;
                else
                    CompleteQuestion(true);
                break;
            case Period.month:
                if (endPeriod != Period.day)
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
