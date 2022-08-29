using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;
using System.Threading;

public class GameController : MonoBehaviour
{
    public Board Board;
    public SuccessScreen SuccessScreen;
    public ScenesManager ScenesManager;
    private Action waitComplete;
    private bool newHigh = false;

    private void Awake()
    {
        Board = FindObjectOfType<Board>();
        ScenesManager = FindObjectOfType<ScenesManager>();
    }

    public void CheckEnd()
    {
        Board.GameStateIdentifier = Board.GameState.operatable;
        if (Board.GameStateIdentifier == Board.GameState.operatable && Board._moveCount == 0)
        {
            if (Board.GameStateIdentifier == Board.GameState.operatable)
            {
                endSequence();
                if (Board.GameStateIdentifier == Board.GameState.end && newHigh == true)
                {
                    Invoke(nameof(endGameSequenceAsync), 1f);
                }
                else
                {
                    Invoke(nameof(notHighScoreEndAsync), 0.2f);
                }
            }
        }
    }

    private async Task AnimateSuccess()
    {
        var cloudSeq = DOTween.Sequence();
        cloudSeq.Join(SuccessScreen.cloud.transform.DOScale(new Vector3(1.5f / 5, 1.5f / 5, 1f), 1.5f));
        cloudSeq.Join(Camera.main.DOShakePosition(1.5f, 0.2f));
        await cloudSeq.Play().AsyncWaitForCompletion();
    }

    public void endSequence()
    {
        if (Board.scoreTracker > Board.highestAttained)
        {
            newHigh = true;
            Board.highestAttained = Board.scoreTracker;
            Board.LevelProvider.LevelBasicInfoUpdate(Board._levelCount, Board.scoreTracker);
            SuccessScreen.setScore(Board.scoreTracker);
        }
        Board.GameStateIdentifier = Board.GameState.end;
    }

    private async Task endGameSequenceAsync()
    {
        Board.LevelInfo.gameObject.SetActive(false);
        Board.gameObject.SetActive(false);
        Disable();
        Camera.main.GetComponent<Camera>().backgroundColor = Color.blue;
        SuccessScreen.gameObject.SetActive(true);
        await AnimateSuccess();
        PlayerPrefs.SetInt("NextToUnlock", Board._levelCount + 2);
        ScenesManager.LoadMain();
    }

    private async Task notHighScoreEndAsync()
    {
        await CameraShacker();
        ScenesManager.LoadMain();
    }

    private async Task CameraShacker()
    {
        var cameraSeq = DOTween.Sequence();
        cameraSeq.Join(Camera.main.DOShakePosition(1f, 2f));
        await cameraSeq.Play().AsyncWaitForCompletion();
    }

    private void Disable()
    {
        foreach (var c in Board.gameObject.GetComponentsInChildren<Collider>())
        {
            if (c.isTrigger) c.enabled = false;
        }
    }

}
