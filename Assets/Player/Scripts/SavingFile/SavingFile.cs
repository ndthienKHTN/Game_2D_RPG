using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Player.Scripts;
using UnityEngine;

[System.Serializable]
public class GameSaveData
{
    public int currentLevel;
    public int currentScene;
    public LevelData levelData = new LevelData();
}

[System.Serializable]
public class LevelData
{
    public int level;
    public int scene;
    public float exp;
    public float hp;
    public float gold;
    public Vector3 position;
}

[System.Serializable]
public class SavingFile : MonoBehaviour
{
    private static SavingFile instance;
    public static SavingFile Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SavingFile>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<SavingFile>();
                    singletonObject.name = typeof(SavingFile).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    GameSaveData gameSaveData = new GameSaveData();
    public GameSaveData GameSaveData
    {
        get { return gameSaveData; }
    }

    PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.Instance;
        if (playerController == null)
        {
            Debug.Log("PlayerController is not initialized.");
            playerController = FindObjectOfType<PlayerController>();
        }
        loadData();
        LoadGame();
    }
    void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        playerController = PlayerController.Instance;
    }
    public void SaveGame()
    {
        if (playerController==null)
        {
            Debug.Log("PlayerController is not initialized.");
            playerController = FindObjectOfType<PlayerController>();
        }
        gameSaveData.currentLevel = playerController.currentLevel;
        gameSaveData.currentScene = playerController.currentScene;
        if (playerController != null)
        {
            gameSaveData.levelData.position = playerController.transform.position;
            gameSaveData.levelData.level = playerController.currentLevel;
            gameSaveData.levelData.scene = playerController.currentScene;
            gameSaveData.levelData.exp = playerController.EXP;
            gameSaveData.levelData.hp = playerController.currentHealth;
            gameSaveData.levelData.gold = playerController.goldCounter;

            // Debug logs to verify values
            Debug.Log("Saving Game");
            Debug.Log("Position: " + gameSaveData.levelData.position);
            Debug.Log("Level: " + gameSaveData.levelData.level);
            Debug.Log("Scene: " + gameSaveData.levelData.scene);
            Debug.Log("Exp: " + gameSaveData.levelData.exp);
            Debug.Log("HP: " + gameSaveData.levelData.hp);
            Debug.Log("Gold: " + gameSaveData.levelData.gold);

            saveData();
        }
        else
        {
            Debug.LogError("PlayerController is not initialized.");
        }
    }
    public void saveData()
    {
        string file = "save.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);
        string json = JsonUtility.ToJson(gameSaveData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("File saved, at path " + filePath);
    }
    public void LoadGame()
    {
        if (gameSaveData == null)
        {
            return;
        }

        if (gameSaveData.levelData.level == gameSaveData.currentLevel && gameSaveData.levelData.scene == gameSaveData.currentScene)
        {
            //Debug.Log("Data loaded for level " + gameSaveData.levelData.level);
        }
        else
        {
            Debug.Log("No data found for level " + gameSaveData.currentLevel);
        }
    }

    public void loadData()
    {
        string file = "save.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);
        if (!File.Exists(filePath))
        {
            gameSaveData = new GameSaveData { currentLevel = 1, currentScene = 1, levelData = new LevelData() };
            string json = JsonUtility.ToJson(gameSaveData, true);
            File.WriteAllText(filePath, json);
        }
        else
        {
            string data = File.ReadAllText(filePath);
            gameSaveData = JsonUtility.FromJson<GameSaveData>(data);
        }
    }

    
}
