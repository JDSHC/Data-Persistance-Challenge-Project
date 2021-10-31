using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor;
using UnityEngine.SocialPlatforms.Impl;

public class UIMenuManager : MonoBehaviour
{
    
    public static UIMenuManager Instance;
    private string playerName;

    public GameObject inputField;
    
    private void Awake()
    {

        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public string GetPlayerName()
    {
        return playerName;
    }
    
    public void StartNew()
    {
        playerName = inputField.GetComponent<TMP_InputField>().text;
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit(); // original code to quit Unity player
#endif
    }
    
    [Serializable]
    class SaveData
    {
        public int score;
        public string playerName;
    }

    public void SaveScore(int score)
    {
        SaveData data = new SaveData();
        data.score = score;
        data.playerName = playerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public int ReadScore()
    {
        int score = 0;
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            score = data.score;
        }

        return score;
    }

    public string ReadHighScoreName()
    {
        string name = "";
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            name = data.playerName;
        }

        return name;
    }
    
    
}