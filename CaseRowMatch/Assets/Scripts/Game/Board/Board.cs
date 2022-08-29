using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public List<string> _levelItems = new List<string>();
    public int _moveCount;
    public int _levelCount;
    public int _width;
    public int _heigth;
    public int scoreTracker;
    public int highestAttained;

    public GameObject prefabTile;
    public LevelInfo LevelInfo;
    public enum GameState { operatable, animating, end};
    public GameState GameStateIdentifier;

    public MoveableItem[,] itemsPos;
    public List<MoveableItem> items;
    public float speedOfItem;
    public Item tick;
    public LevelProvider LevelProvider;

    private const float TweenDuration = 0.5f;

    public RowMatch RowMatchFinder;
    IDictionary<string, int> itemMap = new Dictionary<string, int>();


    void Start()
    {
        RowMatchFinder = FindObjectOfType<RowMatch>();
        LevelProvider = FindObjectOfType<LevelProvider>();
        MapSetter();
        LevelProvider.LevelInfoFetch(PlayerPrefs.GetInt("LevelToStart"), Setup);
    }

    private void MapSetter()
    {
        itemMap.Add("b", 0);
        itemMap.Add("g", 1);
        itemMap.Add("r", 2);
        itemMap.Add("y", 3);
    }

    private int ItemPoint(string item)
    {
        switch (item)
        {
            case "r":
                return 100;
            case "g":
                return 150;
            case "b":
                return 200;
            case "y":
                return 250;
            default:
                return -1;
        }
    }

    private void Setup()
    {
        _levelCount = LevelProvider.level_number;
        _width = LevelProvider.grid_width;
        _heigth = LevelProvider.grid_height;
        _moveCount = LevelProvider.move_count;
        _levelItems = LevelProvider._grid;
        highestAttained = LevelProvider.highestScore;

        this.itemsPos = new MoveableItem[this._width, this._heigth];
        int indexToFollow = 0;
        for (int i = 0; i < _width; i++)
        {
            for(int j = 0; j < _heigth; j++)
            {
                ItemSelecter(i, j, indexToFollow);
                indexToFollow++;
            }
        }

        PrepareSceneAllignment();
        GameStateIdentifier = GameState.operatable;

    }

    public void PrepareSceneAllignment()
    {
        var cam = Camera.main;
        float _xTransform;
        float _yTransform;
        this.transform.localScale = new Vector2(0.55f, 0.55f);
        var renderingDimensions = cam.ScreenToWorldPoint(new Vector2(Display.main.renderingWidth, Display.main.renderingHeight));
        if(_width <= 5)
        {
            _xTransform = (renderingDimensions.x / -2) + ((float)_width / 10);
        }
        else
        {
            _xTransform = (renderingDimensions.x / -2) - ((float)_width / 10);
        }

        if(_heigth <= 5)
        {
            _yTransform = (renderingDimensions.y / -3);
        }
        else
        {
            _yTransform = (renderingDimensions.y / -2);
        }

        this.transform.position = new Vector2(_xTransform, _yTransform);

        LevelInfo.Setup();
    }

    private void ItemSelecter(int i, int  j, int indexToFollow)
    {
        Vector2 posTemp = new Vector2(i, j);
        GameObject tile = Instantiate(prefabTile, posTemp, Quaternion.identity);
        tile.transform.parent = transform; //parent of the game object as board object
        tile.name = "Tile_Object" + "(" + i + "," + j + ")";
        int itemToUse = itemMap[_levelItems[indexToFollow]];
        int itemPoint = ItemPoint(_levelItems[indexToFollow]);
        ItemSpawner(new Vector2Int(i, j), items[itemToUse], itemPoint);
    }
    private void ItemSpawner(Vector2Int posItem, Item spawnType, int itemPoint)
    {
        MoveableItem item = Instantiate(spawnType,new Vector3(posItem.x, posItem.y ,0f), Quaternion.identity) as MoveableItem; 
        item.transform.parent = this.transform;

        item.point = itemPoint;
        item.name = "Item_Object" + "(" + posItem.x + "," + posItem.y + ")";

        this.itemsPos[posItem.x, posItem.y] = item;
        item.PositionSet(posItem, this);
    }

    public async void DestoryItem(Item itemToDestory)
    {
        var makeitDestory = DOTween.Sequence();
        makeitDestory.Join(itemToDestory.transform.DOScale(Vector3.zero, TweenDuration));
        await makeitDestory.Play().AsyncWaitForCompletion();

        this.itemsPos[itemToDestory.indexPos.x, itemToDestory.indexPos.y] = null;
        Destroy(itemToDestory.gameObject);
        
        await RowTickSpawn(itemToDestory.transform.position);

    }

    private async Task RowTickSpawn(Vector2 position)
    {
        Item tick = Instantiate(this.tick, position, Quaternion.identity);
        tick.transform.parent = this.transform;
        tick.name = "Tick - " + position.x + ", " + position.y;
        tick.isDone = true;
        tick.PositionSet(new Vector2Int((int)position.x, (int)position.y), this);

        var makeitLarge = DOTween.Sequence();
        makeitLarge.Join(tick.transform.DOScale(new Vector3(1.25f, 1.25f, 1f), TweenDuration));
        await makeitLarge.Play().AsyncWaitForCompletion();
    }


}
