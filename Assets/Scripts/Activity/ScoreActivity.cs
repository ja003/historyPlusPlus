using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ScoreActivity : MonoBehaviour {

    GameObject questionList;
    GameObject question_full_bg;
    GameObject question_full_text;
    GameObject question_result_text;

    GameObject reset_confirm_bg;
    Text reset_confirm_text;
    Button reset_confirm_yes_button;
    Button reset_confirm_no_button;
    Button reset_button;


    List<GameObject> qElements;
    List<Question> questions;

    void Start () {


        InitializeGameInfo();

        InitializeScoreVariables();

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
        LanguageSupport.Instance.InitializeScoreTexts();
        LoadTexts();

        PopulateQuestionList();

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

    private void InitializeScoreVariables()
    {
        questionList = GameObject.Find("QuestionList");

        question_full_bg = GameObject.Find("question_full_bg");
        question_full_bg.GetComponent<Button>().onClick.AddListener(
            () => HideFullQuestion());
        question_full_text = GameObject.Find("question_full_text");
        question_result_text = GameObject.Find("question_result_text");
        question_full_bg.SetActive(false);


        reset_confirm_bg = GameObject.Find("reset_confirm_bg");
        reset_confirm_text = GameObject.Find("reset_confirm_text").
            GetComponent<Text>();

        reset_confirm_yes_button = GameObject.Find("reset_confirm_yes_button").
            GetComponent<Button>();
        reset_confirm_yes_button.onClick.AddListener(
            () => ResetQuestions());

        reset_confirm_no_button = GameObject.Find("reset_confirm_no_button").
            GetComponent<Button>();
        reset_confirm_no_button.onClick.AddListener(
            () => HideConfirmReset());

        reset_button = GameObject.Find("reset_button").
            GetComponent<Button>();
        reset_button.onClick.AddListener(
            () => ShowConfirmReset());

        reset_confirm_bg.SetActive(false);

    }

    #endregion

    #region Functions
    private void PopulateQuestionList()
    {
        questions = DBAccess.Instance.GetCompletedQuestions();
        qElements = new List<GameObject>();

        if (questions.Count == 0)
        {
            ShowNoCompletedQuestions();
        }

        foreach (Question q in questions)
        {
            Debug.Log(q);

            GameObject qElement = (GameObject)Instantiate(
                Resources.Load("Prefabs/score_quest_bg"));
            qElement.name = "question_" + questions.IndexOf(q);
            qElements.Add(qElement);

            qElement.GetComponentInChildren<Text>().text = q.alias;
            if (q.solved)
                qElement.GetComponent<Image>().color = new Color(0, 1, 0);
            else
                qElement.GetComponent<Image>().color = new Color(1, 0, 0);


            /*qElement.GetComponent<Button>().
                onClick.AddListener(() => ShowFullQuestion(q.text));
            Debug.Log(q.text);*/

            qElement.transform.SetParent(questionList.transform);
            qElement.transform.localScale = new Vector3(1, 1, 1);

        }
        questionList.GetComponent<VerticalLayoutGroup>().childAlignment =
            TextAnchor.UpperCenter;

        //TODO: how the FUCK can I make
        //onClick.AddListener(() => ShowFullQuestion(i)
        for (int i = 0; i < questions.Count; i++)
        {
            GameObject q = GameObject.Find("question_" + i);
            q.GetComponent<Button>().
                onClick.AddListener(() => ShowFullQuestion(
                    int.Parse(q.name.Substring("question_".Length))));
        }

        /*
        qElements[0].GetComponent<Button>().
                onClick.AddListener(() => ShowFullQuestion(0));
        qElements[1].GetComponent<Button>().
                onClick.AddListener(() => ShowFullQuestion(1));*/

        

    }


    private void ClearQuestionList()
    {
        foreach (GameObject q in qElements)
        {
            Destroy(q);
        }
        qElements.Clear();
        questions.Clear();
        ShowNoCompletedQuestions();
    }

    void Menu()
    {
        SceneManager.LoadScene("menu");
    }
    /// <summary>
    /// resets all questions (not just completed) and score.
    /// current period = start period
    /// </summary>
    private void ResetQuestions()
    {
        DBAccess.Instance.ResetQuestions();
        GameInfo.Instance.score.score = 0;
        DBAccess.Instance.SaveScore(GameInfo.Instance.score);
        ClearQuestionList();
        HideConfirmReset();
    }

    #endregion


    




    #region Visualization
    
    private void ShowFullQuestion(int i)
    {
        Debug.Log(i);
        Question question = questions[i];
        Debug.Log("show " + question);

        question_full_bg.SetActive(true);
        question_full_text.GetComponent<Text>().text
            = question.text;
        question_result_text.GetComponent<Text>().text
            = question.GetCorrectAnswer();
    }
    
    private void ShowNoCompletedQuestions()
    {
        GameObject qElement = (GameObject)Instantiate(
                Resources.Load("Prefabs/score_quest_bg"));
        qElement.name = "question_none";
        qElements.Add(qElement);
        qElement.GetComponent<Image>().color = new Color(0, 0, 1);


        /*qElement.GetComponent<Button>().
            onClick.AddListener(() => ShowFullQuestion(q.text));
        Debug.Log(q.text);*/

        qElement.transform.SetParent(questionList.transform);
        qElement.transform.localScale = new Vector3(1, 1, 1);

        qElement.GetComponentInChildren<Text>().text =
            LanguageSupport.Instance.GetText("score_noquestions");
        qElement.GetComponentInChildren<Text>().resizeTextForBestFit = true;
    }

    private void ShowConfirmReset()
    {
        reset_confirm_bg.SetActive(true);
    }

    private void HideFullQuestion()
    {
        question_full_bg.SetActive(false);
    }
    private void HideConfirmReset()
    {
        reset_confirm_bg.SetActive(false);
    }

    private void LoadTexts()
    {

    }

    #endregion

}
