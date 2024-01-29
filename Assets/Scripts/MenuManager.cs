using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI highScoreText;
    public string playerName;
    public string bestPlayer;
    private GameObject mainManager;
    public MainManager mainManagerScript;
    private bool isPlaying;
    public int bestScore;
    public int currentScore = 0;
    public void StartGame()
    {
        playerName = inputText.text;
        SceneManager.LoadScene(1);
    }
    public void Quit()
    {
        Instance.SaveBestScore();
        Instance.SaveBestPlayer();
#if UNITY_EDITOR

        EditorApplication.isPlaying = false;
#else

        Application.Quit();
#endif
    }
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadBestPlayer();
        LoadBestScore();
    }
    private void Update()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            bestPlayer = playerName;
        }
        if (isPlaying)
        {
            currentScore = mainManagerScript.m_Points;
        }
        if (SceneManager.GetActiveScene().name == "main")
        {
            mainManager = GameObject.Find("MainManager");
            mainManagerScript = mainManager.GetComponent<MainManager>();
            isPlaying = true;
        }
        if (SceneManager.GetActiveScene().name == "menu")
        {
            highScoreText = GameObject.Find("High Score").GetComponent<TextMeshProUGUI>();
            highScoreText.text = "High Score: " + bestPlayer + " " + bestScore.ToString();
        }
    }


    [System.Serializable]
    public class SaveDataPlayer
    {
        public string bestPlayer;
    }
    public class SaveDataScore
    {
        public int bestScore;
    }
    public void SaveBestPlayer()
    {
        SaveDataPlayer data = new SaveDataPlayer();
        data.bestPlayer = bestPlayer;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void SaveBestScore()
    {
        SaveDataScore data = new SaveDataScore();
        data.bestScore = bestScore;

        string jsona = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.jsona", jsona);
    }
    public void LoadBestPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveDataPlayer data = JsonUtility.FromJson<SaveDataPlayer>(json);

            bestPlayer = data.bestPlayer;
        }
    }
    public void LoadBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.jsona";
        if (File.Exists(path))
        {
            string jsona = File.ReadAllText(path);
            SaveDataScore data = JsonUtility.FromJson<SaveDataScore>(jsona);

            bestScore = data.bestScore;
        }
    }
}