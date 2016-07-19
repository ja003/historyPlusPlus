using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuActivity : MonoBehaviour {
    
    Button startGame_button;
    Button settings_button;

    Text startGame_text;
    Text settings_text;



    void Start () {
        //Debug.Log("menu start");

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        InitializeGameInfo();

        InitializeOptions();

        InitializeMenuVariables();
        

        StartCoroutine(LateStart(0.01f));
    }

    #region Initialization

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //Your Function You Want to Call
        LanguageSupport.Instance.InitializeMenuTexts();
        LoadTexts();
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

    private void InitializeOptions()
    {
        startGame_button = GameObject.Find("startGame_button").
            GetComponent<Button>();
        startGame_button.onClick.AddListener(StartGame);

        settings_button = GameObject.Find("settings_button").
            GetComponent<Button>();
        settings_button.onClick.AddListener(Settings);
    }

    private void InitializeMenuVariables()
    {
        startGame_text = GameObject.Find("startGame_text").
            GetComponent<Text>();
        settings_text = GameObject.Find("settings_text").
            GetComponent<Text>();

    }

    #endregion

    #region Functions
    public void StartGame()
    {
        SceneManager.LoadScene("game");
    }

    public void Settings()
    {
        SceneManager.LoadScene("settings");
    }
    #endregion

    #region Visualization
    private void LoadTexts()
    {
        startGame_text.text = LanguageSupport.Instance.GetText("menu_start");
        settings_text.text = LanguageSupport.Instance.GetText("menu_settings");
    }
    #endregion
}
