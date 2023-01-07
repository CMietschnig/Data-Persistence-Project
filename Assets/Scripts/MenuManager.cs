using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public TMP_InputField PlayerNameFromInput;
    public string currentPlayer;
    public string highScorePlayer;
    public int highScore;

    public TextMeshProUGUI bestScore;


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadPlayer();
        bestScore.text = "Best score: " + highScorePlayer + " " + highScore;
    }

    public void StartGame()
    {
        SetPlayerName();
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void SetPlayerName()
    {
        currentPlayer = PlayerNameFromInput.text;
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int score;
    }

    public void SavePlayer() { 

        SaveData data = new SaveData();
        data.name = currentPlayer;
        data.score = highScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadPlayer()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScorePlayer = data.name;
            highScore = data.score;
        }
    }
}
