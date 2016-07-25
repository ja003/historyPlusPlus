using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsActivity : MonoBehaviour {

    Text language_text;
    Text category_text;
    Text questionPack_text;
    Text difficulty_text;

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
    Dropdown difficulty_dropdown;

    SettingsTable settings;

    void Start () {


        InitializeGameInfo();

        InitializeSettingsVariables();

        StartCoroutine(LateStart(0.0f));

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
        
        //LoadTexts();
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
        difficulty_text = GameObject.Find("difficulty_text").
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
        difficulty_dropdown = GameObject.Find("difficulty_dropdown").
            GetComponent<Dropdown>();



        science_toggle.onValueChanged.AddListener(delegate {
            SaveSettings();
        });
        war_toggle.onValueChanged.AddListener(delegate {
            SaveSettings();
        });
        politics_toggle.onValueChanged.AddListener(delegate {
            SaveSettings();
        });
        others_toggle.onValueChanged.AddListener(delegate {
            SaveSettings();
        });


        questionPack_dropdown.onValueChanged.AddListener(delegate {
            SaveSettings();
        });
        language_dropdown.onValueChanged.AddListener(delegate {
            SaveSettings();
        });
    }

    
    public void SaveSettings()
    {
        
        settings.index = 1;
        UpdateSettingsLanguage();
        settings.category_science = science_toggle.isOn;
        settings.category_war = war_toggle.isOn;
        settings.category_politics= politics_toggle.isOn;
        settings.category_others= others_toggle.isOn;
        settings.questionPack = questionPack_dropdown.options[questionPack_dropdown.value].text;
        settings.difficulty = difficulty_dropdown.options[difficulty_dropdown.value].text;

        Debug.Log(settings);
        DBAccess.Instance.SaveSettings(settings);

        UpdateSettings();
    }


    #endregion

    public void UpdateSettingsLanguage()
    {
        settings.language = language_dropdown.options[language_dropdown.value].text;
        Debug.Log("language = " + settings.language);
        LanguageSupport.Instance.SetLanguage(settings);
        LanguageSupport.Instance.DropTexts();
        LanguageSupport.Instance.InitializeSettingsTexts();
        LoadTexts();

    }

    void Menu()
    {
        SceneManager.LoadScene("menu");
    }

    #region Visualization

    private void UpdateSettings()
    {

        LoadTexts();
        UpdateDropdown(language_dropdown, settings.language);
        UpdateDropdown(questionPack_dropdown, settings.questionPack);
        UpdateDropdown(difficulty_dropdown, settings.difficulty);

        science_toggle.isOn = settings.category_science;
        war_toggle.isOn = settings.category_war;
        politics_toggle.isOn = settings.category_politics;
        others_toggle.isOn = settings.category_others;
        
        //questionPack_dropdown.GetComponentInChildren<Text>().text =
          //  settings.questionPack;
    }

    private void UpdateDropdown(Dropdown dropdown, string selected)
    {
        for (int i = 0; i < dropdown.options.Count; i++)
        {
            if (selected == dropdown.options[i].text)
            {
                dropdown.value = i;
                dropdown.GetComponentInChildren<Text>().text =
                    dropdown.options[dropdown.value].text;
                return;
            }
        }
    }
    

    private void LoadTexts()
    {
        language_text.text = LanguageSupport.Instance.GetText("settings_language");
        category_text.text = LanguageSupport.Instance.GetText("settings_category");
        questionPack_text.text = LanguageSupport.Instance.GetText("settings_questionPack");
        difficulty_text.text = LanguageSupport.Instance.GetText("settings_difficulty");

        science_text.text = LanguageSupport.Instance.GetText("category_science");
        war_text.text = LanguageSupport.Instance.GetText("category_war");
        politics_text.text = LanguageSupport.Instance.GetText("category_politics");
        others_text.text = LanguageSupport.Instance.GetText("category_others");

        questionPack_dropdown.options.Clear();
        questionPack_dropdown.options.Add(new Dropdown.OptionData(
            LanguageSupport.Instance.GetText("settings_qp_czechia")));
        questionPack_dropdown.options.Add(new Dropdown.OptionData(
            LanguageSupport.Instance.GetText("settings_qp_england")));

        difficulty_dropdown.options.Clear();
        difficulty_dropdown.options.Add(new Dropdown.OptionData(
            LanguageSupport.Instance.GetText("settings_diff_easy")));
        difficulty_dropdown.options.Add(new Dropdown.OptionData(
            LanguageSupport.Instance.GetText("settings_difficulty")));



    }

    #endregion
    

}
