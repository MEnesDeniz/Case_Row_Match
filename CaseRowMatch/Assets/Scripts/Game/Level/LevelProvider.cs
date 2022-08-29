using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class LevelProvider : MonoBehaviour
{
    public int level_number;
    public int grid_width;
    public int grid_height;
    public int move_count;
    public int highestScore;
    public int isLock;

    public List<string> _grid = new List<string>();
    private string LevelUrlHead = "https://row-match.s3.amazonaws.com/levels/RM_";


    private List<string> identifier1 = new List<string> { "A", "B" };


    private List<int> identifier2 = new List<int> { 15, 10 };
    private List<string> fileNames = new List<string>();


    private Action waitComplete;
    private string dataPathToWrite;
    private string dataPathToRead;
    private int levelCount = 25;
 

    public IDictionary<int, int> levelBasicInfo = new Dictionary<int, int>();
    // Start is called before the first frame update
    public void DownloadLevelFiles(Action waitComplete)
    {
        if (PlayerPrefs.GetInt("isBuilt") == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < identifier2[i] + 1; j++)
                {
                    fileNames.Add(LevelUrlHead + identifier1[i] + j);
                }
            }
            dataPathToWrite = Application.persistentDataPath + "/DownloadedFileRW/";
            if (!Directory.Exists(dataPathToWrite)) Directory.CreateDirectory(dataPathToWrite);
            this.waitComplete = waitComplete;

            StartCoroutine(AcquireFromWeb(fileNames));
        }
    }

    private void StoreFiles(int LevelCount, string fileData)
    {
        string[] strArr = fileData.Split('\n');
        levelBasicInfo[int.Parse(strArr[0].Split(':')[1])] = int.Parse(strArr[3].Split(':')[1]);
        var path = dataPathToWrite + LevelCount + ".txt";
        File.WriteAllText(path, fileData);
    }

    private IEnumerator AcquireFromWeb(List<string> fileLines)
    {
        int LevelCount = 1;
        string fileData;
        foreach (string line in fileLines)
        {

                UnityWebRequest www = new UnityWebRequest(line);
                www.downloadHandler = new DownloadHandlerBuffer();
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    fileData = www.downloadHandler.text;
                    StoreFiles(LevelCount, fileData);
                }
                LevelCount++;
        }
        waitComplete();

    }

    public void LevelInfoFetch(int levelNo, Action waitComplete)
    {
        this.waitComplete = waitComplete;
        dataPathToRead = Application.persistentDataPath + "/DownloadedFileRW/";
        var path = dataPathToRead + levelNo + ".txt";
        var fileToRead = File.ReadAllLines(path);

        var levelNumber = fileToRead[0].Replace("level_number: ", "");
        var gridWidth = fileToRead[1].Replace("grid_width: ", "");
        var gridHeight = fileToRead[2].Replace("grid_height: ", "");
        var moveCount = fileToRead[3].Replace("move_count: ", "");
        var grid = fileToRead[4].Replace("grid: ", "");
        highestScore = 0;

        if (fileToRead.Length > 5)
        {
            highestScore = int.Parse(fileToRead[5].Replace("high_score: ", ""));
        }

        level_number = int.Parse(levelNumber);
        grid_width = int.Parse(gridWidth);
        grid_height = int.Parse(gridHeight);
        move_count = int.Parse(moveCount);
        _grid = grid.Split(new char[] { ',' }).ToList();

        waitComplete();

    }

    public void LevelBasicInfoFetch(ref IDictionary<int, int> dictionary, ref List<int> highestScore)
    {
        dataPathToRead = Application.persistentDataPath + "/DownloadedFileRW/";
        for (int i = 1; i < levelCount+1; i++)
        {
            var path = dataPathToRead + i + ".txt";
            if(path != null)
            {
                var fileToRead = File.ReadAllLines(path);

                var levelNumber = fileToRead[0].Replace("level_number: ", "");
                var moveCount = fileToRead[3].Replace("move_count: ", "");
                highestScore.Add(0);
                if(fileToRead.Length > 5)
                {
                    highestScore[i-1] = int.Parse(fileToRead[5].Replace("high_score: ", ""));
                }
                dictionary[int.Parse(levelNumber)] = int.Parse(moveCount);
            }
        }
 
    }

    public void LevelBasicInfoUpdate(int level, int score)
    {
        var modifyDataPath = Application.persistentDataPath + "/DownloadedFileRW/" + level + ".txt";
        var result = File.ReadAllLines(modifyDataPath).ToList();
        if (result.Count > 5)
        {
            result[5] = "high_score: " + score;
        }
        else
        {
            result.Add("high_score: " + score);
        }

        File.WriteAllLines(modifyDataPath, result);
    }

}
