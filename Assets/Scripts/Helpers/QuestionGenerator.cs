using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionGenerator : MonoBehaviour {

    public static QuestionGenerator Instance { get; private set; }
    public AnswerActivity answerActivity;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Question GetRandomQuestion()
    {
        List<Category> c = GameInfo.Instance.settings.GetCategories();

        Question q = new Question();

        while (!q.IsValid() && c.Count > 0)
        {
            int i = Random.Range(0, c.Count);
            q = DBAccess.Instance.GetRandomQuestion(c[i],
                LanguageSupport.Instance.language,
                (QuestionPack)System.Enum.Parse(typeof(QuestionPack), GameInfo.Instance.settings.questionPack));
            c.RemoveAt(i);
        }

        if (q.IsValid())
        {
            //Debug.Log(q);
            return q;
        }
        else
            Debug.Log("NO MORE QUESTIONS from any selected category");

        return q;
        
    }

    public int GetFirstOption(Question question)
    {
        Difficulty difficulty = GameInfo.Instance.settings.GetDifficulty();

        int i = difficulty == Difficulty.easy ? 
            Random.Range(0, 3) : Random.Range(0,9);
        int output = 666;

        switch (question.currentPeriod)
        {
            case Period.century:
                output = question.century -i;
                switch (difficulty)
                {
                    case Difficulty.easy:
                        if (output > 17)
                            return 17;
                        break;
                    case Difficulty.hard:
                        if (output > 12)
                            return 12;
                        break;
                }
                break;
            case Period.decade:
                output = question.decade -i;
                if (output < 0)
                    return 0;
                break;
            case Period.year:
                output = question.year -i;
                if (output < 0)
                    return 0;
                switch (difficulty)
                {
                    case Difficulty.easy:
                        if (output > 6)
                            return 6;
                        break;
                    case Difficulty.hard:
                        if (output > 1)
                            return 1;
                        break;
                }
                break;
            case Period.month:
                output = question.month -i;
                if (output < 0)
                    return 0;
                switch (difficulty)
                {
                    case Difficulty.easy:
                        if (output > 9)
                            return 9;
                        break;
                    case Difficulty.hard:
                        if (output > 4)
                            return 4;
                        break;
                }
                break;
            case Period.day:
                output = question.day - i;
                if (output < 0)
                    return 0;
                switch (difficulty)
                {
                    case Difficulty.easy:
                        if (output > 28)
                            return 28;
                        break;
                    case Difficulty.hard:
                        if (output > 23)
                            return 23;
                        break;
                }

                break;
        }
        
        return output;
    }

    /// <summary>
    /// checks if answer is between currentPeriod and endPeriod values [inclusive]
    /// </summary>
    private bool CheckTimespan(Question question, System.DateTime answer)
    {
        System.DateTime from = question.GetFrom();
        System.DateTime to = question.GetTo();

        switch (question.currentPeriod)
        {
            case Period.century:
                from = new System.DateTime(100*(from.Year / 100), from.Month, from.Day);
                to = new System.DateTime(100*(to.Year / 100), to.Month, to.Day);
                break;
            case Period.decade:
                from = new System.DateTime(10 * (from.Year / 10), from.Month, from.Day);
                to = new System.DateTime(10 * (to.Year / 10), to.Month, to.Day);
                break;
                /*case Period.year:
                    from = question.year;
                    to = question.endYear;
                    break;
                case Period.month:
                    from = question.month;
                    to = question.endMonth;
                    break;
                case Period.day:
                    from = question.day;
                    to = question.endDay;
                    break;*/
        }
        Debug.Log(from);
        Debug.Log(to);


        return (answer >= from && answer <= to) || (answer <= from && answer >= to);
    }

    /// <summary>
    /// check if answer to question is correct
    /// - updates DB: completed, solved and current period
    /// </summary>
    public bool CheckAnswer(Question question, int answer)
    {
        answer = Mathf.Abs(answer);

        Debug.Log(answerActivity.currentAnswer);
        int currentYear = 0;
        if (answerActivity.currentAnswer.Year != 1)
            currentYear = answerActivity.currentAnswer.Year;

        switch (question.currentPeriod)
        {
            case Period.century:
                answerActivity.currentAnswer = new System.DateTime(100 * answer, 1, 1);        
                break;
            case Period.decade:
                answerActivity.currentAnswer = new System.DateTime(
                    currentYear + 10*answer, 1, 1);
                break;
            case Period.year:
                answerActivity.currentAnswer = new System.DateTime(
                    currentYear + answer, 1, 1);
                break;
            case Period.month:
                answerActivity.currentAnswer = new System.DateTime(
                    currentYear, answer, 1);
                break;
            case Period.day:
                answerActivity.currentAnswer = new System.DateTime(
                    currentYear, answerActivity.currentAnswer.Month, answer);
                break;
        }

        bool correct = CheckTimespan(question, answerActivity.currentAnswer);

        if (correct)
        {
            Debug.Log(answer + " is correct: " + answerActivity.currentAnswer);
            question.CorrectAnswer();

            if (question.completed)
                DBAccess.Instance.CompleteQuestion(question, correct);
            else
                DBAccess.Instance.UpdateQuestionPeriod(question);
        }
        else
        {
            Debug.Log(answer + " is NOT correct (" + question + " )");
            Debug.Log(answerActivity.currentAnswer);
        }



        return correct;
    }
}
