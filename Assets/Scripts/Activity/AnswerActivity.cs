using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnswerActivity : MonoBehaviour {

    Canvas answerCanvas;
    Text answer_text;

    GameObject top_btn_obj;

    Button top_btn;
    Button bot_btn;

    Text top_btn_text;
    Text bot_btn_text;

    Image answerCanvas_bg;

    GameActivity gameActivity;

    bool status;


    // Use this for initialization
    void Start () {
        answerCanvas = GameObject.Find("AnswerCanvas").
            GetComponent<Canvas>();

        answer_text = GameObject.Find("answer_text").
            GetComponent<Text>();

        answerCanvas_bg = GameObject.Find("answerCanvas_bg").
            GetComponent<Image>();


        gameActivity = GameObject.Find("GameActivity").
            GetComponent<GameActivity>();

        InitializeOptions();
    }
	
    private void InitializeOptions()
    {
        top_btn_obj = GameObject.Find("top_btn");

        top_btn = top_btn_obj.GetComponent<Button>();
        bot_btn = GameObject.Find("bot_btn").GetComponent<Button>();

        top_btn_text = top_btn_obj.GetComponentInChildren<Text>();
        bot_btn_text = GameObject.Find("bot_btn").
            GetComponentInChildren<Text>();

        top_btn.onClick.AddListener(() => TopBtnOption());
        bot_btn.onClick.AddListener(() => BotBtnOption());
    }

	public void ShowAnswerStatus(bool status)
    {
        this.status = status;

        answerCanvas.enabled = true;
        top_btn_obj.SetActive(true);


        answer_text.text = status? 
            LanguageSupport.Instance.GetText("answer_correct"):
            LanguageSupport.Instance.GetText("answer_wrong");

        ShowOptions(status);

        if (status)
            answerCanvas_bg.color = new Color(0, 1, 0,0.5f);
        else
            answerCanvas_bg.color = new Color(1, 0, 0, 0.5f);


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
        answerCanvas.enabled = false;
        gameActivity.RefreshQuestion();
    }

    private void RevealQuestion(Question question)
    {
        question.currentPeriod = Period.none;
        answer_text.text = question.GetSolvedString();
        question.currentPeriod = Period.none;
        top_btn_obj.SetActive(false);

        DBAccess.Instance.CompleteQuestion(question, false);
    }

    private void NextQuestion()
    {
        answerCanvas.enabled = false;
        gameActivity.LoadNextQuestion();
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
}
