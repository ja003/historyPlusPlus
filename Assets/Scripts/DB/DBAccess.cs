﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class DBAccess : MonoBehaviour {

    DataService ds = new DataService("history_db.db");
    public static DBAccess Instance { get; private set; }

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
    
    void Start () {
		//ds = new DataService ("history_db.db");
        //Debug.Log(ds);
	}

    #region Question

    public Question GetRandomQuestion(Category category, Language language, QuestionPack questionPack)
    {
        return ds.GetRandomQuestion(category, language, questionPack);
    }

    public List<Question> GetCompletedQuestions()
    {
        List<QuestionTable> questionsTable = ds.GetCompletedQuestions();
        List<Question> questions = new List<Question>();
        foreach(QuestionTable q in questionsTable)
        {
            questions.Add(q.GetQuestion(LanguageSupport.Instance.language));
        }
        return questions;
    }

    public void ResetQuestions()
    {
        ds.ResetQuestions();
    }

    public void CompleteQuestion(Question question, bool solved)
    {
        ds.CompleteQuetion(question, solved);
    }

    public void UpdateQuestionPeriod(Question question)
    {
        ds.UpdateQuestionPeriod(question);
    }

    public bool IsAnyQuestionAvailable()
    {
        Array ca = Enum.GetValues(typeof(Category));
        Array qpa = Enum.GetValues(typeof(QuestionPack));

        foreach (Category c in ca)
        {
            foreach (QuestionPack qp in qpa)
            {
                if (GetRandomQuestion(c, Language.CZ, qp).IsValid())
                    return true;
            }                
        }
        return false;
    }

    #endregion


    public string GetText(string code, Language language)
    {
        //Debug.Log(ds);
        TextElement te = ds.GetTextElement(code);
        
        switch (language)
        {
            case Language.CZ:
                return te.text_CZ;
            case Language.ENG:
                return te.text_ENG;
            default:
                return te.text_CZ;
        }
    }

    #region Settings
    public SettingsTable GetSettings()
    {
        SettingsTable settings = ds.GetSettings();
        settings.CheckValidity();
        return settings;
    }

    public void SaveSettings(SettingsTable settings)
    {
        ds.SaveSettings(settings);
    }
    #endregion

    #region Score
    public ScoreTable GetScore()
    {
        return ds.GetScore();
    }

    public void SaveScore(ScoreTable score)
    {
        ds.SaveScore(score);
    }
    #endregion
}
