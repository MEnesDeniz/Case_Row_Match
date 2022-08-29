using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static void LoadMain()
    {
        PlayerPrefs.SetInt("isBuilt", 1);
        SceneManager.LoadScene("MainScene");
    }

    public static void LoadLevels()
    {
        SceneManager.LoadScene("LevelScene");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("isBuilt", 0); 
    }


}
