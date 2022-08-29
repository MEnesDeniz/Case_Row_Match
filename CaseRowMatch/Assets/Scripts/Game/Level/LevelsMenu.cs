using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LevelsMenu : MonoBehaviour
{

    public LevelUnit prefabLevelUnit;
    public LevelProvider LevelProvider;
    public LevelUnit unit;
    public int LevelCount;

    public IDictionary<int, int> levelBasicInfo = new Dictionary<int, int>();
    public List<int> highestScore = new List<int>();

    public void Setup()
    {
        LevelProvider.LevelBasicInfoFetch(ref levelBasicInfo, ref highestScore);
        LevelCount = 0;
        foreach(KeyValuePair<int, int> entry in levelBasicInfo)
        {
            levelUnitSpawn(entry.Key, entry.Value, highestScore[LevelCount]);
            LevelCount++;
        }

    }

    public void levelUnitSpawn(int levelNumber, int moveCount, int highestScore)
    {
        var cam = Camera.main;
        this.transform.localScale = new Vector2(0.92f, 0.92f);
        var renderingDimensions = cam.ScreenToWorldPoint(new Vector2(Display.main.renderingWidth, Display.main.renderingHeight));
        var _yTransform = (renderingDimensions.y * 0.8f);
        var distance_Cell = 2f;

        unit = Instantiate(prefabLevelUnit, new Vector2(0, _yTransform - ((levelNumber-1) * distance_Cell)), Quaternion.identity);
        locationOfUnit();
        unit.levelNumber = levelNumber;
        unit.moveCount = moveCount;
        unit.highestScore = highestScore;

    }

    public void locationOfUnit()
    {
        var _transform = unit.transform;
        _transform.parent = transform;
        _transform.localScale = new Vector2(1.5f, 1f);
    }

    
}
