using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

    public Question GetRandomQuestion(Category category, Language language)
    {
        return ds.GetRandomQuestion(category, language);
    }

    public void CompleteQuestion(Question question, bool solved)
    {
        ds.CompleteQuetion(question, solved);
    }

    public void UpdateQuestionPeriod(Question question)
    {
        ds.UpdateQuestionPeriod(question);
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
        return ds.GetSettings();
    }

    public void UpdateSettings(SettingsTable settings)
    {
        ds.UpdateSettings(settings);
    }
    #endregion
}
