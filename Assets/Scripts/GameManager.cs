using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.UI;





#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    private TMP_InputField playerNameField;
    public TextMeshProUGUI highScoreText;

    public string playerName;

    public string savedName;
    public int highScore = 0;
    void Awake()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        LoadNameAndScore();
    }

    void Start()
    {
        playerNameField = GameObject.Find("NameInput").GetComponent<TMP_InputField>();
        
        highScoreText.text = $"Best Score: {savedName} : {highScore}";

         
    }

    public void StartGame()
    {
        playerName = playerNameField.text;
        SceneManager.LoadScene(1);
    }

        public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();

#endif
    }

    [SerializeField]
    class SaveData
    {
        public string Name;
        public int score;
    }

    public void SaveNameAndScore()
    {
        SaveData data = new SaveData();
        data.Name = savedName;
        data.score = highScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
        
    }

    public void LoadNameAndScore()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            savedName = data.Name;
            highScore = data.score;
        }
    }
}
