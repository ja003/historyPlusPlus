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

    RectTransform period_bg_rect;
    RectTransform period_text_rect;
    RectTransform question_bg_rect;
    RectTransform question_text_rect;

    public Question currentQuestion;
    AnswerActivity answerActivity;

    static float periodPosY = Screen.height / 2;
    static float periodHeight = 40;
    static float gapSize = 10;
    static float leftMargin = 50;


    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        
        InitializeOptions();

        InitializeGameVariables();

        InitializeGameInfo();

        

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
        SetLayoutParameters();

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

        question_bg_rect = GameObject.Find("question_bg").
            GetComponent<RectTransform>();
        question_text_rect = GameObject.Find("question_text").
            GetComponent<RectTransform>();

        period_text = GameObject.Find("period_text").
            GetComponent<Text>();
        period_bg_rect = GameObject.Find("period_bg").
            GetComponent<RectTransform>();
        period_text_rect = GameObject.Find("period_text").
            GetComponent<RectTransform>();

        solved_text = GameObject.Find("solved_text").
            GetComponent<Text>();
        score_text = GameObject.Find("score_text").
            GetComponent<Text>();
    }

    private void InitializeOptions()
    {
        options = new Options();

        options.opt_1_obj = GameObject.Find("opt_1");
        options.opt_1 = options.opt_1_obj.GetComponent<Button>();
        options.opt_1.onClick.AddListener(() => SelectOption(1));
        options.opt_1_text = options.opt_1_obj.GetComponentInChildren<Text>();
        options.opt_1_rect = options.opt_1_obj.GetComponent<RectTransform>();

        options.opt_2_obj = GameObject.Find("opt_2");
        options.opt_2 = options.opt_2_obj.GetComponent<Button>();
        options.opt_2.onClick.AddListener(() => SelectOption(2));
        options.opt_2_text = options.opt_2_obj.GetComponentInChildren<Text>();
        options.opt_2_rect = options.opt_2_obj.GetComponent<RectTransform>();

        options.opt_3_obj = GameObject.Find("opt_3");
        options.opt_3 = options.opt_3_obj.GetComponent<Button>();
        options.opt_3.onClick.AddListener(() => SelectOption(3));
        options.opt_3_text = options.opt_3_obj.GetComponentInChildren<Text>();
        options.opt_3_rect = options.opt_3_obj.GetComponent<RectTransform>();

        options.opt_4_obj = GameObject.Find("opt_4");
        options.opt_4 = options.opt_4_obj.GetComponent<Button>();
        options.opt_4.onClick.AddListener(() => SelectOption(4));
        options.opt_4_text = options.opt_4_obj.GetComponentInChildren<Text>();
        options.opt_4_rect = options.opt_4_obj.GetComponent<RectTransform>();

        options.opt_5_obj = GameObject.Find("opt_5");
        options.opt_5 = options.opt_5_obj.GetComponent<Button>();
        options.opt_5.onClick.AddListener(() => SelectOption(5));
        options.opt_5_text = options.opt_5_obj.GetComponentInChildren<Text>();
        options.opt_5_rect = options.opt_5_obj.GetComponent<RectTransform>();

        options.opt_6_obj = GameObject.Find("opt_6");
        options.opt_6 = options.opt_6_obj.GetComponent<Button>();
        options.opt_6.onClick.AddListener(() => SelectOption(6));
        options.opt_6_text = options.opt_6_obj.GetComponentInChildren<Text>();
        options.opt_6_rect = options.opt_6_obj.GetComponent<RectTransform>();

        options.opt_7_obj = GameObject.Find("opt_7");
        options.opt_7 = options.opt_7_obj.GetComponent<Button>();
        options.opt_7.onClick.AddListener(() => SelectOption(7));
        options.opt_7_text = options.opt_7_obj.GetComponentInChildren<Text>();
        options.opt_7_rect = options.opt_7_obj.GetComponent<RectTransform>();

        options.opt_8_obj = GameObject.Find("opt_8");
        options.opt_8 = options.opt_8_obj.GetComponent<Button>();
        options.opt_8.onClick.AddListener(() => SelectOption(8));
        options.opt_8_text = options.opt_8_obj.GetComponentInChildren<Text>();
        options.opt_8_rect = options.opt_8_obj.GetComponent<RectTransform>();

        options.opt_9_obj = GameObject.Find("opt_9");
        options.opt_9 = options.opt_9_obj.GetComponent<Button>();
        options.opt_9.onClick.AddListener(() => SelectOption(9));
        options.opt_9_text = options.opt_9_obj.GetComponentInChildren<Text>();
        options.opt_9_rect = options.opt_9_obj.GetComponent<RectTransform>();
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
        switch (GameInfo.Instance.settings.GetDifficulty())
        {
            case Difficulty.easy:
                switch (optionNumber)
                {
                    case 1:
                        answerActivity.ShowAnswerStatus(
                            QuestionGenerator.Instance.CheckAnswer(
                                currentQuestion,options.currentFirstValue));
                        break;
                    case 3:
                        answerActivity.ShowAnswerStatus(
                            QuestionGenerator.Instance.CheckAnswer(
                                currentQuestion, options.currentFirstValue+1));
                        break;
                    case 7:
                        answerActivity.ShowAnswerStatus(
                            QuestionGenerator.Instance.CheckAnswer(
                                currentQuestion, options.currentFirstValue+2));
                        break;
                    case 9:
                        answerActivity.ShowAnswerStatus(
                            QuestionGenerator.Instance.CheckAnswer(
                                currentQuestion, options.currentFirstValue+3));
                        break;
                }
                break;
            case Difficulty.hard:
                answerActivity.ShowAnswerStatus(
                    QuestionGenerator.Instance.CheckAnswer(
                        currentQuestion,options.currentFirstValue + optionNumber - 1));
                break;
            default:
                Debug.Log("difficulty fail");
                break;
        }
        
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

        switch (GameInfo.Instance.settings.GetDifficulty())
        {
            case Difficulty.easy:
                options.opt_2.gameObject.SetActive(false);
                options.opt_4.gameObject.SetActive(false);
                options.opt_5.gameObject.SetActive(false);
                options.opt_6.gameObject.SetActive(false);
                options.opt_8.gameObject.SetActive(false);
                break;
            case Difficulty.hard:
                options.opt_2.gameObject.SetActive(true);
                options.opt_4.gameObject.SetActive(true);
                options.opt_5.gameObject.SetActive(true);
                options.opt_6.gameObject.SetActive(true);
                options.opt_8.gameObject.SetActive(true);
                break;
        }

       switch (GameInfo.Instance.settings.GetDifficulty())
        {
            case Difficulty.easy:
                options.opt_1_text.text = prefix + options.currentFirstValue + appendix;
                options.opt_3_text.text = prefix + (options.currentFirstValue + 1) + appendix;
                options.opt_7_text.text = prefix + (options.currentFirstValue + 2) + appendix;
                options.opt_9_text.text = prefix + (options.currentFirstValue + 3) + appendix;
                break;
            case Difficulty.hard:
                options.opt_1_text.text = prefix + options.currentFirstValue + appendix;
                options.opt_2_text.text = prefix + (options.currentFirstValue + 1) + appendix;
                options.opt_3_text.text = prefix + (options.currentFirstValue + 2) + appendix;
                options.opt_4_text.text = prefix + (options.currentFirstValue + 3) + appendix;
                options.opt_5_text.text = prefix + (options.currentFirstValue + 4) + appendix;
                options.opt_6_text.text = prefix + (options.currentFirstValue + 5) + appendix;
                options.opt_7_text.text = prefix + (options.currentFirstValue + 6) + appendix;
                options.opt_8_text.text = prefix + (options.currentFirstValue + 7) + appendix;
                options.opt_9_text.text = prefix + (options.currentFirstValue + 8) + appendix;
                break;
            default:
                Debug.Log("difficulty fail");
                break;
        }        
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

    private void SetLayoutParameters()
    {
        SetOptionsWidth();
        SetPeriodPosition();
        SetOptionsPosition();
        SetQuestionPosition();

    }
    private void SetPeriodPosition()
    {
        //period_bg_rect.anchoredPosition

        period_bg_rect.anchoredPosition = new Vector2(
            period_bg_rect.anchoredPosition.x,
            periodPosY);
        period_text_rect.anchoredPosition = new Vector2(
            period_text_rect.anchoredPosition.x,
            periodPosY);
    }

    private void SetQuestionPosition()
    {
        question_bg_rect.offsetMin = new Vector2(
            question_bg_rect.offsetMin.x,
            periodPosY + periodHeight + gapSize);
        question_text_rect.offsetMin = new Vector2(
            question_text_rect.offsetMin.x,
            periodPosY + periodHeight + gapSize);
    }

    private void SetOptionsPosition()
    {
        options.opt_1_rect.anchoredPosition = new Vector2(
            options.opt_1_rect.anchoredPosition.x,
            periodPosY - gapSize);
        options.opt_2_rect.anchoredPosition = new Vector2(
            options.opt_2_rect.anchoredPosition.x,
            periodPosY - gapSize);
        options.opt_3_rect.anchoredPosition = new Vector2(
            options.opt_3_rect.anchoredPosition.x,
            periodPosY - gapSize);

        options.opt_4_rect.anchoredPosition = new Vector2(
            options.opt_4_rect.anchoredPosition.x,
            periodPosY /2);
        options.opt_5_rect.anchoredPosition = new Vector2(
            options.opt_5_rect.anchoredPosition.x,
            periodPosY / 2);
        options.opt_6_rect.anchoredPosition = new Vector2(
            options.opt_6_rect.anchoredPosition.x,
            periodPosY / 2);

        options.opt_7_rect.anchoredPosition = new Vector2(
            options.opt_7_rect.anchoredPosition.x,
            gapSize);
        options.opt_8_rect.anchoredPosition = new Vector2(
            options.opt_8_rect.anchoredPosition.x,
            gapSize);
        options.opt_9_rect.anchoredPosition = new Vector2(
            options.opt_9_rect.anchoredPosition.x,
            gapSize);
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
        switch (GameInfo.Instance.settings.GetDifficulty())
        {
            case Difficulty.easy:
                option.image.rectTransform.sizeDelta =
                    //new Vector2(Screen.width / 2.75f,Screen.height/4.8f);
                    new Vector2(
                        (Screen.width - 2*leftMargin - gapSize)/2, 
                        (periodPosY - 3*gapSize) / 2);
                break;
            case Difficulty.hard:
                option.image.rectTransform.sizeDelta =
                    new Vector2(
                        (Screen.width - 100 - 2*gapSize)/3,
                        (periodPosY - 4 * gapSize) / 3);
                //new Vector2(Screen.width / 4,options.opt_3.image.rectTransform.sizeDelta.y);
                break;
            default:
                Debug.Log("difficulty fail");
                break;
        }
        
    }

    #endregion
}
