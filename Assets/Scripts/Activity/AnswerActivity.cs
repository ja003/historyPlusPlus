using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnswerActivity : MonoBehaviour {

    Canvas answerCanvas;
    Text answer_text;

    GameObject top_btn_obj;
    GameObject bot_btn_obj;

    Button top_btn;
    Button bot_btn;

    Text top_btn_text;
    Text bot_btn_text;

    Image answerCanvas_bg;

    GameActivity gameActivity;

    bool status;


    // Use this for initialization
    void Start () {
        Debug.Log("AnswerActivity");
        /*if (SceneManager.GetActiveScene().buildIndex == 1)  
            OnLevelWasLoaded(1);
        */
        answerCanvas = GameObject.Find("AnswerCanvas").
            GetComponent<Canvas>();
        Debug.Log(answerCanvas);
        Debug.Log(answerCanvas.enabled);

        answer_text = GameObject.Find("answer_text").
            GetComponent<Text>();

        answerCanvas_bg = GameObject.Find("answerCanvas_bg").
            GetComponent<Image>();


        gameActivity = GameObject.Find("GameActivity").
            GetComponent<GameActivity>();

        InitializeOptions();
    }



    /*void OnLevelWasLoaded(int level)
    {
        Debug.Log("LOAD");
        if (level == 1)
        {
            Debug.Log("GAME");

            
        }

    }*/

    #region Initialization
    private void InitializeOptions()
    {
        top_btn_obj = GameObject.Find("top_btn");
        bot_btn_obj = GameObject.Find("bot_btn");

        top_btn = top_btn_obj.GetComponent<Button>();
        bot_btn = bot_btn_obj.GetComponent<Button>();

        top_btn_text = top_btn_obj.GetComponentInChildren<Text>();
        bot_btn_text = bot_btn_obj.GetComponentInChildren<Text>();

        top_btn.onClick.AddListener(() => TopBtnOption());
        bot_btn.onClick.AddListener(() => BotBtnOption());
    }

    #endregion

    #region Functions
    

    private void IncreaseScore()
    {
        GameInfo.Instance.score.score++;
        DBAccess.Instance.SaveScore(GameInfo.Instance.score);
        gameActivity.RefreshScore();
    }

    private void TopBtnOption()
    {
        if (status)
            MoreQuestion();
        else
            RevealQuestion(gameActivity.currentQuestion);
    }

    private void BotBtnOption()
    {
        if (status)
            NextQuestion();
        else
            NextQuestion();
    }

    private void MoreQuestion()
    {
        HideAnswerCanvas();
        gameActivity.RefreshQuestion();
    }

    private void RevealQuestion(Question question)
    {
        //question.currentPeriod = Period.none;
        answer_text.text = question.GetCorrectAnswer();
        //question.currentPeriod = Period.none;
        top_btn_obj.SetActive(false);

        DBAccess.Instance.CompleteQuestion(question, false);
    }

    private void NextQuestion()
    {
        answerCanvas.enabled = false;
        gameActivity.LoadNextQuestion();
    }

    public void NoMoreQuestions()
    {
        ShowNoMoreOptions(DBAccess.Instance.IsAnyQuestionAvailable());
    }


    #endregion

    #region Vizualization
    private void ShowAnsverCanvas()
    {
        answerCanvas.enabled = true;
        top_btn_obj.SetActive(true);
    }

    private void HideAnswerCanvas()
    {
        answerCanvas.enabled = false;
    }

    private void ShowNoMoreOptions(bool anyAvailable)
    {
        ShowAnsverCanvas();

        answer_text.text = anyAvailable ?
            LanguageSupport.Instance.GetText("answer_anyAvailable") :
            LanguageSupport.Instance.GetText("answer_noAvailable");

        HideOptions();
    }

    private void HideOptions()
    {
        top_btn_obj.SetActive(false);
        bot_btn_obj.SetActive(false);
    }

    public void ShowAnswerStatus(bool status)
    {
        this.status = status;

        ShowAnsverCanvas();


        answer_text.text = status ?
            LanguageSupport.Instance.GetText("answer_correct") :
            LanguageSupport.Instance.GetText("answer_wrong");

        ShowOptions(status);

        if (status)
        {
            answerCanvas_bg.color = new Color(0, 1, 0, 0.5f);
            IncreaseScore();
        }
        else
            answerCanvas_bg.color = new Color(1, 0, 0, 0.5f);

    }

    private void ShowOptions(bool status)
    {
        if (status)
        {
            top_btn_text.text = LanguageSupport.Instance.GetText("answer_more");
            bot_btn_text.text = LanguageSupport.Instance.GetText("answer_next");
        }
        else
        {
            top_btn_text.text = LanguageSupport.Instance.GetText("answer_reveal");
            bot_btn_text.text = LanguageSupport.Instance.GetText("answer_next");
        }

        if (gameActivity.currentQuestion.completed)
            top_btn_obj.SetActive(false);
    }
    #endregion
}
