using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;
using System.Linq;

public class DataService  {

	private SQLiteConnection _connection;

	public DataService(string DatabaseName){

#if UNITY_EDITOR
            var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        //Debug.Log("Final PATH: " + dbPath);     

	}

    #region Question

    public Question GetRandomQuestion(Category category, Language language)
    {
        string c = category.ToString();
        //Debug.Log(c);
        List<QuestionTable> questions = 
            _connection.Table<QuestionTable>().Where(
                x => x.category == c && x.completed == false).ToList();
        //Debug.Log(questions.Count);

        if (questions.Count > 0)
        {
            QuestionTable q = questions[Random.Range(0, questions.Count)];
            Debug.Log("random question: " + q.id);
            return q.GetQuestion(language);
        }
        else
        {
            //Debug.Log("no more questions for category: " + category);
            return new Question();
        }
    }

    public void CompleteQuetion(Question question, bool solved)
    {
        QuestionTable q = GetQuestionTable(question);
        q.completed = true;
        q.solved = solved;
        q.currentPeriod = question.currentPeriod.ToString();
        _connection.Update(q);
        Debug.Log(q.id + " completed (solved: " + solved + ")");
    }

    public void UpdateQuestionPeriod(Question question)
    {
        QuestionTable q = GetQuestionTable(question);
        q.currentPeriod = question.currentPeriod.ToString();
        _connection.Update(q);
        Debug.Log(q.id + " period updated to: " + q.currentPeriod);
    }

    public QuestionTable GetQuestionTable(Question question)
    {
        return _connection.Get<QuestionTable>(question.id);
    }
    #endregion

    public TextElement GetTextElement(string code)
    {
        return _connection.Get<TextElement>(code);
    }

    #region Settings
    public SettingsTable GetSettings()
    {
        return _connection.Table<SettingsTable>().Last();
    } 

    public void SaveSettings(SettingsTable settings)
    {
        _connection.DeleteAll<SettingsTable>();
        _connection.Insert(settings);
        //_connection.Update(settings);
    }
    #endregion

    #region Score
    public ScoreTable GetScore()
    {
        return _connection.Table<ScoreTable>().Last();
    }

    public void SaveScore(ScoreTable score)
    {
        _connection.DeleteAll<ScoreTable>();
        _connection.Insert(score);
        //_connection.Update(settings);
    }
    #endregion
}
