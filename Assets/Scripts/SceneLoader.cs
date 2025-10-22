using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    private string saveLocation;

    private void Awake()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log("[MainMenu] Save file path: " + saveLocation);
    }

    public void OnNewGame()
    {
        Debug.Log("[MainMenu] Starting new game...");

        if (File.Exists(saveLocation))
        {
            try
            {
                File.Delete(saveLocation);
                Debug.Log("[MainMenu] Old save file deleted.");
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("[MainMenu] Failed to delete old save: " + ex.Message);
            }
        }
        SceneManager.LoadScene("CemeteryScene");
    }

    public void OnLoadGame()
    {
        if (File.Exists(saveLocation))
        {
            Debug.Log("[MainMenu] Save file found. Loading game...");
            SceneManager.LoadScene("CemeteryScene");
        }
        else
        {
            Debug.LogWarning("[MainMenu] No save file found. Starting new game instead...");
            OnNewGame();
        }
    }

    public void OnQuit()
    {
        Debug.Log("[MainMenu] Quitting game...");
        Application.Quit();
    }
}
