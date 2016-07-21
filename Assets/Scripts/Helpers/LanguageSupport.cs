using UnityEngine;
using System.Collections;

using System.Xml;
using UnityEngine.UI;
using System;

public class LanguageSupport : MonoBehaviour
{
    public Language language;
    public Hashtable texts;
    

    public static LanguageSupport Instance { get; private set; }

    void Awake()
    {
        language = Language.CZ;
        if (Instance != null && Instance != this)
        {
            // If that is the case, we destroy other instances
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        texts = new Hashtable();
    }


    #region Initialization
    public void InitializeMenuTexts()
    {
        AddText("menu_start");
        AddText("menu_settings");
    }

    public void InitializeGameTexts()
    {
        AddText("game_century");
        AddText("game_decade");
        AddText("game_year");
        AddText("game_month");
        AddText("game_day");

    }

    public void DropTexts()
    {
        texts.Clear();
    }

    public void InitializeAnswerTexts()
    {
        AddText("answer_next");
        AddText("answer_reveal");
        AddText("answer_more");
        AddText("answer_correct");
        AddText("answer_wrong");
        AddText("answer_anyAvailable"); ;
        AddText("answer_noAvailable");

    }

    public void InitializeSettingsTexts()
    {
        AddText("settings_language");
        AddText("settings_category");
        AddText("settings_questionPack");
        
        AddText("settings_qp_czechia");
        AddText("settings_qp_england");

        InitializeCategoryTexts();
    }

    void InitializeCategoryTexts()
    {
        AddText("category_science");
        AddText("category_politics");
        AddText("category_others");
        AddText("category_war");
    }

    #endregion

    #region Functions

    /// <summary>
    /// loads text to texts table if it is not already there
    /// - key = name
    /// </summary>
    private void AddText(string key)
    {
        if (!texts.Contains(key))
            texts.Add(key, LoadText(key));
    }

    public void SetLanguage(SettingsTable settings)
    {
        language = (Language)Enum.Parse(typeof(Language), settings.language);
    }

    string LoadText(string code)
    {
        string s = DBAccess.Instance.GetText(code, language);
        return s;
    }

    public string GetText(string code)
    {
        string t = (string)texts[code];
        //Debug.Log(code + ": " + t);
        if (t != null)
            return (string)texts[code];
        else
        {
            Debug.Log(code + ": *text not found*");
            foreach (string value in texts.Values)
            {
                Debug.Log(value);
            }
            return "*text not found*";
        }
    }
    #endregion


}
