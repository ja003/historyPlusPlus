using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo : MonoBehaviour {

    //public Language language;
    public List<Category> categories;

    public SettingsTable settings;


    public static GameInfo Instance { get; private set; }

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

    void Start()
    {

        categories = new List<Category>();
        categories.Add(Category.War);
        categories.Add(Category.Science);
        categories.Add(Category.Politics);
        categories.Add(Category.Others);

        settings = DBAccess.Instance.GetSettings();
        LanguageSupport.Instance.SetLanguage(settings);

        //StartCoroutine(LateStart(0.1f));
    }
    
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
