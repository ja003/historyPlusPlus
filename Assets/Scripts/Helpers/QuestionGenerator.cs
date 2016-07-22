using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuestionGenerator : MonoBehaviour {

    public static QuestionGenerator Instance { get; private set; }

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
            LanguageSupport.Instance.language);
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
        int i = Random.Range(0, 1);
        int output = 666;
        switch (GameInfo.Instance.settings.difficulty)
        {
            case "easy":
                switch (question.currentPeriod)
                {
                    case Period.century:
                        output = question.century -i;
                        break;
                    case Period.decade:
                        output = question.decade -i;
                        break;
                    case Period.year:
                        output = question.year -i;
                        break;
                    case Period.month:
                        output = question.month -i;
                        break;
                    case Period.day:
                        output = question.day - i;
                        break;
                }
                break;
            case "hard":
                output = i;
                break;
        }
        if (output < 0)
            return 0;
        else
            return output;
    }

    /// <summary>
    /// check if answer to question is correct
    /// - updates DB: completed, solved and current period
    /// </summary>
    public bool CheckAnswer(Question question, int answer)
    {
        bool correct = true;
        switch (question.currentPeriod)
        {
            case Period.century:
                correct =  question.century == answer;
                break;
            case Period.decade:
                correct = question.decade == answer;
                break;
            case Period.year:
                correct = question.year == answer;
                break;
            case Period.month:
                correct = question.month == answer;
                break;
            case Period.day:
                correct = question.day == answer;
                break;
        }

        if (correct)
        {
            Debug.Log(answer + " is correct");
            question.CorrectAnswer();

            if (question.completed)
                DBAccess.Instance.CompleteQuestion(question, correct);
            else
                DBAccess.Instance.UpdateQuestionPeriod(question);
        }
        else
        {
            Debug.Log(answer + " is NOT correct (" + question + " )");
        }



        return correct;
    }
}
