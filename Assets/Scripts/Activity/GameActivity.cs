using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameActivity : MonoBehaviour {
    
    Options options;

    Text questionText;
    Text period_text;
    Text solved_text;
    Text score_text;

    public Question currentQuestion;
    AnswerActivity answerActivity;


    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        
        InitializeOptions();

        InitializeGameVariables();

        InitializeGameInfo();

        SetOptionsWidth();

        StartCoroutine(LateStart(0.1f));
        /*if (SceneManager.GetActiveScene().buildIndex == 1)
            OnLevelWasLoaded(1);*/
    }

    /*void OnLevelWasLoaded(int level)
    {
        if(level == 1)
            

    }/*/

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
        //Your Function You Want to Call
        LanguageSupport.Instance.InitializeGameTexts();
        LanguageSupport.Instance.InitializeAnswerTexts();

        LoadNextQuestion();
        RefreshSolvedText();
        RefreshScore();
    }

    private void InitializeGameVariables()
    {
        answerActivity = GameObject.Find("AnswerActivity").
            GetComponent<AnswerActivity>();

        questionText = GameObject.Find("question_text").
            GetComponent<Text>();
        period_text = GameObject.Find("period_text").
            GetComponent<Text>();
        solved_text = GameObject.Find("solved_text").
            GetComponent<Text>();
        score_text = GameObject.Find("score_text").
            GetComponent<Text>();
    }

    private void InitializeOptions()
    {
        options = new Options();

        options.opt_1_obj = GameObject.Find("opt_1");
        options.opt_1 = GameObject.Find("opt_1").
            GetComponent<Button>();
        options.opt_1.onClick.AddListener(() => SelectOption(1));
        options.opt_1_text = GameObject.Find("opt_1").
            GetComponentInChildren<Text>();

        options.opt_2_obj = GameObject.Find("opt_2");
        options.opt_2 = GameObject.Find("opt_2").
            GetComponent<Button>();
        options.opt_2.onClick.AddListener(() => SelectOption(2));
        options.opt_2_text = GameObject.Find("opt_2").
            GetComponentInChildren<Text>();

        options.opt_3_obj = GameObject.Find("opt_3");
        options.opt_3 = GameObject.Find("opt_3").
            GetComponent<Button>();
        options.opt_3.onClick.AddListener(() => SelectOption(3));
        options.opt_3_text = GameObject.Find("opt_3").
            GetComponentInChildren<Text>();

        options.opt_4 = GameObject.Find("opt_4").
            GetComponent<Button>();
        options.opt_4.onClick.AddListener(() => SelectOption(4));
        options.opt_4_text = GameObject.Find("opt_4").
            GetComponentInChildren<Text>();

        options.opt_5 = GameObject.Find("opt_5").
            GetComponent<Button>();
        options.opt_5.onClick.AddListener(() => SelectOption(5));
        options.opt_5_text = GameObject.Find("opt_5").
            GetComponentInChildren<Text>();

        options.opt_6 = GameObject.Find("opt_6").
            GetComponent<Button>();
        options.opt_6.onClick.AddListener(() => SelectOption(6));
        options.opt_6_text = GameObject.Find("opt_6").
            GetComponentInChildren<Text>();

        options.opt_7 = GameObject.Find("opt_7").
            GetComponent<Button>();
        options.opt_7.onClick.AddListener(() => SelectOption(7));
        options.opt_7_text = GameObject.Find("opt_7").
            GetComponentInChildren<Text>();

        options.opt_8 = GameObject.Find("opt_8").
            GetComponent<Button>();
        options.opt_8.onClick.AddListener(() => SelectOption(8));
        options.opt_8_text = GameObject.Find("opt_8").
            GetComponentInChildren<Text>();

        options.opt_9 = GameObject.Find("opt_9").
            GetComponent<Button>();
        options.opt_9.onClick.AddListener(() => SelectOption(9));
        options.opt_9_text = GameObject.Find("opt_9").
            GetComponentInChildren<Text>();

    }

    private void InitializeGameInfo()
    {
        if(GameObject.Find("GameInfo") == null)
        {
            //Debug.Log("no GameInfo found");
            GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/GameInfo"));
            go.name = "GameInfo";
            //Debug.Log("GameInfo loaded");
        }
    }
    #endregion

    #region Functions
    public void LoadNextQuestion()
    {
        currentQuestion = QuestionGenerator.Instance.GetRandomQuestion();
        if (currentQuestion.IsValid())
            RefreshQuestion();
        else
            answerActivity.NoMoreQuestions();
        
    }
      
    public void SelectOption(int optionNumber)
    {

        answerActivity.ShowAnswerStatus(
            QuestionGenerator.Instance.
            CheckAnswer(currentQuestion,
            options.currentFirstValue + optionNumber - 1));
        RefreshSolvedText();
    }

    void Menu()
    {
        SceneManager.LoadScene("menu");
        Destroy(GameObject.Find("AnswerActivity"));
        Destroy(gameObject);
    }
    #endregion

    #region Visualization
    private void ShowOptions(Question question)
    {
        options.currentFirstValue = QuestionGenerator.Instance.GetFirstOption(question);
        string appendix = question.GetPeriodAppendix();
        string prefix = question.GetPeriodPrefix();


        options.opt_1_text.text = prefix + options.currentFirstValue + appendix;
        options.opt_2_text.text = prefix + (options.currentFirstValue +1) +appendix;
        options.opt_3_text.text = prefix + (options.currentFirstValue +2) + appendix;
        options.opt_4_text.text = prefix + (options.currentFirstValue +3) + appendix;
        options.opt_5_text.text = prefix + (options.currentFirstValue +4) + appendix;
        options.opt_6_text.text = prefix + (options.currentFirstValue +5) + appendix;
        options.opt_7_text.text = prefix + (options.currentFirstValue +6) + appendix;
        options.opt_8_text.text = prefix + (options.currentFirstValue +7) + appendix;
        options.opt_9_text.text = prefix + (options.currentFirstValue +8)+ appendix;
    }
    
    private void UdatePeriodText(Question question)
    {
        string text = "";
        switch (question.currentPeriod)
        {
            case Period.century:
                text = LanguageSupport.Instance.GetText("game_century");
                break;
            case Period.decade:
                text = LanguageSupport.Instance.GetText("game_decade");
                break;
            case Period.year:
                text = LanguageSupport.Instance.GetText("game_year");
                break;
            case Period.month:
                text = LanguageSupport.Instance.GetText("game_month");
                break;
            case Period.day:
                text = LanguageSupport.Instance.GetText("game_day");
                break;
            default:
                text = "NONE";
                break;
        }
        period_text.text = text;
    }

    private void ShowQuestion(Question question)
    {
        questionText.text = question.text;
    }

    public void RefreshSolvedText()
    {
        solved_text.text = currentQuestion.GetSolvedString();
    }

    public void RefreshQuestion()
    {
        ShowQuestion(currentQuestion);
        ShowOptions(currentQuestion);
        //LanguageSupport.Instance.UdatePeriodText(currentQuestion.currentPeriod);
        UdatePeriodText(currentQuestion);
        RefreshSolvedText();
    }

    public void RefreshScore()
    {
        score_text.text = GameInfo.Instance.score.score + "*";
    }

    private void SetOptionsWidth()
    {
        //options.opt_1_obj.GetComponent<RectTransform>().sizeDelta = 
        SetOptionWidth(options.opt_1);
        SetOptionWidth(options.opt_2);
        SetOptionWidth(options.opt_3);
        SetOptionWidth(options.opt_4);
        SetOptionWidth(options.opt_5);
        SetOptionWidth(options.opt_6);
        SetOptionWidth(options.opt_7);
        SetOptionWidth(options.opt_8);
        SetOptionWidth(options.opt_9);

    }

    private void SetOptionWidth(Button option)
    {
        option.image.rectTransform.sizeDelta =
                    new Vector2(Screen.width / 4,
                    options.opt_3.image.rectTransform.sizeDelta.y);
    }

    #endregion
}
