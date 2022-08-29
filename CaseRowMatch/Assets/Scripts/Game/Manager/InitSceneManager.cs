using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSceneManager : MonoBehaviour
{
    public bool gameStartTrigger = false;
    public LevelsButton LevelsButton;
    public LevelsMenu LevelsMenu;
    public LevelProvider LevelProvider;
    public ButtonToGoDown ButtonToGoDown;
    public ButtonToGoUp ButtonToGoUp;
    private void Awake()
    {
        if(PlayerPrefs.GetInt("isBuilt") == 1)
        {
            gameStartTrigger = true;
            ButtonToGoDown.gameObject.SetActive(false);
            ButtonToGoUp.gameObject.SetActive(false);
            LevelsButton.gameObject.SetActive(false);
            LevelsMenu.Setup();
        }
        else
        {
            PlayerPrefs.SetInt("NextToUnlock", 2);
            LevelProvider.DownloadLevelFiles(LevelsMenu.Setup);
            LevelsMenu.gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStartTrigger == true)
        {
            LevelsButton.gameObject.SetActive(false);
            LevelsMenu.gameObject.SetActive(true);
            ButtonToGoDown.gameObject.SetActive(true);
            ButtonToGoUp.gameObject.SetActive(true);

            ButtonToGoDown.Setup(LevelsMenu.LevelCount);
            ButtonToGoUp.Setup();
            gameStartTrigger = false;
        }
    }
}
