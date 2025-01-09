using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Player.Scripts;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
[System.Serializable]
class GameSaveData
{
    public int currentLevel;
    public int currentScene;
    public List<LevelData> levelData = new List<LevelData>();
}
[System.Serializable]
public class LevelData
{
    public int level;
    public int scene;
    public int exp;
    public Vector3 position;
}
public class SavingFile : MonoBehaviour
{
    GameSaveData gameSaveData =  new GameSaveData(); // This is the data that will be saved
    LevelData level = new LevelData(); // This is the level that the player is currently on
    PlayerController playerController;
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        loadData();
        LoadGame();
    }
    public void SaveGame()
    {
        
        if (playerController != null)
        {
            level.position = playerController.transform.position;
            level.level = playerController.currentLevel;
            level.scene = playerController.currentScene;
        }
        Debug.Log("Saving Game");
        if (gameSaveData.levelData.Count != 0)
        {
            foreach (LevelData data in gameSaveData.levelData)
            {
                if (data.level == level.level)
                {
                    data.exp = level.exp;
                    data.position = level.position;
                    data.level = level.level;
                    data.scene = level.scene;
                    saveData();
                    return;
                }
            }
        }
        else
        {
            LevelData levelData = new LevelData();
            levelData.level = playerController.currentLevel;
            levelData.scene = playerController.currentScene;
            Debug.Log("PlayController level: " + playerController.currentScene);
            levelData.exp = 100;
            levelData.position = playerController.transform.position;
            gameSaveData.levelData.Add(levelData);
            saveData();
        }
        
    }
    public void LoadGame()
    {
        if(gameSaveData == null)
        {
            return;
        }
        foreach (LevelData data in gameSaveData.levelData)
        {
            if (data.level == gameSaveData.currentLevel && data.scene == gameSaveData.currentScene)
            {
                level.exp = data.exp;
                level.position = data.position;
                level.level = data.level;
                level.scene = data.scene;
                Debug.Log("Data loaded for level " + level.level);
                return;
            }
        }
        Debug.Log("No data found for level " + level);
    }
    public void loadData()
    {
        string file = "save.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);
        if (!File.Exists(filePath))
        {
            gameSaveData = new GameSaveData { currentLevel = 1,currentScene = 1, levelData = new List<LevelData>() };
            string json = JsonUtility.ToJson(gameSaveData, true);
            File.WriteAllText(filePath, json);
            //File.WriteAllText(filePath, "");
        }
        else
        {
            string data = File.ReadAllText(filePath);
            gameSaveData = JsonUtility.FromJson<GameSaveData>(data);
            Debug.Log("Data Loaded");
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
    
}
