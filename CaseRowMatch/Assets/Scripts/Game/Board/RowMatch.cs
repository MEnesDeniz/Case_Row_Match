using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowMatch : MonoBehaviour
{
    private Board board;
    public GameController GameContoller;
    List<Item> possibleMatchContainer = new List<Item>();
    private int[] possibleMatchCounter = new int[4] {0,0,0,0};

    private void Awake()
    {
        board = FindObjectOfType<Board>();
        GameContoller = FindObjectOfType<GameController>();
    }

    public bool PossibleMatch()
    {
        possibleMatchSetter();

        for (int i = 0; i < board._heigth; i++)
        {
            if (board.itemsPos[0, i] == null)
            {
                possibleMatchSetter();
                continue;
            }
            Item.ItemTypes typeOfItem = board.itemsPos[0, i].typeOfItem;
            if (board.itemsPos[0, i].isDone == true || typeOfItem == Item.ItemTypes.tick)
            {
                if (possibleMatchDetecter())
                {
                    return true;
                }
                possibleMatchSetter();
                continue;
            }
            else
            {
                for (int j = 0; j < board._width; j++)
                {
                    MoveableItem currentItem = board.itemsPos[j, i];
                    if (currentItem != null)
                    {
                        itemTypeCollector(currentItem.typeOfItem);
                    }
                }

                if (possibleMatchDetecter())
                {
                    return true;
                }
            }

        }

        return possibleMatchDetecter();

    }

    private void possibleMatchSetter()
    {
        possibleMatchCounter = new int[4] { 0, 0, 0, 0 };
    }


    private bool possibleMatchDetecter()
    {
        bool flag = false;
        for (int i = 0; i < 4; i++)
        {
            if (possibleMatchCounter[i] >= board._width)
            {
                flag = true;
                return flag;
            }
        }
        return flag;
    }

    private void itemTypeCollector(Item.ItemTypes itemType)
    {

        if (itemType == Item.ItemTypes.red)
        {
            possibleMatchCounter[0]++;
        }
        else if (itemType == Item.ItemTypes.green)
        {
            possibleMatchCounter[1]++;
        }
        else if (itemType == Item.ItemTypes.blue)
        {
            possibleMatchCounter[2]++;
        }
        else if (itemType == Item.ItemTypes.yellow)
        {
            possibleMatchCounter[3]++;
        }

    }


    public void FindMatches()
    {
       
        for (int i = 0; i < board._heigth; i++)
        {
            if(board.itemsPos[0, i] == null)
            {
                continue;
            }
            Item.ItemTypes typeOfItem = board.itemsPos[0, i].typeOfItem;
            if (board.itemsPos[0, i].isDone == true || typeOfItem == Item.ItemTypes.tick)
            {
                possibleMatchContainer.Clear();
                continue;
            }
            else
            {
                for (int j = 0; j < board._width; j++)
                {
                    MoveableItem currentItem = board.itemsPos[j, i];
                    if (currentItem != null)
                    {
                        if (currentItem.typeOfItem == typeOfItem)
                        {
                            possibleMatchContainer.Add(board.itemsPos[j, i]);
                        }
                        else
                        {
                            possibleMatchContainer.Clear();
                        }
                    }
                }

                if (possibleMatchContainer.Count == board._width)
                {
                    foreach (Item item in possibleMatchContainer)
                    {
                        board.GameStateIdentifier = Board.GameState.animating;
                        var point = item.point;
                        item.isDone = true;
                        board.DestoryItem(item);
                        board.scoreTracker += point;
                        board.LevelInfo.SetscoreTrack();
                    }
                }
                possibleMatchContainer.Clear();
                board.GameStateIdentifier = Board.GameState.operatable;
            }

    
        }

    }

    public void gameStatusCheck()
    {
        GameContoller.CheckEnd();
    }
}

