using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MoveableItem : Item
{
    private MoveableItem itemToSwap;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 currentPosition;
    private bool mousePressed;
    public float swipeAngle = 0;
    private const float DurationOfTween = 0.25f;
    private bool isMatchPossible = true;

    // Update is called once per frame
    void Update()
    {
            
            if (board.GameStateIdentifier == Board.GameState.operatable && mousePressed && Input.GetMouseButtonUp(0) && this != null)
            {
                mousePressed = false;
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if(Vector2.Distance(firstTouchPosition, finalTouchPosition) > 0.3f)
                {
                    CalculateAngle();
                    board.RowMatchFinder.FindMatches();
                    isMatchPossible = board.RowMatchFinder.PossibleMatch();
                    if (!isMatchPossible)
                    {
                        board._moveCount = 0; 
                        board.RowMatchFinder.gameStatusCheck();
                    }
                    board.RowMatchFinder.gameStatusCheck();
                }
            }
    }

    public override void PositionSet(Vector2Int position, Board board)
    {
        this.indexPos = position;
        this.board = board;
        this.isDone = false;
    }
    
    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;
    }

    public void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle *= Mathf.Rad2Deg;
        if (swipeAngle > -45 && swipeAngle < 45 && indexPos.x < board._width - 1)//Right Swipe
        {
            MoveOperation(1, 0);
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && indexPos.y < board._heigth - 1)//Up Swipe
        {
            MoveOperation(0, 1);
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && indexPos.y > 0)//Down Swipe
        {
            MoveOperation(0, -1);
        }
        else if ((swipeAngle > 135 && indexPos.x > 0) || (swipeAngle < -135 && indexPos.x > 0))//Left Swipe
        {
            MoveOperation(-1, 0);
        }
    }

    void MoveOperation(int columnMove, int rowMove)
    {
        
        if (board.GameStateIdentifier == Board.GameState.operatable && board.itemsPos[indexPos.x + columnMove, indexPos.y + rowMove] != null)
        {
            board._moveCount--;

            itemToSwap = board.itemsPos[indexPos.x + columnMove, indexPos.y + rowMove];
            itemToSwap.indexPos.x -= columnMove;
            itemToSwap.indexPos.y -= rowMove;
            this.indexPos.x += columnMove;
            this.indexPos.y += rowMove;

            board.itemsPos[itemToSwap.indexPos.x, itemToSwap.indexPos.y] = itemToSwap;
            board.itemsPos[this.indexPos.x, this.indexPos.y] = this;

            board.GameStateIdentifier = Board.GameState.animating;
            board.LevelInfo.SetmoveCount();
            SwapAnimation();
            board.GameStateIdentifier = Board.GameState.operatable;
        }

    }
    public async void SwapAnimation()
    {
        await Swap(itemToSwap);
        
    }

    public async Task Swap(MoveableItem itemToSwap)
    {
        var itemItself = transform;
        var itemTarget = itemToSwap.transform;
        var swapSequence = DOTween.Sequence();
        swapSequence.Join(itemItself.DOMove(itemTarget.position, DurationOfTween))
                .Join(itemTarget.DOMove(itemItself.position, DurationOfTween));
        await swapSequence.Play().AsyncWaitForCompletion();
    }



}
