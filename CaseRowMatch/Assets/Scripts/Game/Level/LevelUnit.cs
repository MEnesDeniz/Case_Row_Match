using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUnit : MonoBehaviour
{
    public int levelNumber;
    public int moveCount;
    public int highestScore;
    public int isAvaiable = 0;
    public GameObject maxScore;
    public GameObject levelBasicInfo;
    public PlayButton playButton;
   

    public void Start()
    {
        var _transform = this.transform;
        maxScore = _transform.GetChild(0).gameObject;
        levelBasicInfo = _transform.GetChild(1).gameObject;

        SetLevelLock();
        SetMaxScore();
        SetNameAndMoveCount();
    }

    private void SetLevelLock()
    {
        if (isAvaiable == 1 || levelNumber < PlayerPrefs.GetInt("NextToUnlock"))
        {
            playButton.transform.GetChild(0).GetComponent<TextMeshPro>().text = "Play";
        }
        else
        {
            var _buttonText = playButton.transform.GetChild(0).GetComponent<TextMeshPro>();
            _buttonText.text = "Locked";
            _buttonText.color = Color.red;
        }
    }

    public void SetMaxScore()
    {
        maxScore.GetComponent<TextMeshPro>().text = "Highest Score: " + highestScore;
    }

    private void SetNameAndMoveCount()
    {
        levelBasicInfo.GetComponent<TextMeshPro>().text = "Level " + levelNumber + " - Moves: " + moveCount;
    }
    


}
