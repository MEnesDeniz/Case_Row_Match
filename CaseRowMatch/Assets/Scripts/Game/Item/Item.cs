using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


    public class Item : MonoBehaviour
    {

        public Vector2Int indexPos;
        public Board board;
        public enum ItemTypes { blue, green, red, yellow, tick };
        public ItemTypes typeOfItem;
        public bool isDone;
        public int point;


        public virtual void PositionSet(Vector2Int position, Board board)
        {
            this.indexPos = position;
            this.board = board;
            this.isDone = true;
        }

    }
