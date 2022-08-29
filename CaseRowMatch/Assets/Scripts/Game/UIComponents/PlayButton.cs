using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class PlayButton : MonoBehaviour
{
    public LevelUnit LevelUnit;
    public ScenesManager ScenesManager;

    private void Awake()
    {
        ScenesManager = FindObjectOfType<ScenesManager>();
    }

    private void OnMouseDown()
    {
        PlayerPrefs.SetInt("LevelToStart", LevelUnit.levelNumber);
        Debug.Log("Clicked to the Level");
        
        if (LevelUnit.isAvaiable == 1 || LevelUnit.levelNumber < PlayerPrefs.GetInt("NextToUnlock"))
        {
            ScenesManager.LoadLevels();
        }
    }

}
