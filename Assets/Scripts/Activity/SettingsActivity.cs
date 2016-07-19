using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsActivity : MonoBehaviour {

    Text language_text;
    Text category_text;
    Text questionPack_text;

    Text science_text;
    Text war_text;
    Text politics_text;
    Text others_text;

    Toggle science_toggle;
    Toggle war_toggle;
    Toggle politics_toggle;
    Toggle others_toggle;

    Dropdown questionPack_dropdown;
    Dropdown language_dropdown;

    SettingsTable settings;

    void Start () {


        InitializeGameInfo();

        InitializeSettingsVariables();

        StartCoroutine(LateStart(0.01f));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse3) || Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
    }


    #region Initialization
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        settings = GameObject.Find("GameInfo").GetComponent<GameInfo>().settings;

        //Your Function You Want to Call
        LanguageSupport.Instance.InitializeSettingsTexts();
        
        LoadTexts();
        UpdateSettings();
    }

    private void InitializeGameInfo()
    {
        if (GameObject.Find("GameInfo") == null)
        {
            Debug.Log("no GameInfo found");
            GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/GameInfo"));
            go.name = "GameInfo";
            Debug.Log("GameInfo loaded");
        }
    }

    private void InitializeSettingsVariables()
    {
        language_text = GameObject.Find("language_text").
            GetComponent<Text>();
        category_text = GameObject.Find("category_text").
            GetComponent<Text>();
        questionPack_text = GameObject.Find("questionPack_text").
            GetComponent<Text>();

        science_text = GameObject.Find("science_text").
            GetComponent<Text>();
        war_text = GameObject.Find("war_text").
            GetComponent<Text>();
        politics_text = GameObject.Find("politics_text").
            GetComponent<Text>();
        others_text = GameObject.Find("others_text").
            GetComponent<Text>();

        science_toggle = GameObject.Find("science_toggle").
            GetComponent<Toggle>();
        war_toggle = GameObject.Find("war_toggle").
            GetComponent<Toggle>();
        politics_toggle = GameObject.Find("politics_toggle").
            GetComponent<Toggle>();
        others_toggle = GameObject.Find("others_toggle").
            GetComponent<Toggle>();

        questionPack_dropdown = GameObject.Find("questionPack_dropdown").
            GetComponent<Dropdown>();
        language_dropdown = GameObject.Find("language_dropdown").
            GetComponent<Dropdown>();
        language_dropdown.onValueChanged.AddListener(delegate {
            UpdateLanguage();
        });
    }

    public void UpdateLanguage()
    {
        SaveSettings();
        Debug.Log("language = " + settings.language);
        LanguageSupport.Instance.SetLanguage(settings);
        LanguageSupport.Instance.DropTexts();
        LanguageSupport.Instance.InitializeSettingsTexts();
        LoadTexts();        
    }

    public void SaveSettings()
    {
        UpdateSettings();
        settings.index = 1;
        settings.language = language_dropdown.options[language_dropdown.value].text;
        settings.category_science = science_toggle.isOn;
        settings.category_war = war_toggle.isOn;
        settings.category_politics= politics_toggle.isOn;
        settings.category_others= others_toggle.isOn;
        settings.questionPack = questionPack_dropdown.options[questionPack_dropdown.value].text;

        DBAccess.Instance.UpdateSettings(settings);
    }


    #endregion

    void Menu()
    {
        SceneManager.LoadScene("menu");
    }

    #region Visualization

    private void UpdateSettings()
    {
        language_dropdown.GetComponentInChildren<Text>().text =
            settings.language;

        science_toggle.isOn = settings.category_science;
        war_toggle.isOn = settings.category_war;
        politics_toggle.isOn = settings.category_politics;
        others_toggle.isOn = settings.category_others;
        
        questionPack_dropdown.GetComponentInChildren<Text>().text =
            settings.questionPack;
    }

    private void LoadTexts()
    {
        language_text.text = LanguageSupport.Instance.GetText("settings_language");
        category_text.text = LanguageSupport.Instance.GetText("settings_category");
        questionPack_text.text = LanguageSupport.Instance.GetText("settings_questionPack");

        science_text.text = LanguageSupport.Instance.GetText("category_science");
        war_text.text = LanguageSupport.Instance.GetText("category_war");
        politics_text.text = LanguageSupport.Instance.GetText("category_politics");
        others_text.text = LanguageSupport.Instance.GetText("category_others");

        questionPack_dropdown.options.Clear();
        questionPack_dropdown.GetComponentInChildren<Text>().text = "CZ";
        questionPack_dropdown.options.Add(new Dropdown.OptionData(
            LanguageSupport.Instance.GetText("settings_qp_czechia")));
        questionPack_dropdown.options.Add(new Dropdown.OptionData(
            LanguageSupport.Instance.GetText("settings_qp_england")));

        

    }

    #endregion
}
