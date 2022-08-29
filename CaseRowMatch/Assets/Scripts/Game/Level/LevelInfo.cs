using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public GameObject highest;
    private GameObject moveCount;
    private GameObject scoreTrack;
    public Board Board;
    public void Setup()
    {
        Board = FindObjectOfType<Board>();
        moveCount = gameObject.transform.GetChild(0).gameObject;
        highest = gameObject.transform.GetChild(2).gameObject;
        scoreTrack = gameObject.transform.GetChild(4).gameObject;
        SetmoveCount();
        SetscoreTrack();
        SetHighest();
    }

    public void SetmoveCount()
    {
        moveCount.GetComponent<TextMeshPro>().text = Board._moveCount.ToString();
    }

    public void SetscoreTrack()
    {
        scoreTrack.GetComponent<TextMeshPro>().text = Board.scoreTracker.ToString();
    }

    public void SetHighest()
    {
        highest.GetComponent<TextMeshPro>().text = Board.highestAttained.ToString();
    }
}
